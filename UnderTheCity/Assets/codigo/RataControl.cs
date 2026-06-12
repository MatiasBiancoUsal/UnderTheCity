using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class RataControl : MonoBehaviour
{
    public float velocidad = 5f;
    public float fuerzaSalto = 7f;

    private Rigidbody2D rb;
    private bool enSuelo;
    private Animator anim;

    private float escalaX = 0.07186557f;
    private float escalaY = 0.08849049f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
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

        if (anim != null)
        {
            anim.SetFloat("velocidad", Mathf.Abs(movimiento));

            anim.SetBool("isJumping", !enSuelo);
        }

        Transform sprite = anim != null ? anim.transform : transform;

        if (movimiento != 0)
        {
            sprite.localScale = new Vector3(
                movimiento > 0 ? escalaX : -escalaX,
                escalaY,
                1f
            );
        }

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