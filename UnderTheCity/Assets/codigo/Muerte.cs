using UnityEngine;
using Debug = UnityEngine.Debug;

public class ZonaMuerte : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Vagabundo") || collision.CompareTag("Rata"))
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