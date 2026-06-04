using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class RataControl : MonoBehaviour
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

        if (Keyboard.current.leftArrowKey.isPressed)
        {
            movimiento = -1f;
        }
        if (Keyboard.current.rightArrowKey.isPressed)
        {
            movimiento = 1f;
        }

        rb.linearVelocity = new Vector2(movimiento * velocidad, rb.linearVelocity.y);

        if (Keyboard.current.upArrowKey.wasPressedThisFrame && enSuelo)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, fuerzaSalto);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Piso"))
        {
            enSuelo = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Piso"))
        {
            enSuelo = false;
        }
    }
}