using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class VagabundoControl : MonoBehaviour
{
    public float velocidad = 5f;
    public float fuerzaSalto = 7f;
    public float velocidadExtra = 3f;

    [Header("Sonidos")]
    public AudioClip sonidoSalto;
    public AudioClip sonidoLata;

    [Header("Detección de empuje (Raycast)")]
    public LayerMask layerCaja;
    public float distanciaRaycast = 0.3f;
    public Vector2 offsetRaycast = new Vector2(0.4f, 0f);

    [Header("Detección de suelo (Raycast)")]
    public LayerMask layerSuelo;
    public float distanciaSuelo = 0.15f;
    public Vector2 offsetSuelo = new Vector2(0f, -0.5f);

    private Rigidbody2D rb;
    private bool enSuelo;
    private Animator anim;
    private AudioSource audioSource;

    private float escala = 0.349641f;
    private float direccionMirando = 1f;

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
            if (Keyboard.current.aKey.isPressed)
            {
                movimiento = -1f;
            }

            if (Keyboard.current.dKey.isPressed)
            {
                movimiento = 1f;
            }
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

        if (!bloqueado &&
            Keyboard.current.wKey.wasPressedThisFrame &&
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

    private bool DetectarCajaConRaycast(float movimiento)
    {
        if (Mathf.Abs(movimiento) < 0.01f) return false;

        Vector2 origen = (Vector2)transform.position + offsetRaycast * direccionMirando;

        RaycastHit2D hit = Physics2D.Raycast(
            origen,
            Vector2.right * direccionMirando,
            distanciaRaycast,
            layerCaja
        );

        return hit.collider != null;
    }

    private IEnumerator SecuenciaLata()
    {
        bloqueado = true;

        rb.linearVelocity = Vector2.zero;

        if (anim != null)
        {
            anim.SetTrigger("BeberLata");
        }

        if (audioSource != null && sonidoLata != null)
        {
            audioSource.PlayOneShot(sonidoLata);
        }

        yield return new WaitForSeconds(1f);

        bloqueado = false;

        StartCoroutine(VelocidadTemporal());
    }

    private IEnumerator VelocidadTemporal()
    {
        velocidad += velocidadExtra;

        yield return new WaitForSeconds(5f);

        velocidad -= velocidadExtra;
    }

    public void BeberLata()
    {
        StopAllCoroutines();
        StartCoroutine(SecuenciaLata());
    }

    private void OnDrawGizmos()
    {
        Vector2 origenCaja = (Vector2)transform.position +
                             offsetRaycast * direccionMirando;

        Gizmos.color = Color.red;

        Gizmos.DrawLine(
            origenCaja,
            origenCaja + Vector2.right * direccionMirando * distanciaRaycast
        );

        Vector2 origenSuelo = (Vector2)transform.position + offsetSuelo;

        Gizmos.color = Color.green;

        Gizmos.DrawLine(
            origenSuelo,
            origenSuelo + Vector2.down * distanciaSuelo
        );
    }
}