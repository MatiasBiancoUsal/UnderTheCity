using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class RataControl : MonoBehaviour
{
    public float velocidad = 5f;
    public float fuerzaSalto = 7f;

    [Header("Sonidos")]
    public AudioClip sonidoSalto;

    [Header("Detección de suelo (Raycast)")]
    public LayerMask layerSuelo;
    public float distanciaSuelo = 0.15f;
    public Vector2 offsetSuelo = new Vector2(0f, -0.5f);

    private Rigidbody2D rb;
    private bool enSuelo;
    private bool empujandoCaja;

    private Animator anim;
    private AudioSource audioSource;

    private float escalaX = 0.07186557f;
    private float escalaY = 0.08849049f;

    private bool bloqueado = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        enSuelo = DetectarSuelo();

        float movimiento = 0f;

        if (!bloqueado)
        {
            if (Keyboard.current.leftArrowKey.isPressed)
            {
                movimiento = -1f;
            }

            if (Keyboard.current.rightArrowKey.isPressed)
            {
                movimiento = 1f;
            }
        }

        rb.linearVelocity = new Vector2(
            movimiento * velocidad,
            rb.linearVelocity.y
        );

        if (anim != null)
        {
            if (!bloqueado)
            {
                anim.SetFloat(
                    "velocidad",
                    Mathf.Abs(movimiento)
                );

                anim.SetBool(
                    "isJumping",
                    !enSuelo
                );

                anim.SetBool(
                    "isPushing",
                    empujandoCaja && movimiento != 0
                );
            }
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

        if (!bloqueado &&
            Keyboard.current.upArrowKey.wasPressedThisFrame &&
            enSuelo)
        {
            rb.linearVelocity = new Vector2(
                rb.linearVelocity.x,
                fuerzaSalto
            );

            if (audioSource != null && sonidoSalto != null)
            {
                audioSource.PlayOneShot(sonidoSalto);
            }
        }
    }

    private bool DetectarSuelo()
    {
        Vector2 origen = (Vector2)transform.position + offsetSuelo;

        RaycastHit2D hit = Physics2D.Raycast(
            origen,
            Vector2.down,
            distanciaSuelo,
            layerSuelo
        );

        return hit.collider != null;
    }

    public void BloquearParaMaquina()
    {
        bloqueado = true;

        rb.linearVelocity = new Vector2(
            0f,
            rb.linearVelocity.y
        );
    }

    public void DesbloquearParaMaquina()
    {
        bloqueado = false;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Caja"))
        {
            empujandoCaja = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Caja"))
        {
            empujandoCaja = false;
        }
    }

    private void OnDrawGizmos()
    {
        Vector2 origenSuelo = (Vector2)transform.position + offsetSuelo;

        Gizmos.color = Color.green;

        Gizmos.DrawLine(
            origenSuelo,
            origenSuelo + Vector2.down * distanciaSuelo
        );
    }
}