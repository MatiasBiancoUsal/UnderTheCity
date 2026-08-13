using UnityEngine;

public class Boton : MonoBehaviour
{
    public PlataformaSubeyBaja plataforma;

    [Header("Texturas del botón")]
    public Sprite texturaNormal;
    public Sprite texturaPulsada;

    private SpriteRenderer spriteRenderer;
    private bool boton = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        plataforma.activada = true;
        GetComponent<SpriteRenderer>().sprite = texturaNormal;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        plataforma.activada = false;
        GetComponent<SpriteRenderer>().sprite = texturaPulsada;

    }
}