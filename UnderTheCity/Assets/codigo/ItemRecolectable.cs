using UnityEngine;

public class ItemRecolectable : MonoBehaviour
{
    public string tipo;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (tipo == "cerveza" && collision.CompareTag("Vagabundo"))
        {
            GameManager.instancia.cervezas++;
            Destroy(gameObject);
        }

        if (tipo == "queso" && collision.CompareTag("Rata"))
        {
            GameManager.instancia.quesos++;
            Destroy(gameObject);
        }

        if (tipo == "velocidad" && collision.CompareTag("Vagabundo"))
        {
            VagabundoControl jugador = collision.GetComponent<VagabundoControl>();

            if (jugador != null)
            {
                jugador.BeberLata();
                Destroy(gameObject);
            }
        }
    }
}