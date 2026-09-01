using UnityEngine;
using System.Collections.Generic;

// IMPORTANTE: la plataforma necesita un Rigidbody2D en modo Kinematic
// para que los eventos de colisión (OnCollisionEnter2D/Exit2D) funcionen
// bien mientras la movemos por código (rotación) en vez de con física.
[RequireComponent(typeof(Rigidbody2D))]
public class PlataformaBalanceo : MonoBehaviour
{
    [Header("Inclinación")]
    public float anguloMaximo = 25f;
    public float velocidadRotacion = 2f;

    [Tooltip("Diferencia de 'torque' (peso x distancia) necesaria para llegar al ángulo máximo. Subilo si se inclina demasiado rápido/brusco, bajalo si casi no se mueve.")]
    public float torqueParaAnguloMaximo = 5f;

    private float anguloObjetivo = 0f;

    // Guardamos quién está parado encima y su componente de peso
    private Dictionary<Transform, PesoObjeto> objetosEncima = new Dictionary<Transform, PesoObjeto>();

    void Awake()
    {
        // Nos aseguramos de que el Rigidbody2D sea Kinematic:
        // no queremos que la física mueva la plataforma sola,
        // solo que detecte colisiones mientras la rotamos nosotros.
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
    }

    void Update()
    {
        CalcularAnguloObjetivo();

        float anguloActual = transform.localEulerAngles.z;
        if (anguloActual > 180f)
        {
            anguloActual -= 360f;
        }

        float nuevoAngulo = Mathf.Lerp(
            anguloActual,
            anguloObjetivo,
            velocidadRotacion * Time.deltaTime
        );

        transform.localRotation = Quaternion.Euler(0f, 0f, nuevoAngulo);
    }

    private void CalcularAnguloObjetivo()
    {
        float torqueTotal = 0f;

        foreach (KeyValuePair<Transform, PesoObjeto> kvp in objetosEncima)
        {
            Transform objeto = kvp.Key;
            float peso = kvp.Value.peso;

            // Distancia horizontal del personaje al centro de la plataforma.
            // Positivo = está a la derecha, negativo = está a la izquierda.
            float distanciaAlCentro = objeto.position.x - transform.position.x;

            // Cada personaje aporta peso x distancia (como un sube y baja real)
            torqueTotal += peso * distanciaAlCentro;
        }

        // Si el torque es positivo (peso a la derecha), la plataforma
        // debe bajar del lado derecho -> ángulo negativo. Por eso el signo negativo.
        anguloObjetivo = Mathf.Clamp(
            -torqueTotal / torqueParaAnguloMaximo * anguloMaximo,
            -anguloMaximo,
            anguloMaximo
        );
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PesoObjeto peso = collision.gameObject.GetComponent<PesoObjeto>();

        if (peso != null && !objetosEncima.ContainsKey(collision.transform))
        {
            objetosEncima.Add(collision.transform, peso);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (objetosEncima.ContainsKey(collision.transform))
        {
            objetosEncima.Remove(collision.transform);
        }
    }
}