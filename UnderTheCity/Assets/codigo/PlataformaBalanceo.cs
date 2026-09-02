using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlataformaBalanceo : MonoBehaviour
{
    [Header("Configuración física")]
    public float masa = 3f;
    public float gravedad = 1f;
    public float resistenciaRotacion = 0.5f;

    [Header("Fuerza del jugador")]
    public float fuerzaMinima = 8f;
    public float fuerzaMaxima = 35f;
    public float zonaCentro = 0.05f;

    [Header("Regreso al centro")]
    public float tiempoAntesDeVolver = 4f;
    public float velocidadRegreso = 4f;
    public float fuerzaRegreso = 2f;

    private Rigidbody2D rb;

    private bool jugadorEncima = false;
    private float ultimoContacto;
    private float anguloInicial;

    private Collider2D plataformaCollider;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        plataformaCollider = GetComponent<Collider2D>();

        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.mass = masa;
        rb.gravityScale = gravedad;

        rb.freezeRotation = false;
        rb.angularDamping = resistenciaRotacion;

        anguloInicial = rb.rotation;
    }

    void FixedUpdate()
    {
        if (jugadorEncima)
            return;

        float tiempoDesdeContacto = Time.time - ultimoContacto;

        // Durante el tiempo de espera, frenamos la plataforma
        if (tiempoDesdeContacto < tiempoAntesDeVolver)
        {
            rb.angularVelocity = Mathf.Lerp(
                rb.angularVelocity,
                0f,
                0.05f
            );

            return;
        }

        VolverAlCentro();
    }

    private void VolverAlCentro()
    {
        float diferencia = Mathf.DeltaAngle(
            rb.rotation,
            anguloInicial
        );

        // Si estamos prácticamente en el centro,
        // frenamos suavemente.
        if (Mathf.Abs(diferencia) < 0.5f)
        {
            rb.angularVelocity = Mathf.Lerp(
                rb.angularVelocity,
                0f,
                0.1f
            );

            return;
        }

        // Fuerza progresiva para volver al centro.
        float torque = diferencia * fuerzaRegreso;

        rb.AddTorque(
            torque,
            ForceMode2D.Force
        );

        // Limitar la velocidad de regreso.
        if (Mathf.Abs(rb.angularVelocity) > velocidadRegreso)
        {
            rb.angularVelocity = Mathf.Lerp(
                rb.angularVelocity,
                Mathf.Sign(rb.angularVelocity) * velocidadRegreso,
                0.1f
            );
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
            return;

        jugadorEncima = true;

        // IMPORTANTE:
        // NO ponemos ultimoContacto aquí.
        // Si lo hacemos, el temporizador se reinicia
        // constantemente mientras el jugador esté encima.

        // Posición del jugador respecto al centro de la plataforma.
        float diferenciaX =
            collision.transform.position.x - transform.position.x;

        float distancia = Mathf.Abs(diferenciaX);

        // Pequeña zona central donde casi no gira.
        if (distancia < zonaCentro)
        {
            return;
        }

        // Mitad del ancho de la plataforma.
        float mitadPlataforma =
            plataformaCollider.bounds.extents.x;

        // Porcentaje de distancia desde el centro.
        float porcentaje =
            Mathf.Clamp01(distancia / mitadPlataforma);

        // Cuanto más lejos del centro,
        // más fuerza se aplica.
        float fuerza = Mathf.Lerp(
            fuerzaMinima,
            fuerzaMaxima,
            porcentaje
        );

        // Determinar el lado donde está el jugador.
        float direccion = Mathf.Sign(diferenciaX);

        // Aplicar torque continuamente.
        rb.AddTorque(
            -direccion * fuerza,
            ForceMode2D.Force
        );
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
            return;

        jugadorEncima = false;

        // ACÁ empieza a contar el tiempo de espera.
        ultimoContacto = Time.time;
    }
}

