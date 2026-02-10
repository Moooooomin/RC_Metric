using UnityEngine;

/// <summary>
/// 지형(Plane, Terrain 등)에 자동으로 Collider를 추가하는 스크립트
/// 이 스크립트를 지형 오브젝트에 추가하면 자동으로 설정됩니다
/// </summary>
public class GroundSetup : MonoBehaviour
{
    [Header("=== Collider 설정 ===")]
    [Tooltip("자동으로 Collider 추가")]
    public bool autoAddCollider = true;
    
    [Tooltip("PhysicMaterial 적용 (선택사항)")]
    public PhysicsMaterial physicMaterial;
    
    void Awake()
    {
        if (autoAddCollider)
        {
            SetupCollider();
        }
    }
    
    void SetupCollider()
    {
        // 이미 Collider가 있는지 확인
        Collider existingCollider = GetComponent<Collider>();
        
        if (existingCollider != null)
        {
            Debug.Log($"[GroundSetup] '{gameObject.name}'에 이미 Collider가 있습니다: {existingCollider.GetType().Name}");
            ApplyPhysicMaterial(existingCollider);
            return;
        }
        
        // MeshFilter가 있으면 MeshCollider 추가
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        if (meshFilter != null)
        {
            MeshCollider meshCollider = gameObject.AddComponent<MeshCollider>();
            meshCollider.sharedMesh = meshFilter.sharedMesh;
            meshCollider.convex = false; // 지형은 일반적으로 convex가 false
            
            ApplyPhysicMaterial(meshCollider);
            Debug.Log($"[GroundSetup] '{gameObject.name}'에 MeshCollider 추가됨");
            return;
        }
        
        // Terrain 컴포넌트가 있으면 TerrainCollider 추가
        Terrain terrain = GetComponent<Terrain>();
        if (terrain != null)
        {
            TerrainCollider terrainCollider = gameObject.AddComponent<TerrainCollider>();
            terrainCollider.terrainData = terrain.terrainData;
            
            ApplyPhysicMaterial(terrainCollider);
            Debug.Log($"[GroundSetup] '{gameObject.name}'에 TerrainCollider 추가됨");
            return;
        }
        
        // 기본적으로 BoxCollider 추가
        BoxCollider boxCollider = gameObject.AddComponent<BoxCollider>();
        ApplyPhysicMaterial(boxCollider);
        Debug.Log($"[GroundSetup] '{gameObject.name}'에 BoxCollider 추가됨 (기본)");
    }
    
    void ApplyPhysicMaterial(Collider col)
    {
        if (physicMaterial != null)
        {
            col.material = physicMaterial;
            Debug.Log($"[GroundSetup] PhysicMaterial '{physicMaterial.name}' 적용됨");
        }
    }
    
    // Gizmo로 Collider 시각화
    void OnDrawGizmos()
    {
        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            Gizmos.color = new Color(0f, 1f, 0f, 0.3f);
            Gizmos.matrix = transform.localToWorldMatrix;
            
            if (col is BoxCollider)
            {
                BoxCollider box = col as BoxCollider;
                Gizmos.DrawCube(box.center, box.size);
            }
        }
    }
}


