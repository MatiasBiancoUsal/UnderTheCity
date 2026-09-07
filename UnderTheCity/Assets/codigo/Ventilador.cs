using System.Collections.Generic;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class Ventilador : MonoBehaviour
{
    [Header("FUERZA DEL VIENTO")]
    public float fuerza = 15f;

    [Header("VELOCIDAD MAXIMA HACIA ARRIBA")]
    public float velocidadMaxima = 6f;

    private List<Rigidbody2D> jugadoresDentro = new List<Rigidbody2D>();

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!EsJugador(other))
            return;

        Rigidbody2D rb = other.GetComponentInParent<Rigidbody2D>();

        if (rb == null)
        {
            Debug.LogWarning(
                "[" + gameObject.name + "] " + other.name +
                " tiene el tag de jugador pero no se encontró Rigidbody2D " +
                "ni en el mismo objeto ni en sus padres."
            );
            return;
        }

        if (!jugadoresDentro.Contains(rb))
        {
            jugadoresDentro.Add(rb);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!EsJugador(other))
            return;

        Rigidbody2D rb = other.GetComponentInParent<Rigidbody2D>();

        if (rb != null)
        {
            jugadoresDentro.Remove(rb);
        }
    }

    private bool EsJugador(Collider2D otro)
    {
        return otro.CompareTag("Rata") || otro.CompareTag("Vagabundo");
    }

    private void FixedUpdate()
    {
        foreach (Rigidbody2D rb in jugadoresDentro)
        {
            if (rb == null)
                continue;

            rb.AddForce(Vector2.up * fuerza, ForceMode2D.Force);

            if (rb.linearVelocity.y > velocidadMaxima)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, velocidadMaxima);
            }
        }
    }
}