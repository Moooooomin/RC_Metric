using UnityEngine;

/// <summary>
/// WheelCollider 자동 설정 및 문제 진단 스크립트
/// Movement 컴포넌트와 함께 사용하여 휠 설정을 최적화합니다
/// </summary>
[RequireComponent(typeof(Movement))]
public class WheelColliderSetup : MonoBehaviour
{
    [Header("=== 자동 설정 ===")]
    public bool autoSetupOnStart = true;
    public bool showDebugInfo = true;
    
    [Header("=== 휠 기본 설정 ===")]
    [Tooltip("휠 반지름 (RC카는 0.05~0.15)")]
    public float wheelRadius = 0.1f;
    
    [Tooltip("휠 무게")]
    public float wheelMass = 0.2f;
    
    [Tooltip("서스펜션 거리 (RC카는 0.05~0.15)")]
    public float suspensionDistance = 0.1f;
    
    [Tooltip("서스펜션 스프링 강도")]
    public float suspensionSpring = 35000f;
    
    [Tooltip("서스펜션 댐퍼")]
    public float suspensionDamper = 4500f;
    
    [Tooltip("서스펜션 목표 위치 (0~1)")]
    public float suspensionTargetPosition = 0.5f;
    
    [Header("=== 프릭션 설정 ===")]
    public float forwardStiffness = 1.5f;
    public float sidewaysStiffness = 1.5f;
    
    private Movement movement;
    
    void Start()
    {
        movement = GetComponent<Movement>();
        
        if (autoSetupOnStart)
        {
            SetupAllWheels();
            DiagnoseWheels();
        }
    }
    
    public void SetupAllWheels()
    {
        if (movement == null) return;
        
        Debug.Log("[WheelSetup] 휠 콜라이더 자동 설정 시작...");
        
        SetupWheel(movement.frontLeftWheel, "FrontLeft");
        SetupWheel(movement.frontRightWheel, "FrontRight");
        SetupWheel(movement.rearLeftWheel, "RearLeft");
        SetupWheel(movement.rearRightWheel, "RearRight");
        
        Debug.Log("[WheelSetup] 모든 휠 설정 완료!");
    }
    
    void SetupWheel(WheelCollider wheel, string wheelName)
    {
        if (wheel == null)
        {
            Debug.LogWarning($"[WheelSetup] {wheelName} 휠이 할당되지 않았습니다!");
            return;
        }
        
        // 기본 설정
        wheel.radius = wheelRadius;
        wheel.mass = wheelMass;
        wheel.suspensionDistance = suspensionDistance;
        
        // 서스펜션 스프링 설정
        JointSpring spring = wheel.suspensionSpring;
        spring.spring = suspensionSpring;
        spring.damper = suspensionDamper;
        spring.targetPosition = suspensionTargetPosition;
        wheel.suspensionSpring = spring;
        
        // 전방 프릭션
        WheelFrictionCurve forwardFriction = wheel.forwardFriction;
        forwardFriction.stiffness = forwardStiffness;
        wheel.forwardFriction = forwardFriction;
        
        // 측면 프릭션
        WheelFrictionCurve sidewaysFriction = wheel.sidewaysFriction;
        sidewaysFriction.stiffness = sidewaysStiffness;
        wheel.sidewaysFriction = sidewaysFriction;
        
        Debug.Log($"[WheelSetup] {wheelName} 설정 완료 - Radius: {wheelRadius}, Suspension: {suspensionDistance}");
    }
    
    public void DiagnoseWheels()
    {
        if (movement == null) return;
        
        Debug.Log("=== 휠 콜라이더 진단 시작 ===");
        
        // 차량 위치 체크
        float carHeight = transform.position.y;
        Debug.Log($"차량 높이: {carHeight:F2}m");
        
        if (carHeight < 0.5f)
        {
            Debug.LogWarning("⚠️ 차량이 너무 낮습니다! Y 위치를 1.0 이상으로 설정하세요.");
        }
        
        // 각 휠 진단
        DiagnoseWheel(movement.frontLeftWheel, "FrontLeft");
        DiagnoseWheel(movement.frontRightWheel, "FrontRight");
        DiagnoseWheel(movement.rearLeftWheel, "RearLeft");
        DiagnoseWheel(movement.rearRightWheel, "RearRight");
        
        // 지면 체크
        CheckGroundCollider();
        
        Debug.Log("=== 휠 콜라이더 진단 완료 ===");
    }
    
    void DiagnoseWheel(WheelCollider wheel, string wheelName)
    {
        if (wheel == null)
        {
            Debug.LogError($"❌ {wheelName}: 휠이 할당되지 않음!");
            return;
        }
        
        Debug.Log($"\n--- {wheelName} 진단 ---");
        Debug.Log($"위치: {wheel.transform.position}");
        Debug.Log($"Radius: {wheel.radius}");
        Debug.Log($"Suspension Distance: {wheel.suspensionDistance}");
        Debug.Log($"Mass: {wheel.mass}");
        
        // 지면 접촉 체크
        WheelHit hit;
        bool isGrounded = wheel.GetGroundHit(out hit);
        
        if (isGrounded)
        {
            Debug.Log($"✓ 지면 접촉: {hit.collider.gameObject.name}");
            Debug.Log($"  Force: {hit.force:F1}N");
        }
        else
        {
            Debug.LogWarning($"⚠️ 지면 접촉 안됨!");
            
            // 레이캐스트로 지면 찾기
            RaycastHit rayHit;
            Vector3 wheelPos = wheel.transform.position;
            float checkDistance = suspensionDistance + wheelRadius + 1f;
            
            if (Physics.Raycast(wheelPos, Vector3.down, out rayHit, checkDistance))
            {
                float distanceToGround = rayHit.distance;
                Debug.Log($"  아래 {distanceToGround:F3}m에 지면 발견: {rayHit.collider.gameObject.name}");
                
                if (distanceToGround > suspensionDistance + wheelRadius)
                {
                    float recommended = distanceToGround - wheelRadius;
                    Debug.LogWarning($"  💡 해결책: Suspension Distance를 {recommended:F3}m로 증가시키세요");
                }
            }
            else
            {
                Debug.LogError($"  ❌ {checkDistance}m 아래에 지면이 없습니다!");
                Debug.LogError($"  💡 해결책: 차량을 더 높이 올리거나, 지면에 Collider를 추가하세요");
            }
        }
    }
    
    void CheckGroundCollider()
    {
        Debug.Log("\n--- 지면 Collider 체크 ---");
        
        // 차량 아래 지면 찾기
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 10f))
        {
            GameObject ground = hit.collider.gameObject;
            Debug.Log($"✓ 지면 발견: {ground.name}");
            Debug.Log($"  Collider 타입: {hit.collider.GetType().Name}");
            Debug.Log($"  거리: {hit.distance:F3}m");
            
            // PhysicMaterial 체크
            if (hit.collider.material != null)
            {
                Debug.Log($"  PhysicMaterial: {hit.collider.material.name}");
            }
            else
            {
                Debug.Log("  PhysicMaterial: 없음 (기본값 사용)");
            }
        }
        else
        {
            Debug.LogError("❌ 차량 아래 지면을 찾을 수 없습니다!");
            Debug.LogError("💡 해결책:");
            Debug.LogError("  1. Plane 또는 Terrain 생성");
            Debug.LogError("  2. 지형에 Collider 추가 (MeshCollider, BoxCollider 등)");
            Debug.LogError("  3. 지형의 Layer를 'Default'로 설정");
        }
    }
    
    // Scene 뷰에서 휠 위치 시각화
    void OnDrawGizmos()
    {
        if (movement == null) return;
        
        DrawWheelGizmo(movement.frontLeftWheel, Color.red);
        DrawWheelGizmo(movement.frontRightWheel, Color.green);
        DrawWheelGizmo(movement.rearLeftWheel, Color.blue);
        DrawWheelGizmo(movement.rearRightWheel, Color.yellow);
    }
    
    void DrawWheelGizmo(WheelCollider wheel, Color color)
    {
        if (wheel == null) return;
        
        Vector3 pos = wheel.transform.position;
        
        // 휠 위치
        Gizmos.color = color;
        Gizmos.DrawWireSphere(pos, wheelRadius);
        
        // 서스펜션 범위
        Gizmos.color = new Color(color.r, color.g, color.b, 0.3f);
        Gizmos.DrawLine(pos, pos + Vector3.down * (suspensionDistance + wheelRadius));
        
        // 지면 접촉 체크
        WheelHit hit;
        if (wheel.GetGroundHit(out hit))
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(hit.point, 0.05f);
        }
    }
    
    // Inspector에서 호출 가능한 메서드들
    [ContextMenu("휠 설정 적용")]
    public void ApplySettings()
    {
        SetupAllWheels();
    }
    
    [ContextMenu("휠 진단 실행")]
    public void RunDiagnosis()
    {
        DiagnoseWheels();
    }
}


