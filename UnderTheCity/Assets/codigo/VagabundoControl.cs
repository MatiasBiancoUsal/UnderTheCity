using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class VagabundoControl : MonoBehaviour
{
    public float velocidad = 5f;
    public float fuerzaSalto = 7f;
    public float velocidadExtra = 3f;

    [Header("Detección de empuje (Raycast)")]
    public LayerMask layerCaja;
    public float distanciaRaycast = 0.3f;
    public Vector2 offsetRaycast = new Vector2(0.4f, 0f);

    private Rigidbody2D rb;
    private bool enSuelo;
    private Animator anim;

    private float escala = 0.349641f;
    private float direccionMirando = 1f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        float movimiento = 0f;

        if (Keyboard.current.aKey.isPressed)
        {
            movimiento = -1f;
        }
        if (Keyboard.current.dKey.isPressed)
        {
            movimiento = 1f;
        }

        rb.linearVelocity = new Vector2(
            movimiento * velocidad,
            rb.linearVelocity.y
        );

        if (movimiento != 0)
        {
            direccionMirando = movimiento > 0 ? 1f : -1f;
        }

        bool empujandoCaja = DetectarCajaConRaycast(movimiento);

        if (anim != null)
        {
            anim.SetFloat("velocidad", Mathf.Abs(movimiento));

            anim.SetBool("isJumping", !enSuelo);

            anim.SetBool("Empujando", empujandoCaja);
        }

        Transform sprite = anim != null ? anim.transform : transform;

        if (movimiento != 0)
        {
            sprite.localScale = new Vector3(
                movimiento > 0 ? escala : -escala,
                escala,
                1f
            );
        }

        if (Keyboard.current.wKey.wasPressedThisFrame && enSuelo)
        {
            rb.linearVelocity = new Vector2(
                rb.linearVelocity.x,
                fuerzaSalto
            );
        }
    }

    private bool DetectarCajaConRaycast(float movimiento)
    {
        if (Mathf.Abs(movimiento) < 0.01f) return false;

        Vector2 origen = (Vector2)transform.position + offsetRaycast * direccionMirando;
        RaycastHit2D hit = Physics2D.Raycast(origen, Vector2.right * direccionMirando, distanciaRaycast, layerCaja);

        return hit.collider != null;
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

    public void ActivarVelocidadTemporal()
    {
        StopAllCoroutines();
        StartCoroutine(VelocidadTemporal());
    }

    private System.Collections.IEnumerator VelocidadTemporal()
    {
        velocidad += velocidadExtra;

        yield return new WaitForSeconds(5f);

        velocidad -= velocidadExtra;
    }

    private void OnDrawGizmos()
    {
        Vector2 origen = (Vector2)transform.position + offsetRaycast * direccionMirando;
        Gizmos.color = Color.red;
        Gizmos.DrawLine(origen, origen + Vector2.right * direccionMirando * distanciaRaycast);
    }
}