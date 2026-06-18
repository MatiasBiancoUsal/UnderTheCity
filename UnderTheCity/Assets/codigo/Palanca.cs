using UnityEngine;
using UnityEngine.InputSystem;

public class Palanca : MonoBehaviour
{
    public Trampilla trampilla;

    private bool jugadorCerca = false;

    private void Update()
    {
        if (jugadorCerca && Keyboard.current.eKey.wasPressedThisFrame)
        {
            trampilla.Activar();
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
