using UnityEngine;
using UnityEngine.InputSystem;

public class Palanca : MonoBehaviour
{
    public Trampilla trampilla;

    public Sprite spriteApagado; 
    public Sprite spriteEncendido; 

    private SpriteRenderer sr;
    private bool jugadorCerca = false;
    private bool activada = false;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (jugadorCerca && Keyboard.current.eKey.wasPressedThisFrame)
        {
            trampilla.Activar();

            activada = !activada;

            if (activada)
            {
                sr.sprite = spriteEncendido;
            }
            else
            {
                sr.sprite = spriteApagado;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Vagabundo"))
        {
            jugadorCerca = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Vagabundo"))
        {
            jugadorCerca = false;
        }
    }
}
