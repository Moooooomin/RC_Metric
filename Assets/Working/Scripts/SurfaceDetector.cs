using UnityEngine;

/// <summary>
/// 표면 재질을 감지하고 차량에 적용하는 스크립트
/// 지형의 Tag를 사용하여 표면 타입을 구분합니다
/// </summary>
public class SurfaceDetector : MonoBehaviour
{
    private Movement vehicleMovement;
    private Movement.SurfaceType currentSurface = Movement.SurfaceType.Normal;
    
    [Header("=== 표면 감지 설정 ===")]
    [Tooltip("표면 체크 빈도 (초)")]
    public float checkInterval = 0.2f;
    
    private float checkTimer;
    
    void Start()
    {
        vehicleMovement = GetComponent<Movement>();
        if (vehicleMovement == null)
        {
            Debug.LogError("SurfaceDetector: Movement 컴포넌트를 찾을 수 없습니다!");
        }
    }
    
    void Update()
    {
        checkTimer += Time.deltaTime;
        
        if (checkTimer >= checkInterval)
        {
            checkTimer = 0f;
            DetectSurface();
        }
    }
    
    void DetectSurface()
    {
        RaycastHit hit;
        
        // 차량 아래로 레이캐스트
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 2f))
        {
            Movement.SurfaceType detectedSurface = GetSurfaceType(hit.collider);
            
            // 표면이 변경되었을 때만 적용
            if (detectedSurface != currentSurface)
            {
                currentSurface = detectedSurface;
                if (vehicleMovement != null)
                {
                    vehicleMovement.ApplySurfacePhysics(currentSurface);
                }
            }
        }
    }
    
    Movement.SurfaceType GetSurfaceType(Collider hitCollider)
    {
        // Tag를 기반으로 표면 타입 결정
        switch (hitCollider.tag)
        {
            case "Ice":
            case "IceSurface":
                return Movement.SurfaceType.Ice;
                
            case "Oil":
            case "OilSurface":
                return Movement.SurfaceType.Oil;
                
            case "Gravel":
            case "GravelSurface":
                return Movement.SurfaceType.Gravel;
                
            default:
                return Movement.SurfaceType.Normal;
        }
    }
    
    // 디버그용 시각화
    void OnDrawGizmos()
    {
        Gizmos.color = GetSurfaceColor();
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * 2f);
    }
    
    Color GetSurfaceColor()
    {
        switch (currentSurface)
        {
            case Movement.SurfaceType.Ice:
                return Color.cyan;
            case Movement.SurfaceType.Oil:
                return Color.yellow;
            case Movement.SurfaceType.Gravel:
                return new Color(0.6f, 0.4f, 0.2f);
            default:
                return Color.green;
        }
    }
}


