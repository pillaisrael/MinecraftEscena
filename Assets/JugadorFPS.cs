using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class JugadorFPS : MonoBehaviour
{
    public float velocidad = 6f;
    public float sensibilidad = 0.1f;
    public float salto = 6f;
    public float gravedad = -20f;
    public Transform camara;

    CharacterController cc;
    float rotX;
    float velY;

    void Start()
    {
        cc = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        var teclado = Keyboard.current;
        var mouse = Mouse.current;
        if (teclado == null || mouse == null) return;

        Vector2 delta = mouse.delta.ReadValue() * sensibilidad;
        transform.Rotate(0f, delta.x, 0f);
        rotX = Mathf.Clamp(rotX - delta.y, -85f, 85f);
        camara.localRotation = Quaternion.Euler(rotX, 0f, 0f);

        float h = (teclado.dKey.isPressed ? 1 : 0) - (teclado.aKey.isPressed ? 1 : 0);
        float v = (teclado.wKey.isPressed ? 1 : 0) - (teclado.sKey.isPressed ? 1 : 0);
        Vector3 mov = (transform.right * h + transform.forward * v).normalized * velocidad;

        if (cc.isGrounded && velY < 0) velY = -2f;
        if (cc.isGrounded && teclado.spaceKey.wasPressedThisFrame) velY = salto;
        velY += gravedad * Time.deltaTime;
        mov.y = velY;

        cc.Move(mov * Time.deltaTime);

        if (teclado.escapeKey.wasPressedThisFrame) Cursor.lockState = CursorLockMode.None;
    }
}