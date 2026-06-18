using UnityEngine;

public class Boton : MonoBehaviour
{
    public PlataformaSubeyBaja plataforma;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        plataforma.activada = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        plataforma.activada = false;
    }
}