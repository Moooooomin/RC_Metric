using UnityEngine;

public class Movement : MonoBehaviour
{
    [Header("=== 차량 종류 ===")]
    public EngineType engineType = EngineType.Motor;
    public DriveType driveType = DriveType.RWD;
    
    [Header("=== 차량 기본 설정 ===")]
    [Tooltip("차량 무게 (권장: 15kg)")]
    [Range(5f, 50f)]
    public float vehicleMass = 15.0f;
    
    [Tooltip("무게중심 (낮을수록 안정적, 권장: Y=-0.2)")]
    public Vector3 centerOfMass = new Vector3(0f, -0.2f, 0f);
    
    [Header("=== 휠 콜라이더 서스펜션 설정 ===")]
    [Tooltip("서스펜션 거리 (권장: 0.2m)")]
    public float suspensionDistance = 0.2f;
    
    [Tooltip("스프링 강도 (권장: 35000) - 차체 무게 지탱")]
    public float springStrength = 35000f; 
    
    [Tooltip("댐퍼 강도 (권장: 4500) - 튀는 것 방지")]
    public float damperStrength = 4500f;
    
    [Tooltip("타겟 포지션 (권장: 0.5)")]
    public float targetPosition = 0.5f;
    
    [Tooltip("휠 반지름 (권장: 0.08m = 8cm)")]
    public float wheelRadius = 0.08f;
    
    [Tooltip("휠 질량 (권장: 1kg)")]
    public float wheelMass = 1.0f;
    
    [Header("=== 모터/엔진 설정 ===")]
    [Tooltip("모터: 빠른 초반 가속, 낮은 최고속도 (권장: 30km/h)")]
    public float motorMaxSpeed = 30f;
    
    [Tooltip("모터 토크 (권장: 150)")]
    public float motorTorque = 150f;
    public AnimationCurve motorTorqueCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0.3f);
    
    [Tooltip("엔진: 리니어한 가속, 높은 최고속도 (권장: 50km/h)")]
    public float engineMaxSpeed = 50f;
    
    [Tooltip("엔진 토크 (권장: 100)")]
    public float engineTorque = 100f;
    public AnimationCurve engineTorqueCurve = AnimationCurve.Linear(0f, 0.5f, 1f, 1f);
    
    [Header("=== 조향 설정 ===")]
    [Tooltip("최대 조향 각도 (권장: 35도)")]
    public float maxSteeringAngle = 35f;
    
    [Tooltip("조향 속도 (권장: 3)")]
    public float steeringSpeed = 3f;
    
    [Header("=== 브레이크 설정 ===")]
    [Tooltip("브레이크 힘 (권장: 500)")]
    public float brakeForce = 500f;
    
    [Tooltip("자연 감속 배수 (권장: 2)")]
    public float decelerationMultiplier = 2f;
    
    [Header("=== 휠 콜라이더 ===")]
    public WheelCollider frontLeftWheel;
    public WheelCollider frontRightWheel;
    public WheelCollider rearLeftWheel;
    public WheelCollider rearRightWheel;
    
    [Header("=== 휠 메시 (선택사항) ===")]
    public GameObject frontLeftMesh;
    public GameObject frontRightMesh;
    public GameObject rearLeftMesh;
    public GameObject rearRightMesh;
    
    [Header("=== 스키드 마크 ===")]
    public bool useSkidMarks = true;
    public TrailRenderer frontLeftSkid;
    public TrailRenderer frontRightSkid;
    public TrailRenderer rearLeftSkid;
    public TrailRenderer rearRightSkid;
    [Range(0f, 1f)]
    public float skidThreshold = 0.4f; // 스키드 마크 생성 임계값
    
    [Header("=== 표면 설정 ===")]
    public PhysicsMaterial normalSurface;
    public PhysicsMaterial icySurface;
    public PhysicsMaterial oilySurface;
    
    // Private 변수들
    private Rigidbody rb;
    private float currentSpeed;
    private float throttleInput;
    private float steeringInput;
    private float brakeInput;
    private bool isGrounded;
    private float currentSteerAngle;
    
    // 휠 프릭션 데이터
    private WheelFrictionCurve[] defaultForwardFriction = new WheelFrictionCurve[4];
    private WheelFrictionCurve[] defaultSidewaysFriction = new WheelFrictionCurve[4];
    
    public enum EngineType
    {
        Motor,  // 전기 모터 - 초반 강한 토크
        Engine  // 엔진 - 리니어한 토크, 높은 최고속도
    }
    
    public enum DriveType
    {
        FWD,    // 전륜구동
        RWD,    // 후륜구동
        AWD     // 4륜구동
    }
    
    void Start()
    {
        // Rigidbody 가져오기 (자동 생성 안함 - 수동으로 추가 필요)
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("[Movement] Rigidbody가 없습니다! Car 오브젝트에 Rigidbody를 추가하세요.");
            return;
        }
        
        // Center of Mass만 적용 (나머지는 Inspector에서 수동 설정)
        rb.centerOfMass = centerOfMass;
        
        // 기본 휠 프릭션 저장
        SaveDefaultWheelFriction();
        
        // 트레일 렌더러 초기화
        InitializeSkidMarks();
    }
    
    void Update()
    {
        // 입력 받기
        GetInput();
        
        // 휠 메시 애니메이션
        AnimateWheels();
        
        // 스키드 마크 처리
        HandleSkidMarks();
    }
    
    void FixedUpdate()
    {
        // 현재 속도 계산 (km/h)
        currentSpeed = rb.linearVelocity.magnitude * 3.6f;
        
        // 지면 체크
        CheckGroundStatus();
        
        // 과도한 Y축 속도 강제 제한 (통통 튀는 것 완전 방지)
        Vector3 vel = rb.linearVelocity;
        if (vel.y > 2f)
        {
            vel.y = 2f;
            rb.linearVelocity = vel;
        }
        else if (vel.y < -5f)
        {
            vel.y = -5f;
            rb.linearVelocity = vel;
        }
        
        // 지면에 있을 때 강한 아래 방향 힘 추가 (접지력 대폭 향상)
        if (isGrounded)
        {
            rb.AddForce(-transform.up * vehicleMass * 15f, ForceMode.Force);
        }
        
        // 조향
        HandleSteering();
        
        // 가속/감속
        HandleMotor();
        
        // 브레이크
        HandleBrake();
        
        // RC카 특유의 가벼움 - 공중에서 회전하기 쉽게
        if (!isGrounded)
        {
            rb.AddTorque(steeringInput * 3f * Vector3.up, ForceMode.Acceleration);
        }
    }
    
    void GetInput()
    {
        // WASD 또는 화살표 키로 입력
        throttleInput = Input.GetAxis("Vertical");
        steeringInput = Input.GetAxis("Horizontal");
        brakeInput = Input.GetKey(KeyCode.Space) ? 1f : 0f;
    }
    
    void HandleSteering()
    {
        // 부드러운 스티어링
        float targetSteerAngle = maxSteeringAngle * steeringInput;
        currentSteerAngle = Mathf.Lerp(currentSteerAngle, targetSteerAngle, Time.fixedDeltaTime * steeringSpeed);
        
        frontLeftWheel.steerAngle = currentSteerAngle;
        frontRightWheel.steerAngle = currentSteerAngle;
    }
    
    void HandleMotor()
    {
        if (throttleInput == 0 && brakeInput == 0)
        {
            // 자연 감속
            ApplyDeceleration();
            return;
        }
        
        float maxSpeed = engineType == EngineType.Motor ? motorMaxSpeed : engineMaxSpeed;
        float torque = engineType == EngineType.Motor ? motorTorque : engineTorque;
        AnimationCurve torqueCurve = engineType == EngineType.Motor ? motorTorqueCurve : engineTorqueCurve;
        
        // 속도에 따른 토크 조절
        float speedRatio = Mathf.Abs(currentSpeed) / maxSpeed;
        float torqueMultiplier = torqueCurve.Evaluate(speedRatio);
        
        // 최고 속도 제한
        if (throttleInput > 0 && currentSpeed >= maxSpeed)
        {
            torqueMultiplier = 0f;
        }
        else if (throttleInput < 0 && currentSpeed <= -maxSpeed * 0.5f) // 후진은 절반 속도
        {
            torqueMultiplier = 0f;
        }
        
        float finalTorque = throttleInput * torque * torqueMultiplier;
        
        // 구동 방식에 따라 토크 배분
        switch (driveType)
        {
            case DriveType.FWD:
                frontLeftWheel.motorTorque = finalTorque;
                frontRightWheel.motorTorque = finalTorque;
                rearLeftWheel.motorTorque = 0f;
                rearRightWheel.motorTorque = 0f;
                break;
                
            case DriveType.RWD:
                frontLeftWheel.motorTorque = 0f;
                frontRightWheel.motorTorque = 0f;
                rearLeftWheel.motorTorque = finalTorque;
                rearRightWheel.motorTorque = finalTorque;
                break;
                
            case DriveType.AWD:
                float torquePerWheel = finalTorque * 0.5f;
                frontLeftWheel.motorTorque = torquePerWheel;
                frontRightWheel.motorTorque = torquePerWheel;
                rearLeftWheel.motorTorque = torquePerWheel;
                rearRightWheel.motorTorque = torquePerWheel;
                break;
        }
    }
    
    void HandleBrake()
    {
        float brake = brakeInput * brakeForce;
        
        frontLeftWheel.brakeTorque = brake;
        frontRightWheel.brakeTorque = brake;
        rearLeftWheel.brakeTorque = brake;
        rearRightWheel.brakeTorque = brake;
    }
    
    void ApplyDeceleration()
    {
        // 모든 휠에 모터 토크 0
        frontLeftWheel.motorTorque = 0f;
        frontRightWheel.motorTorque = 0f;
        rearLeftWheel.motorTorque = 0f;
        rearRightWheel.motorTorque = 0f;
        
        // 약간의 브레이크로 자연스러운 감속
        float decelBrake = decelerationMultiplier * 50f;
        frontLeftWheel.brakeTorque = decelBrake;
        frontRightWheel.brakeTorque = decelBrake;
        rearLeftWheel.brakeTorque = decelBrake;
        rearRightWheel.brakeTorque = decelBrake;
    }
    
    void CheckGroundStatus()
    {
        WheelHit hit;
        isGrounded = frontLeftWheel.GetGroundHit(out hit) || 
                     frontRightWheel.GetGroundHit(out hit) ||
                     rearLeftWheel.GetGroundHit(out hit) || 
                     rearRightWheel.GetGroundHit(out hit);
    }
    
    void AnimateWheels()
    {
        if (frontLeftMesh != null) AnimateWheel(frontLeftWheel, frontLeftMesh);
        if (frontRightMesh != null) AnimateWheel(frontRightWheel, frontRightMesh);
        if (rearLeftMesh != null) AnimateWheel(rearLeftWheel, rearLeftMesh);
        if (rearRightMesh != null) AnimateWheel(rearRightWheel, rearRightMesh);
    }
    
    void AnimateWheel(WheelCollider wheelCollider, GameObject wheelMesh)
    {
        Vector3 position;
        Quaternion rotation;
        wheelCollider.GetWorldPose(out position, out rotation);
        wheelMesh.transform.position = position;
        wheelMesh.transform.rotation = rotation;
    }
    
    void SaveDefaultWheelFriction()
    {
        WheelCollider[] wheels = { frontLeftWheel, frontRightWheel, rearLeftWheel, rearRightWheel };
        
        for (int i = 0; i < wheels.Length; i++)
        {
            if (wheels[i] != null)
            {
                defaultForwardFriction[i] = wheels[i].forwardFriction;
                defaultSidewaysFriction[i] = wheels[i].sidewaysFriction;
            }
        }
    }
    
    void InitializeSkidMarks()
    {
        if (!useSkidMarks) return;
        
        TrailRenderer[] skids = { frontLeftSkid, frontRightSkid, rearLeftSkid, rearRightSkid };
        
        foreach (var skid in skids)
        {
            if (skid != null)
            {
                skid.emitting = false;
            }
        }
    }
    
    void HandleSkidMarks()
    {
        if (!useSkidMarks) return;
        
        HandleWheelSkid(frontLeftWheel, frontLeftSkid);
        HandleWheelSkid(frontRightWheel, frontRightSkid);
        HandleWheelSkid(rearLeftWheel, rearLeftSkid);
        HandleWheelSkid(rearRightWheel, rearRightSkid);
    }
    
    void HandleWheelSkid(WheelCollider wheel, TrailRenderer skid)
    {
        if (wheel == null || skid == null) return;
        
        WheelHit hit;
        if (wheel.GetGroundHit(out hit))
        {
            // 전방 슬립과 측면 슬립 체크
            float totalSlip = Mathf.Abs(hit.forwardSlip) + Mathf.Abs(hit.sidewaysSlip);
            
            if (totalSlip > skidThreshold)
            {
                skid.emitting = true;
            }
            else
            {
                skid.emitting = false;
            }
        }
        else
        {
            skid.emitting = false;
        }
    }
    
    // 표면 재질에 따른 물리 변경
    public void ApplySurfacePhysics(SurfaceType surfaceType)
    {
        WheelCollider[] wheels = { frontLeftWheel, frontRightWheel, rearLeftWheel, rearRightWheel };
        
        foreach (var wheel in wheels)
        {
            if (wheel == null) continue;
            
            WheelFrictionCurve forwardFriction = wheel.forwardFriction;
            WheelFrictionCurve sidewaysFriction = wheel.sidewaysFriction;
            
            switch (surfaceType)
            {
                case SurfaceType.Normal:
                    // 기본 프릭션
                    forwardFriction.stiffness = 1.0f;
                    sidewaysFriction.stiffness = 1.0f;
                    break;
                    
                case SurfaceType.Ice:
                    // 빙판 - 매우 미끄러움
                    forwardFriction.stiffness = 0.3f;
                    sidewaysFriction.stiffness = 0.3f;
                    forwardFriction.extremumSlip = 0.8f;
                    sidewaysFriction.extremumSlip = 0.8f;
                    break;
                    
                case SurfaceType.Oil:
                    // 기름 - 미끄러움
                    forwardFriction.stiffness = 0.5f;
                    sidewaysFriction.stiffness = 0.4f;
                    forwardFriction.extremumSlip = 0.6f;
                    sidewaysFriction.extremumSlip = 0.7f;
                    break;
                    
                case SurfaceType.Gravel:
                    // 자갈 - 약간 불안정
                    forwardFriction.stiffness = 0.8f;
                    sidewaysFriction.stiffness = 0.7f;
                    break;
            }
            
            wheel.forwardFriction = forwardFriction;
            wheel.sidewaysFriction = sidewaysFriction;
        }
    }
    
    public enum SurfaceType
    {
        Normal,
        Ice,
        Oil,
        Gravel
    }
    
    // 디버그 정보
    void OnGUI()
    {
        if (Debug.isDebugBuild)
        {
            GUIStyle style = new GUIStyle();
            style.fontSize = 20;
            style.normal.textColor = Color.white;
            
            GUI.Label(new Rect(10, 10, 400, 30), $"속도: {currentSpeed:F1} km/h", style);
            GUI.Label(new Rect(10, 40, 400, 30), $"엔진 타입: {engineType}", style);
            GUI.Label(new Rect(10, 70, 400, 30), $"구동 방식: {driveType}", style);
            GUI.Label(new Rect(10, 100, 400, 30), $"지면 접촉: {isGrounded}", style);
        }
    }
}
