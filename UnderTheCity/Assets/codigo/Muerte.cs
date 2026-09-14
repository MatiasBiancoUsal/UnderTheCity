using UnityEngine;
using Debug = UnityEngine.Debug;

public class ZonaMuerte : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Vagabundo"))
        {
            MatarVagabundo(collision.gameObject);
        }
        else if (collision.CompareTag("Rata"))
        {
            MatarRata(collision.gameObject);
        }
    }

    private void MatarVagabundo(GameObject jugador)
    {
        VagabundoControl vagabundo = jugador.GetComponent<VagabundoControl>();

        if (vagabundo != null)
        {
            vagabundo.Morir();
        }
        else
        {
            MostrarPantallaDerrota();
        }
    }

    private void MatarRata(GameObject jugador)
    {
        RataControl rata = jugador.GetComponent<RataControl>();

        if (rata != null)
        {
            rata.Morir();
        }
        else
        {
            MostrarPantallaDerrota();
        }
    }

    private void MostrarPantallaDerrota()
    {
        if (PantallaDerrota.instancia != null)
        {
            PantallaDerrota.instancia.Mostrar();
        }
        else
        {
            Debug.LogWarning("No se encontró PantallaDerrota en la escena.");
        }
    }
}