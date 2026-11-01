using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    
    public Transform pointA; // Punto A: primera posición de la plataforma 
    public Transform pointB; // Punto B: segunda posición de la plataforma
    public float speed = 2;
    public float proximityThreshold = 0.2f; // Si la plataforma está a menos de esta distancia del punto, invierte la dirección
    private Vector3 currentTarget; // Posición actual hacia la que se mueve la plataforma (A o B)
    private Vector3 direction; // Dirección del movimiento 

    
    void Start()
    {
        pointA.parent = null; // Desparentar los puntos para que no se muevan con la plataforma
        pointB.parent = null;
        currentTarget = pointB.position; // Empezar moviéndose hacia el punto B
    }
        
    void LateUpdate()
    {
        
        float distanceToTarget = Vector3.Distance(transform.position, currentTarget); // Calcular la distancia entre la plataforma y el punto actual
                
        if (distanceToTarget < proximityThreshold) // Si la plataforma está muy cerca del punto 
        {
            
            currentTarget = currentTarget == pointA.position ? pointB.position : pointA.position; // Cambiar el punto de destino: - Si estaba yendo a A, ahora va a 
                                                                                                                              // - Si estaba yendo a B, ahora va a A
        }
              
        direction = (currentTarget - transform.position).normalized; // Calcula la dirección hacia el punto actual 
                
        transform.position += direction * speed * Time.deltaTime; // Mover la plataforma en la dirección calculada
    }
}