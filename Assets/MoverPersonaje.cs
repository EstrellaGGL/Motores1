using UnityEngine;

public class MoverPersonaje : MonoBehaviour
{
    public float velocidad = 8f;
    public float fuerzaSalto = 7f;
    public float gravedad = -15f;
    public float sensibilidadRaton = 2f;

    private CharacterController controller;
    private Vector3 velocidadVertical;
    private Transform camaraPrincipal;
    private float rotacionX = 0f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        // Encuentra la cámara física automáticamente
        if (Camera.main != null) camaraPrincipal = Camera.main.transform;

        // Bloquea el cursor del ratón en el centro de la pantalla de juego
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // 1. ROTACIÓN CON EL RATÓN (Girar la cápsula y la cámara)
        float ratonX = Input.GetAxis("Mouse X") * sensibilidadRaton;
        transform.Rotate(Vector3.up * ratonX);

        // 2. MOVIMIENTO HORIZONTAL (WASD / Flechas)
        float moverX = Input.GetAxis("Horizontal");
        float moverZ = Input.GetAxis("Vertical");
        Vector3 movimiento = transform.right * moverX + transform.forward * moverZ;
        controller.Move(movimiento * velocidad * Time.deltaTime);

        // 3. GRAVEDAD Y SUELO
        if (controller.isGrounded && velocidadVertical.y < 0)
        {
            velocidadVertical.y = -2f;
        }

        // 4. DETECTAR EL SALTO (Barra Espaciadora)
        if (Input.GetButtonDown("Jump") && controller.isGrounded)
        {
            velocidadVertical.y = fuerzaSalto;
        }

        // Aplicar movimiento vertical completo
        velocidadVertical.y += gravedad * Time.deltaTime;
        controller.Move(velocidadVertical * Time.deltaTime);
    }
}