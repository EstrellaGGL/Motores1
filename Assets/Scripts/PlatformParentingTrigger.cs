using StarterAssets;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlatformParenting : MonoBehaviour
{
    [Header("Configuración")]
    [Tooltip("Layer de las plataformas")]
    public LayerMask platformLayer;

    [Tooltip("Radio de detección bajo los pies")]
    public float checkRadius = 0.5f;

    [Tooltip("Distancia hacia abajo para detectar")]
    public float checkDistance = 0.2f;

    [Tooltip("Fuerza para pegar al suelo")]
    public float stickToGroundForce = 10f;

    private Transform currentPlatform;
    private Vector3 lastPlatformPosition;
    private CharacterController controller;
    private bool isGrounded = false;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Detectar plataforma bajo los pies
        Vector3 checkPosition = transform.position + Vector3.down * checkDistance;
        Collider[] colliders = Physics.OverlapSphere(checkPosition, checkRadius, platformLayer);

        Transform detectedPlatform = null;

        foreach (var col in colliders)
        {
            if (col.gameObject == gameObject) continue;
            detectedPlatform = col.transform;
            break;
        }

        // Si detectamos plataforma
        if (detectedPlatform != null)
        {
            isGrounded = true;

            if (currentPlatform != detectedPlatform)
            {
                // Nueva plataforma
                currentPlatform = detectedPlatform;
                lastPlatformPosition = detectedPlatform.position;
            }
            else
            {
                // Misma plataforma - aplicar movimiento de la plataforma
                Vector3 platformDelta = detectedPlatform.position - lastPlatformPosition;

                if (platformDelta.magnitude > 0.0001f)
                {
                    // Mover personaje con la plataforma
                    controller.Move(platformDelta);
                }

                lastPlatformPosition = detectedPlatform.position;
            }

            // StickToGround: solo si NO está caminando (input = 0)
            // Necesitamos saber si el jugador está inputeando movimiento
            ThirdPersonController player = GetComponent<ThirdPersonController>();
            if (player != null && !player.IsMoving())
            {
                // Está quieto - aplicar StickToGround
                Vector3 stickForce = Vector3.down * stickToGroundForce;
                controller.Move(stickForce * Time.deltaTime);
            }
        }
        else
        {
            isGrounded = false;
            currentPlatform = null;
        }
    }

    public bool IsGrounded()
    {
        return isGrounded;
    }
}