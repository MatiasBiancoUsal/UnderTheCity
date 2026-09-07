using System.Collections.Generic;
using UnityEngine;

public class CajaSoloVagabundo : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool empujando = false;

    private Vector2 posicionAnterior;
    private List<Rigidbody2D> jugadoresEncima = new List<Rigidbody2D>();

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        posicionAnterior = rb.position;
    }

    void Update()
    {
        if (!empujando)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        }

        empujando = false;
    }

    private void FixedUpdate()
    {
        Vector2 desplazamiento = rb.position - posicionAnterior;

        if (desplazamiento != Vector2.zero)
        {
            foreach (Rigidbody2D jugador in jugadoresEncima)
            {
                if (jugador != null)
                {
                    jugador.position += desplazamiento;
                }
            }
        }

        posicionAnterior = rb.position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Vagabundo"))
        {
            empujando = true;
        }

        AgregarSiEstaEncima(collision);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Vagabundo"))
        {
            empujando = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        Rigidbody2D rbJugador = collision.gameObject.GetComponentInParent<Rigidbody2D>();

        if (rbJugador != null)
        {
            jugadoresEncima.Remove(rbJugador);
        }
    }

    private void AgregarSiEstaEncima(Collision2D collision)
    {
        foreach (ContactPoint2D contacto in collision.contacts)
        {
            if (contacto.normal.y < -0.3f)
            {
                Rigidbody2D rbJugador = collision.gameObject.GetComponentInParent<Rigidbody2D>();

                if (rbJugador != null && !jugadoresEncima.Contains(rbJugador))
                {
                    jugadoresEncima.Add(rbJugador);
                }

                return;
            }
        }
    }
}