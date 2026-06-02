using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class VagabundoControl : MonoBehaviour
{
    public float velocidad = 5f;
    public float fuerzaSalto = 7f;

    private Rigidbody2D rb;
    private bool enSuelo;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float movimiento = 0f;

        // Movimiento A / D
        if (Keyboard.current.aKey.isPressed)
        {
            movimiento = -1f;
        }
        if (Keyboard.current.dKey.isPressed)
        {
            movimiento = 1f;
        }

        rb.linearVelocity = new Vector2(movimiento * velocidad, rb.linearVelocity.y);

        // Salto con W
        if (Keyboard.current.wKey.wasPressedThisFrame && enSuelo)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, fuerzaSalto);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        enSuelo = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        enSuelo = false;
    }
}