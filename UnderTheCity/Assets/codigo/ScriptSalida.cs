using UnityEngine;

public class ZonaFinal : MonoBehaviour
{
    public string tipoJugador;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(tipoJugador))
        {
            if (tipoJugador == "Vagabundo")
                GameManager.instancia.vagabundoListo = true;

            if (tipoJugador == "Rata")
                GameManager.instancia.rataLista = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(tipoJugador))
        {
            if (tipoJugador == "Vagabundo")
                GameManager.instancia.vagabundoListo = false;

            if (tipoJugador == "Rata")
                GameManager.instancia.rataLista = false;
        }
    }
}
