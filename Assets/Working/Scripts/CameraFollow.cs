using UnityEngine;

/// <summary>
/// RC카를 부드럽게 따라가는 카메라 스크립트
/// </summary>
public class RCCameraFollow : MonoBehaviour
{
    [Header("=== 추적 대상 ===")]
    public Transform target;
    
    [Header("=== 카메라 설정 ===")]
    public Vector3 offset = new Vector3(0f, 3f, -5f);
    public float followSpeed = 5f;
    public float rotationSpeed = 3f;
    
    [Header("=== 룩앳 설정 ===")]
    public bool useLookAt = true;
    public Vector3 lookAtOffset = new Vector3(0f, 0.5f, 2f);
    
    void LateUpdate()
    {
        if (target == null) return;
        
        // 목표 위치 계산
        Vector3 desiredPosition = target.position + target.rotation * offset;
        
        // 부드러운 이동
        transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);
        
        // 카메라 회전
        if (useLookAt)
        {
            Vector3 lookAtPoint = target.position + target.rotation * lookAtOffset;
            Quaternion desiredRotation = Quaternion.LookRotation(lookAtPoint - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, rotationSpeed * Time.deltaTime);
        }
        else
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, target.rotation, rotationSpeed * Time.deltaTime);
        }
    }
}



