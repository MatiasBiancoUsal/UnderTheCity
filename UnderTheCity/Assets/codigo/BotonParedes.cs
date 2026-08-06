using UnityEngine;

public class BotonParedes : MonoBehaviour
{
    public Paredes[] paredes;

    [Header("Texturas del botón")]
    public Sprite texturaNormal;
    public Sprite texturaPulsada;

    private SpriteRenderer spriteRenderer;
    private bool botonPulsado = false;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (texturaNormal != null)
        {
            spriteRenderer.sprite = texturaNormal;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Rata"))
            return;

        if (botonPulsado)
            return;

        botonPulsado = true;

        if (texturaPulsada != null)
        {
            spriteRenderer.sprite = texturaPulsada;
        }

        foreach (Paredes pared in paredes)
        {
            pared.ActivarMovimiento();
        }
    }
}
