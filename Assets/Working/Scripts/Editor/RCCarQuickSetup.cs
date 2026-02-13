using UnityEngine;
using UnityEditor;

/// <summary>
/// RC카 셋업을 한번에 해결하는 에디터 도구
/// 메뉴: Tools → RC Car → Quick Setup
/// </summary>
public class RCCarQuickSetup : EditorWindow
{
    private GameObject carObject;
    private GameObject groundObject;
    
    [MenuItem("Tools/RC Car/Quick Setup")]
    static void OpenWindow()
    {
        RCCarQuickSetup window = GetWindow<RCCarQuickSetup>();
        window.titleContent = new GUIContent("RC Car Quick Setup");
        window.minSize = new Vector2(400, 500);
        window.Show();
    }
    
    void OnGUI()
    {
        GUILayout.Label("🚗 RC Car 빠른 셋업", EditorStyles.boldLabel);
        EditorGUILayout.Space();
        
        EditorGUILayout.HelpBox(
            "이 도구는 RC카와 지형을 자동으로 설정합니다.\n" +
            "1. CAR 오브젝트 선택\n" +
            "2. Ground 오브젝트 선택 (없으면 자동 생성)\n" +
            "3. 'Setup Everything' 클릭", 
            MessageType.Info);
        
        EditorGUILayout.Space();
        
        // CAR 오브젝트 선택
        carObject = (GameObject)EditorGUILayout.ObjectField(
            "CAR Object", 
            carObject, 
            typeof(GameObject), 
            true);
        
        // Ground 오브젝트 선택
        groundObject = (GameObject)EditorGUILayout.ObjectField(
            "Ground Object (선택사항)", 
            groundObject, 
            typeof(GameObject), 
            true);
        
        EditorGUILayout.Space();
        
        // 전체 셋업 버튼
        GUI.backgroundColor = Color.green;
        if (GUILayout.Button("Setup Everything", GUILayout.Height(40)))
        {
            SetupEverything();
        }
        GUI.backgroundColor = Color.white;
        
        EditorGUILayout.Space();
        
        // 개별 버튼들
        EditorGUILayout.LabelField("개별 설정:", EditorStyles.boldLabel);
        
        if (GUILayout.Button("1. Ground 설정"))
        {
            SetupGround();
        }
        
        if (GUILayout.Button("2. CAR 위치 조정"))
        {
            SetupCarPosition();
        }
        
        if (GUILayout.Button("3. Rigidbody 설정"))
        {
            SetupRigidbody();
        }
        
        if (GUILayout.Button("4. WheelCollider 자동 찾기"))
        {
            FindAndAssignWheels();
        }
        
        EditorGUILayout.Space();
        
        // 진단 버튼
        GUI.backgroundColor = Color.cyan;
        if (GUILayout.Button("🔍 현재 설정 진단"))
        {
            DiagnoseCurrentSetup();
        }
        GUI.backgroundColor = Color.white;
    }
    
    void SetupEverything()
    {
        if (carObject == null)
        {
            EditorUtility.DisplayDialog("오류", "CAR Object를 먼저 선택하세요!", "확인");
            return;
        }
        
        Debug.Log("=== RC Car 자동 셋업 시작 ===");
        
        SetupGround();
        SetupCarPosition();
        SetupRigidbody();
        FindAndAssignWheels();
        AddMovementScript();
        AddWheelColliderSetup();
        
        Debug.Log("✅ 모든 설정 완료! Play 모드로 테스트하세요!");
        EditorUtility.DisplayDialog("완료", "RC Car 셋업이 완료되었습니다!\nPlay 모드로 테스트하세요.", "확인");
    }
    
    void SetupGround()
    {
        // Ground가 없으면 생성
        if (groundObject == null)
        {
            GameObject existingGround = GameObject.Find("Ground");
            if (existingGround == null)
            {
                existingGround = GameObject.Find("Plane");
            }
            
            if (existingGround != null)
            {
                groundObject = existingGround;
                Debug.Log($"✓ 기존 지형 발견: {groundObject.name}");
            }
            else
            {
                // Plane 생성
                groundObject = GameObject.CreatePrimitive(PrimitiveType.Plane);
                groundObject.name = "Ground";
                groundObject.transform.position = Vector3.zero;
                groundObject.transform.localScale = new Vector3(10, 1, 10);
                Debug.Log("✓ 새로운 Plane 생성됨");
            }
        }
        
        // GroundSetup 컴포넌트 추가
        GroundSetup groundSetup = groundObject.GetComponent<GroundSetup>();
        if (groundSetup == null)
        {
            groundSetup = groundObject.AddComponent<GroundSetup>();
            Debug.Log("✓ GroundSetup 컴포넌트 추가됨");
        }
        
        // Collider 확인
        Collider col = groundObject.GetComponent<Collider>();
        if (col == null)
        {
            MeshCollider meshCol = groundObject.AddComponent<MeshCollider>();
            Debug.Log("✓ MeshCollider 추가됨");
        }
        else
        {
            Debug.Log($"✓ Collider 이미 있음: {col.GetType().Name}");
        }
    }
    
    void SetupCarPosition()
    {
        if (carObject == null) return;
        
        Vector3 pos = carObject.transform.position;
        if (pos.y < 1.0f)
        {
            pos.y = 1.5f;
            carObject.transform.position = pos;
            Debug.Log($"✓ CAR 위치 조정: Y = {pos.y}");
        }
    }
    
    void SetupRigidbody()
    {
        if (carObject == null) return;
        
        // ⭐ 중요: Body에 Rigidbody가 있으면 제거
        Transform bodyTransform = carObject.transform.Find("Body");
        if (bodyTransform != null)
        {
            Rigidbody bodyRb = bodyTransform.GetComponent<Rigidbody>();
            if (bodyRb != null)
            {
                DestroyImmediate(bodyRb);
                Debug.Log("✓ Body의 Rigidbody 제거됨 (CAR로 이동 필요)");
            }
            
            // Body의 Mesh Collider Convex 설정
            MeshCollider bodyCollider = bodyTransform.GetComponent<MeshCollider>();
            if (bodyCollider != null)
            {
                bodyCollider.convex = true;
                Debug.Log("✓ Body의 Mesh Collider Convex 설정됨");
            }
        }
        
        // CAR에 Rigidbody 추가/설정
        Rigidbody rb = carObject.GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = carObject.AddComponent<Rigidbody>();
            Debug.Log("✓ CAR에 Rigidbody 추가됨");
        }
        
        rb.mass = 1.5f;
        rb.linearDamping = 0.05f;
        rb.angularDamping = 0.05f;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        rb.centerOfMass = new Vector3(0, -0.1f, 0);
        
        if (rb.isKinematic)
        {
            rb.isKinematic = false;
            Debug.Log("✓ Is Kinematic 해제됨");
        }
        
        Debug.Log("✓ Rigidbody 설정 완료 (CAR에만 존재)");
    }
    
    void FindAndAssignWheels()
    {
        if (carObject == null) return;
        
        Movement movement = carObject.GetComponent<Movement>();
        if (movement == null)
        {
            Debug.LogWarning("Movement 스크립트가 없습니다. 먼저 추가하세요.");
            return;
        }
        
        // WheelCollider 찾기
        WheelCollider[] allWheels = carObject.GetComponentsInChildren<WheelCollider>();
        
        if (allWheels.Length == 0)
        {
            Debug.LogError("❌ WheelCollider를 찾을 수 없습니다!");
            Debug.LogError("Colliders 폴더 아래에 WheelCollider를 추가하세요.");
            return;
        }
        
        Debug.Log($"✓ {allWheels.Length}개의 WheelCollider 발견");
        
        // 이름으로 자동 할당
        foreach (var wheel in allWheels)
        {
            string wheelName = wheel.gameObject.name.ToLower();
            
            if (wheelName.Contains("frontleft") || wheelName.Contains("fl"))
            {
                movement.frontLeftWheel = wheel;
                Debug.Log($"  - Front Left: {wheel.gameObject.name}");
            }
            else if (wheelName.Contains("frontright") || wheelName.Contains("fr"))
            {
                movement.frontRightWheel = wheel;
                Debug.Log($"  - Front Right: {wheel.gameObject.name}");
            }
            else if (wheelName.Contains("rearleft") || wheelName.Contains("rl"))
            {
                movement.rearLeftWheel = wheel;
                Debug.Log($"  - Rear Left: {wheel.gameObject.name}");
            }
            else if (wheelName.Contains("rearright") || wheelName.Contains("rr"))
            {
                movement.rearRightWheel = wheel;
                Debug.Log($"  - Rear Right: {wheel.gameObject.name}");
            }
        }
        
        EditorUtility.SetDirty(movement);
    }
    
    void AddMovementScript()
    {
        if (carObject == null) return;
        
        Movement movement = carObject.GetComponent<Movement>();
        if (movement == null)
        {
            movement = carObject.AddComponent<Movement>();
            Debug.Log("✓ Movement 스크립트 추가됨");
        }
    }
    
    void AddWheelColliderSetup()
    {
        if (carObject == null) return;
        
        WheelColliderSetup setup = carObject.GetComponent<WheelColliderSetup>();
        if (setup == null)
        {
            setup = carObject.AddComponent<WheelColliderSetup>();
            Debug.Log("✓ WheelColliderSetup 스크립트 추가됨");
        }
    }
    
    void RemoveBodyRigidbody()
    {
        if (carObject == null)
        {
            EditorUtility.DisplayDialog("오류", "CAR Object를 먼저 선택하세요!", "확인");
            return;
        }
        
        Transform bodyTransform = carObject.transform.Find("Body");
        if (bodyTransform == null)
        {
            EditorUtility.DisplayDialog("오류", "Body 오브젝트를 찾을 수 없습니다!", "확인");
            return;
        }
        
        Rigidbody bodyRb = bodyTransform.GetComponent<Rigidbody>();
        if (bodyRb != null)
        {
            DestroyImmediate(bodyRb);
            Debug.Log("✓ Body의 Rigidbody 제거 완료!");
            EditorUtility.DisplayDialog("완료", "Body의 Rigidbody가 제거되었습니다.\n이제 CAR에 Rigidbody를 추가하세요.", "확인");
        }
        else
        {
            EditorUtility.DisplayDialog("정보", "Body에 Rigidbody가 없습니다.", "확인");
        }
    }
    
    void DiagnoseCurrentSetup()
    {
        Debug.Log("\n=== 🔍 현재 설정 진단 ===\n");
        
        // Ground 체크
        if (groundObject != null)
        {
            Debug.Log($"✓ Ground: {groundObject.name}");
            Collider col = groundObject.GetComponent<Collider>();
            if (col != null)
                Debug.Log($"  - Collider: {col.GetType().Name}");
            else
                Debug.LogWarning("  ⚠️ Collider 없음!");
        }
        else
        {
            Debug.LogError("❌ Ground 오브젝트 없음!");
        }
        
        // CAR 체크
        if (carObject != null)
        {
            Debug.Log($"\n✓ CAR: {carObject.name}");
            Debug.Log($"  - Position: {carObject.transform.position}");
            
            if (carObject.transform.position.y < 0.5f)
                Debug.LogWarning($"  ⚠️ Y 위치가 너무 낮음! ({carObject.transform.position.y})");
            
            Rigidbody rb = carObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Debug.Log($"  - Rigidbody: Mass={rb.mass}, Kinematic={rb.isKinematic}");
                if (rb.isKinematic)
                    Debug.LogError("  ❌ Is Kinematic이 켜져있음!");
            }
            else
            {
                Debug.LogError("  ❌ Rigidbody 없음!");
            }
            
            Movement movement = carObject.GetComponent<Movement>();
            if (movement != null)
            {
                Debug.Log("  - Movement 스크립트: 있음");
                
                int wheelCount = 0;
                if (movement.frontLeftWheel != null) wheelCount++;
                if (movement.frontRightWheel != null) wheelCount++;
                if (movement.rearLeftWheel != null) wheelCount++;
                if (movement.rearRightWheel != null) wheelCount++;
                
                Debug.Log($"  - WheelCollider: {wheelCount}/4개 할당됨");
                
                if (wheelCount < 4)
                    Debug.LogWarning("  ⚠️ 모든 휠이 할당되지 않았습니다!");
            }
            else
            {
                Debug.LogError("  ❌ Movement 스크립트 없음!");
            }
        }
        else
        {
            Debug.LogError("❌ CAR 오브젝트 없음!");
        }
        
        Debug.Log("\n=== 진단 완료 ===\n");
    }
}



