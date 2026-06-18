using UnityEngine;
using System.Collections;

public class Paredes : MonoBehaviour
{
    public float tiempoDesaparicion = 1f;

    private SpriteRenderer spriteRenderer;
    private Collider2D col;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
    }

    public void Desaparecer()
    {
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        if (col != null)
        {
            col.enabled = false;
        }

        Color colorInicial = spriteRenderer.color;

        float tiempo = 0f;

        while (tiempo < tiempoDesaparicion)
        {
            tiempo += Time.deltaTime;

            Color nuevoColor = colorInicial;
            nuevoColor.a = Mathf.Lerp(1f, 0f, tiempo / tiempoDesaparicion);

            spriteRenderer.color = nuevoColor;

            yield return null;
        }

        gameObject.SetActive(false);
    }
}
