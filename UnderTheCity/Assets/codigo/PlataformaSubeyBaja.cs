using System.Collections.Generic;
using UnityEngine;

public class PlataformaSubeyBaja : MonoBehaviour
{
    public Transform posicionArriba;
    public Transform posicionAbajo;
    public float velocidad = 2f;

    public bool activada = false;

    private Vector3 posicionAnterior;
    private List<Transform> jugadoresEncima = new List<Transform>();

    private void Start()
    {
        posicionAnterior = transform.position;
    }

    void Update()
    {
        if (activada)
        {
            transform.position = Vector2.MoveTowards(
                transform.position,
                posicionArriba.position,
                velocidad * Time.deltaTime
            );
        }
        else
        {
            transform.position = Vector2.MoveTowards(
                transform.position,
                posicionAbajo.position,
                velocidad * Time.deltaTime
            );
        }

        Vector3 desplazamiento = transform.position - posicionAnterior;

        if (desplazamiento != Vector3.zero)
        {
            foreach (Transform jugador in jugadoresEncima)
            {
                if (jugador != null)
                {
                    jugador.position += desplazamiento;
                }
            }
        }

        posicionAnterior = transform.position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!EsJugador(collision.gameObject))
            return;

        foreach (ContactPoint2D contacto in collision.contacts)
        {
            if (contacto.normal.y < -0.5f)
            {
                if (!jugadoresEncima.Contains(collision.transform))
                {
                    jugadoresEncima.Add(collision.transform);
                }

                return;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (!EsJugador(collision.gameObject))
            return;

        jugadoresEncima.Remove(collision.transform);
    }

    private bool EsJugador(GameObject objeto)
    {
        return objeto.CompareTag("Vagabundo") || objeto.CompareTag("Rata");
    }
}
