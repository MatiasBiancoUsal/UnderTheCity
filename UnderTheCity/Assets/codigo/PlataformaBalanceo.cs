using UnityEngine;
using System.Collections.Generic;

// IMPORTANTE: la plataforma necesita un Rigidbody2D en modo Kinematic
// para que los eventos de colisión (OnCollisionEnter2D/Exit2D) funcionen
// bien mientras la movemos por código (rotación) en vez de con física.
[RequireComponent(typeof(Rigidbody2D))]
public class PlataformaBalanceo : MonoBehaviour
{
    [Header("Giro por peso")]
    [Tooltip("Qué tan fuerte acelera el giro la diferencia de peso. Subilo si te cuesta que empiece a girar, bajalo si gira demasiado rápido con poco peso encima.")]
    public float factorTorque = 15f;

    [Tooltip("Velocidad angular máxima permitida, en grados por segundo. Esto evita que gire descontroladamente rápido.")]
    public float velocidadAngularMaxima = 180f;

    [Header("Fricción (frenado realista)")]
    [Tooltip("Cuánta velocidad angular pierde por segundo cuando no hay nadie empujando (o incluso mientras empujan, si el torque no alcanza a compensarla). Es un frenado CONSTANTE, no exponencial, para que se sienta natural y realmente llegue a pararse. Más alto = frena más rápido.")]
    public float friccionAngular = 90f;

    // Velocidad angular actual, en grados por segundo. Positivo = sentido horario, negativo = antihorario (o viceversa según tus ejes).
    private float velocidadAngular = 0f;

    private Rigidbody2D rb;

    // Guardamos quién está parado encima y su componente de peso
    private Dictionary<Transform, PesoObjeto> objetosEncima = new Dictionary<Transform, PesoObjeto>();

    void Awake()
    {
        // Nos aseguramos de que el Rigidbody2D sea Kinematic:
        // no queremos que la física de Unity mueva la plataforma sola,
        // solo que detecte colisiones mientras la rotamos nosotros a mano.
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
    }

    void FixedUpdate()
    {
        float torqueTotal = CalcularTorqueTotal();

        // El torque de los personajes ACELERA la velocidad angular
        // (como aplicar fuerza a un sube y baja real).
        // Signo negativo: peso a la derecha empuja el giro hacia ese lado.
        velocidadAngular += -torqueTotal * factorTorque * Time.fixedDeltaTime;

        // Tope de velocidad máxima, para que no gire como una licuadora
        velocidadAngular = Mathf.Clamp(velocidadAngular, -velocidadAngularMaxima, velocidadAngularMaxima);

        // Fricción: resta velocidad de forma CONSTANTE hacia 0 (no asintótica).
        // Esto es lo que hace que frene de forma realista y en algún momento
        // se detenga del todo, en vez de temblar cerca de cero para siempre.
        velocidadAngular = Mathf.MoveTowards(velocidadAngular, 0f, friccionAngular * Time.fixedDeltaTime);

        // Integramos la velocidad angular para obtener el nuevo ángulo.
        // No hay límite de ángulo: puede girar 360° las veces que haga falta.
        float nuevoAngulo = rb.rotation + velocidadAngular * Time.fixedDeltaTime;
        rb.MoveRotation(nuevoAngulo);
    }

    private float CalcularTorqueTotal()
    {
        float torqueTotal = 0f;

        foreach (KeyValuePair<Transform, PesoObjeto> kvp in objetosEncima)
        {
            Transform objeto = kvp.Key;
            float peso = kvp.Value.peso;

            // Distancia horizontal del personaje al centro de la plataforma.
            float distanciaAlCentro = objeto.position.x - transform.position.x;

            // Cada personaje aporta peso x distancia (como un sube y baja real)
            torqueTotal += peso * distanciaAlCentro;
        }

        return torqueTotal;
    }

    // El Collider2D vive en este mismo objeto (PuntoGiroPlataforma),
    // así que Unity llama a estos eventos automáticamente.
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