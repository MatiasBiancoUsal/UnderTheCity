using UnityEngine;
using UnityEngine.InputSystem;

public class Escondite : MonoBehaviour
{
    [Header("TEXTURAS DEL ESCONDITE")]
    public Sprite texturaVacia;
    public Sprite texturaOcupada;

    private SpriteRenderer spriteRenderer;

    private GameObject jugadorDentro;
    private bool escondido = false;

    private SpriteRenderer[] spritesJugador;
    private Collider2D[] collidersJugador;

    private Rigidbody2D rbJugador;
    private RigidbodyConstraints2D restriccionesOriginales;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (texturaVacia != null)
        {
            spriteRenderer.sprite = texturaVacia;
        }
    }

    void Update()
    {
        if (jugadorDentro == null)
            return;

        if (jugadorDentro.CompareTag("Vagabundo"))
        {
            if (Keyboard.current.wKey.wasPressedThisFrame)
            {
                AlternarEscondite();
            }
        }
        else if (jugadorDentro.CompareTag("Rata"))
        {
            if (Keyboard.current.upArrowKey.wasPressedThisFrame)
            {
                AlternarEscondite();
            }
        }
    }

    private void AlternarEscondite()
    {
        escondido = !escondido;

        if (escondido)
        {
            EsconderJugador();
        }
        else
        {
            MostrarJugador();
        }
    }

    private void EsconderJugador()
    {
        spritesJugador = jugadorDentro.GetComponentsInChildren<SpriteRenderer>();
        collidersJugador = jugadorDentro.GetComponentsInChildren<Collider2D>();

        rbJugador = jugadorDentro.GetComponent<Rigidbody2D>();

        if (rbJugador != null)
        {
            restriccionesOriginales = rbJugador.constraints;

            rbJugador.linearVelocity = Vector2.zero;
            rbJugador.angularVelocity = 0f;

            rbJugador.constraints = RigidbodyConstraints2D.FreezeAll;
        }

        foreach (SpriteRenderer sprite in spritesJugador)
        {
            sprite.enabled = false;
        }

        foreach (Collider2D col in collidersJugador)
        {
            col.enabled = false;
        }
    }

    private void MostrarJugador()
    {
        if (spritesJugador != null)
        {
            foreach (SpriteRenderer sprite in spritesJugador)
            {
                if (sprite != null)
                {
                    sprite.enabled = true;
                }
            }
        }

        if (collidersJugador != null)
        {
            foreach (Collider2D col in collidersJugador)
            {
                if (col != null)
                {
                    col.enabled = true;
                }
            }
        }

        if (rbJugador != null)
        {
            rbJugador.constraints = restriccionesOriginales;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Vagabundo") ||
            collision.CompareTag("Rata"))
        {
            if (!escondido)
            {
                jugadorDentro = collision.gameObject;

                if (spriteRenderer != null && texturaOcupada != null)
                {
                    spriteRenderer.sprite = texturaOcupada;
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == jugadorDentro && !escondido)
        {
            jugadorDentro = null;

            if (spriteRenderer != null && texturaVacia != null)
            {
                spriteRenderer.sprite = texturaVacia;
            }
        }
    }
}