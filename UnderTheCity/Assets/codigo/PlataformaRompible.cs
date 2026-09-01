using System.Collections;
using UnityEngine;

public class PlataformaRompible : MonoBehaviour
{
    [Header("TIEMPO PARADO ANTES DE ROMPERSE")]
    public float tiempoAntesDeRomper = 1.5f;

    [Header("TIEMPO ANTES DE RESPAWNEAR")]
    public float tiempoParaRespawnear = 3f;

    [Header("VELOCIDAD DE CAIDA")]
    public float gravedadAlCaer = 1f;

    private Rigidbody2D rb;
    private Collider2D col;

    private Vector3 posicionInicial;
    private bool jugadorEncima = false;
    private bool rompiendose = false;
    private Coroutine rutinaActual;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();

        posicionInicial = transform.position;

        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Kinematic;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!EsJugador(collision.collider))
            return;

        if (!SeParoEncima(collision))
            return;

        jugadorEncima = true;

        if (!rompiendose)
        {
            rutinaActual = StartCoroutine(SecuenciaDeRotura());
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (!EsJugador(collision.collider))
            return;

        jugadorEncima = false;

        if (!rompiendose && rutinaActual != null)
        {
            StopCoroutine(rutinaActual);
            rutinaActual = null;
        }
    }

    private bool EsJugador(Collider2D otro)
    {
        return otro.CompareTag("Vagabundo") || otro.CompareTag("Rata");
    }

    private bool SeParoEncima(Collision2D collision)
    {
        foreach (ContactPoint2D contacto in collision.contacts)
        {
            if (contacto.normal.y < -0.3f)
            {
                return true;
            }
        }

        return false;
    }

    private IEnumerator SecuenciaDeRotura()
    {
        rompiendose = true;

        yield return new WaitForSeconds(tiempoAntesDeRomper);

        if (!jugadorEncima)
        {
            rompiendose = false;
            rutinaActual = null;
            yield break;
        }

        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.gravityScale = gravedadAlCaer;
        }

        yield return new WaitForSeconds(tiempoParaRespawnear);

        Respawnear();
    }

    private void Respawnear()
    {

        if (col != null)
        {
            col.enabled = false;
        }

        transform.position = posicionInicial;
        transform.rotation = Quaternion.identity;

        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }

        if (col != null)
        {
            col.enabled = true;
        }

        jugadorEncima = false;
        rompiendose = false;
        rutinaActual = null;
    }
}