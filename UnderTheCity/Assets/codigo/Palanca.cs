using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Debug = UnityEngine.Debug;

public class Palanca : MonoBehaviour
{
    public Trampilla trampilla;

    [Header("CLIP DE LA PALANCA (arrastrar aca y listo)")]
    public AnimationClip animacionPalanca;

    private Animator animator;
    private bool jugadorCerca = false;
    private bool activada = false;
    private Coroutine reproduccionActual;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();

        if (animator == null)
        {
            Debug.LogWarning(
                "[" + gameObject.name + "] No se encontró ningún Animator " +
                "en este objeto ni en sus hijos."
            );
        }
        else
        {
            animator.speed = 0f;
        }
    }

    private void Update()
    {
        if (jugadorCerca && Keyboard.current.eKey.wasPressedThisFrame)
        {
            trampilla.Activar();

            activada = !activada;

            ReproducirAnimacionPalanca();
        }
    }

    private void ReproducirAnimacionPalanca()
    {
        if (animator == null || animacionPalanca == null)
            return;

        if (reproduccionActual != null)
        {
            StopCoroutine(reproduccionActual);
        }

        float desde = activada ? 0f : 1f;
        float hasta = activada ? 1f : 0f;

        reproduccionActual = StartCoroutine(ReproducirManual(desde, hasta));
    }

    private IEnumerator ReproducirManual(float desde, float hasta)
    {
        float duracion = animacionPalanca.length;
        float tiempo = 0f;

        while (tiempo < duracion)
        {
            tiempo += Time.deltaTime;

            float t = Mathf.Clamp01(tiempo / duracion);
            float normalizedTime = Mathf.Lerp(desde, hasta, t);

            animator.Play(animacionPalanca.name, 0, normalizedTime);
            animator.Update(0f);

            yield return null;
        }

        animator.Play(animacionPalanca.name, 0, hasta);
        animator.Update(0f);

        reproduccionActual = null;
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