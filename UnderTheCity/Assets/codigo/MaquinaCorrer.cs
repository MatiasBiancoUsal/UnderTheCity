using UnityEngine;
using UnityEngine.InputSystem;

public class MaquinaCorrer : MonoBehaviour
{
    [Header("ZONAS DE OSCURIDAD")]
    public GameObject[] zonasOscuridad;

    [Header("VELOCIDAD DE LA TRANSICION")]
    public float velocidadOscuridad = 0.5f;

    [Header("ANIMACION")]
    public float velocidadAnimacion = 1f;

    private GameObject jugadorDentro;

    private bool bajandoOscuridad = false;
    private bool subiendoOscuridad = false;

    private SpriteRenderer[] spritesOscuridad;
    private Animator animatorJugador;

    void Start()
    {
        ObtenerSpritesOscuridad();

        foreach (SpriteRenderer sprite in spritesOscuridad)
        {
            if (sprite != null)
            {
                Color color = sprite.color;
                color.a = 254f / 255f;
                sprite.color = color;
            }
        }
    }

    void Update()
    {
        if (jugadorDentro != null)
        {
            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                bajandoOscuridad = true;
                subiendoOscuridad = false;
            }
        }

        if (bajandoOscuridad)
        {
            BajarOscuridad();
        }

        if (subiendoOscuridad)
        {
            SubirOscuridad();
        }

        ActualizarAnimacion();
    }

    private void ActualizarAnimacion()
    {
        if (animatorJugador == null)
            return;

        if (jugadorDentro != null)
        {
            animatorJugador.SetFloat("velocidad", velocidadAnimacion);
        }
        else
        {
            animatorJugador.SetFloat("velocidad", 0f);
        }
    }

    private void ObtenerSpritesOscuridad()
    {
        int cantidadSprites = 0;

        foreach (GameObject zona in zonasOscuridad)
        {
            if (zona != null)
            {
                cantidadSprites += zona.GetComponentsInChildren<SpriteRenderer>().Length;
            }
        }

        spritesOscuridad = new SpriteRenderer[cantidadSprites];

        int indice = 0;

        foreach (GameObject zona in zonasOscuridad)
        {
            if (zona != null)
            {
                SpriteRenderer[] sprites = zona.GetComponentsInChildren<SpriteRenderer>();

                foreach (SpriteRenderer sprite in sprites)
                {
                    spritesOscuridad[indice] = sprite;
                    indice++;
                }
            }
        }
    }

    private void BajarOscuridad()
    {
        bool termino = true;

        foreach (SpriteRenderer sprite in spritesOscuridad)
        {
            if (sprite == null)
                continue;

            Color color = sprite.color;

            color.a = Mathf.MoveTowards(
                color.a,
                0f,
                velocidadOscuridad * Time.deltaTime
            );

            sprite.color = color;

            if (color.a > 0f)
            {
                termino = false;
            }
        }

        if (termino)
        {
            bajandoOscuridad = false;
        }
    }

    private void SubirOscuridad()
    {
        bool termino = true;

        foreach (SpriteRenderer sprite in spritesOscuridad)
        {
            if (sprite == null)
                continue;

            Color color = sprite.color;

            color.a = Mathf.MoveTowards(
                color.a,
                254f / 255f,
                velocidadOscuridad * Time.deltaTime
            );

            sprite.color = color;

            if (color.a < 254f / 255f)
            {
                termino = false;
            }
        }

        if (termino)
        {
            subiendoOscuridad = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Vagabundo") ||
            collision.CompareTag("Rata"))
        {
            jugadorDentro = collision.gameObject;

            animatorJugador = jugadorDentro.GetComponentInChildren<Animator>();

            if (animatorJugador != null)
            {
                animatorJugador.SetFloat("velocidad", velocidadAnimacion);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == jugadorDentro)
        {
            if (animatorJugador != null)
            {
                animatorJugador.SetFloat("velocidad", 0f);
            }

            jugadorDentro = null;
            animatorJugador = null;

            bajandoOscuridad = false;
            subiendoOscuridad = true;
        }
    }
}