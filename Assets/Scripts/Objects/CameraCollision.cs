using UnityEngine;

public class CameraCollision : MonoBehaviour
{
    [SerializeField] private float minDistance = 1.0f;
    [SerializeField] private float maxDistance = 5.0f;
    [SerializeField] private float smoothness = 10f;
    [SerializeField] private float distance;

    Vector3 direction;

    void Start()
    {
        direction = transform.localPosition.normalized;
        distance = transform.localPosition.magnitude;
    }
        
    void Update()
    {
        Vector3 cameraPos = transform.parent.TransformPoint(direction * maxDistance);
        RaycastHit hit;
        if (Physics.Linecast(transform.parent.position, cameraPos, out hit))
        {
            distance = Mathf.Clamp(hit.distance * 0.85f, minDistance, maxDistance);
        }
        else 
        {
            distance = maxDistance;
        }
        transform.localPosition = Vector3.Lerp(transform.localPosition, direction * distance, smoothness * Time.deltaTime);
    } 
}
