using UnityEngine;
using UnityEngine.InputSystem;

public class MaquinaCorrer : MonoBehaviour
{
    [Header("ZONAS DE OSCURIDAD")]
    public GameObject[] zonasOscuridad;

    [Header("VELOCIDAD DE LA TRANSICION")]
    public float velocidadOscuridad = 0.5f;

    [Header("ANIMACION VAGABUNDO")]
    public Animator animatorVagabundo;

    [Header("ANIMACION RATA")]
    public Animator animatorRata;

    [Header("VELOCIDAD DE ANIMACION")]
    public float velocidadAnimacion = 1f;

    private GameObject jugadorDentro;

    private bool maquinaActiva = false;

    private bool esVagabundo = false;
    private bool esRata = false;

    private bool bajandoOscuridad = false;
    private bool subiendoOscuridad = false;

    private SpriteRenderer[] spritesOscuridad;

    private RataControl rataControl;

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
        if (jugadorDentro != null && !maquinaActiva)
        {
            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                ActivarMaquina();
            }
        }

        if (maquinaActiva)
        {
            MantenerAnimacion();
        }

        if (bajandoOscuridad)
        {
            BajarOscuridad();
        }

        if (subiendoOscuridad)
        {
            SubirOscuridad();
        }
    }

    private void ActivarMaquina()
    {
        maquinaActiva = true;

        bajandoOscuridad = true;
        subiendoOscuridad = false;

        if (esRata && rataControl != null)
        {
            rataControl.BloquearParaMaquina();
        }

        if (esVagabundo)
        {
            if (animatorVagabundo != null)
            {
                animatorVagabundo.SetFloat(
                    "velocidad",
                    velocidadAnimacion
                );
            }
        }

        if (esRata)
        {
            if (animatorRata != null)
            {
                animatorRata.SetFloat(
                    "velocidad",
                    velocidadAnimacion
                );
            }
        }
    }

    private void MantenerAnimacion()
    {
        if (esVagabundo)
        {
            if (animatorVagabundo != null)
            {
                animatorVagabundo.SetFloat(
                    "velocidad",
                    velocidadAnimacion
                );
            }
        }

        if (esRata)
        {
            if (animatorRata != null)
            {
                animatorRata.SetFloat(
                    "velocidad",
                    velocidadAnimacion
                );
            }
        }
    }

    private void ObtenerSpritesOscuridad()
    {
        int cantidadSprites = 0;

        foreach (GameObject zona in zonasOscuridad)
        {
            if (zona != null)
            {
                cantidadSprites +=
                    zona.GetComponentsInChildren<SpriteRenderer>().Length;
            }
        }

        spritesOscuridad = new SpriteRenderer[cantidadSprites];

        int indice = 0;

        foreach (GameObject zona in zonasOscuridad)
        {
            if (zona != null)
            {
                SpriteRenderer[] sprites =
                    zona.GetComponentsInChildren<SpriteRenderer>();

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
        if (collision.CompareTag("Vagabundo"))
        {
            jugadorDentro = collision.gameObject;

            esVagabundo = true;
            esRata = false;

            maquinaActiva = false;

            if (animatorVagabundo != null)
            {
                animatorVagabundo.SetFloat("velocidad", 0f);
            }
        }

        if (collision.CompareTag("Rata"))
        {
            jugadorDentro = collision.gameObject;

            esRata = true;
            esVagabundo = false;

            maquinaActiva = false;

            rataControl = collision.GetComponent<RataControl>();

            if (animatorRata != null)
            {
                animatorRata.SetFloat("velocidad", 0f);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject != jugadorDentro)
            return;

        if (esVagabundo)
        {
            if (animatorVagabundo != null)
            {
                animatorVagabundo.SetFloat("velocidad", 0f);
            }
        }

        if (esRata)
        {
            if (animatorRata != null)
            {
                animatorRata.SetFloat("velocidad", 0f);
            }

            if (rataControl != null)
            {
                rataControl.DesbloquearParaMaquina();
            }
        }

        jugadorDentro = null;

        rataControl = null;

        esVagabundo = false;
        esRata = false;

        maquinaActiva = false;

        bajandoOscuridad = false;
        subiendoOscuridad = true;
    }
}