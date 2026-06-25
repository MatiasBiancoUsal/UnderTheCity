using UnityEngine;

public class ItemRecolectable : MonoBehaviour
{
    public string tipo;
    public AudioClip sonidoRecolectar;

    private AudioSource audioSource;
    private Collider2D col;
    private SpriteRenderer sprite;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        col = GetComponent<Collider2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (tipo == "cerveza" && collision.CompareTag("Vagabundo"))
        {
            GameManager.instancia.cervezas++;
            Recolectar();
        }

        if (tipo == "queso" && collision.CompareTag("Rata"))
        {
            GameManager.instancia.quesos++;
            Recolectar();
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

    private void Recolectar()
    {
        if (col != null)
            col.enabled = false;

        if (sprite != null)
            sprite.enabled = false;

        if (audioSource != null && sonidoRecolectar != null)
        {
            audioSource.PlayOneShot(sonidoRecolectar);
            Destroy(gameObject, sonidoRecolectar.length);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}