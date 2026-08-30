using UnityEngine;

public class ZonaFinal : MonoBehaviour
{
    [Header("JUGADOR")]
    public string tipoJugador;

    [Header("TIEMPO ANTES DE CAMBIAR DE NIVEL")]
    public float tiempoAntesDeCambiar = 1.5f;

    private Animator animatorPuerta;
    private bool jugadorDentro = false;
    private bool animacionDisparada = false;

    private void Start()
    {
        animatorPuerta = GetComponentInChildren<Animator>();

        if (animatorPuerta == null)
        {
            UnityEngine.Debug.LogWarning(
                "[" + gameObject.name + "] No se encontró ningún Animator " +
                "en este objeto ni en sus hijos."
            );
        }
        else
        {
            animatorPuerta.speed = 0f;
        }
    }

    private void Update()
    {
        if (GameManager.instancia == null)
            return;

        if (!animacionDisparada && TieneTodosLosObjetos())
        {
            animacionDisparada = true;

            if (animatorPuerta != null)
            {
                animatorPuerta.speed = 1f; 
                UnityEngine.Debug.Log("PUERTA ABRIENDO: " + tipoJugador);
            }
        }
    }

    private bool TieneTodosLosObjetos()
    {
        if (tipoJugador == "Vagabundo")
        {
            return GameManager.instancia.cervezas >=
                   GameManager.instancia.cervezasNecesarias;
        }
        else if (tipoJugador == "Rata")
        {
            return GameManager.instancia.quesos >=
                   GameManager.instancia.quesosNecesarios;
        }

        return false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(tipoJugador))
            return;

        jugadorDentro = true;

        if (tipoJugador == "Vagabundo")
        {
            GameManager.instancia.vagabundoListo = true;
        }
        else if (tipoJugador == "Rata")
        {
            GameManager.instancia.rataLista = true;
        }

        IntentarCambiarDeNivel();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag(tipoJugador))
            return;

        jugadorDentro = false;

        if (tipoJugador == "Vagabundo")
        {
            GameManager.instancia.vagabundoListo = false;
        }
        else if (tipoJugador == "Rata")
        {
            GameManager.instancia.rataLista = false;
        }
    }

    private void IntentarCambiarDeNivel()
    {
        if (GameManager.instancia != null && jugadorDentro)
        {
            Invoke(nameof(LlamarCambioDeNivel), tiempoAntesDeCambiar);
        }
    }

    private void LlamarCambioDeNivel()
    {
        if (GameManager.instancia != null)
        {
            GameManager.instancia.IntentarCambiarDeNivel();
        }
    }
}