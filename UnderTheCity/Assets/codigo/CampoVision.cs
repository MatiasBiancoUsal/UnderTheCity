using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CampoVision : MonoBehaviour
{
    [Header("TIEMPO PARA DETECTAR")]
    public float tiempoDeteccion = 2f;

    [Header("OBSTACULOS")]
    public LayerMask capaObstaculos;

    [Header("AUDIO")]
    public AudioSource audioSource;

    [Header("BARRA DE DETECCION")]
    public GameObject barraDeteccion;
    public UnityEngine.UI.Image barra;

    private float tiempoMirando = 0f;

    private HashSet<GameObject> jugadoresDentro = new HashSet<GameObject>();

    private bool jugadorDetectado = false;

    void Start()
    {
        if (barraDeteccion != null)
            barraDeteccion.SetActive(false);

        if (barra != null)
            barra.fillAmount = 0f;

        if (audioSource != null)
            audioSource.playOnAwake = false;
    }

    void Update()
    {
        DetectarJugadores();
    }

    void DetectarJugadores()
    {
        Enemigo enemigo = transform.parent.GetComponent<Enemigo>();

        if (jugadoresDentro.Count == 0)
        {
            tiempoMirando = 0f;
            jugadorDetectado = false;

            if (barra != null)
                barra.fillAmount = 0f;

            if (barraDeteccion != null)
                barraDeteccion.SetActive(false);

            if (enemigo != null)
                enemigo.Reanudar();

            return;
        }

        tiempoMirando += Time.deltaTime;

        if (enemigo != null)
            enemigo.Parar();

        if (barraDeteccion != null)
            barraDeteccion.SetActive(true);

        if (barra != null)
            barra.fillAmount = tiempoMirando / tiempoDeteccion;

        if (tiempoMirando >= tiempoDeteccion && !jugadorDetectado)
        {
            jugadorDetectado = true;

            ReiniciarNivel();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject jugador = ObtenerJugador(collision);

        if (jugador == null)
            return;

        if (HayObstaculo(jugador.transform))
            return;

        bool yaEstabaDentro = jugadoresDentro.Contains(jugador);

        jugadoresDentro.Add(jugador);

        if (!yaEstabaDentro)
        {
            tiempoMirando = 0f;

            if (barraDeteccion != null)
                barraDeteccion.SetActive(true);

            if (barra != null)
                barra.fillAmount = 0f;

            if (audioSource != null)
                audioSource.Play();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        GameObject jugador = ObtenerJugador(collision);

        if (jugador == null)
            return;

        jugadoresDentro.Remove(jugador);

        if (jugadoresDentro.Count == 0)
        {
            tiempoMirando = 0f;
            jugadorDetectado = false;

            if (barra != null)
                barra.fillAmount = 0f;

            if (barraDeteccion != null)
                barraDeteccion.SetActive(false);

            Enemigo enemigo = transform.parent.GetComponent<Enemigo>();

            if (enemigo != null)
                enemigo.Reanudar();
        }
    }

    GameObject ObtenerJugador(Collider2D collision)
    {
        if (collision.CompareTag("Vagabundo"))
            return collision.gameObject;

        if (collision.CompareTag("Rata"))
            return collision.gameObject;

        Transform padre = collision.transform.parent;

        if (padre != null)
        {
            if (padre.CompareTag("Vagabundo") ||
                padre.CompareTag("Rata"))
            {
                return padre.gameObject;
            }
        }

        return null;
    }

    bool HayObstaculo(Transform jugador)
    {
        if (capaObstaculos.value == 0)
            return false;

        Transform enemigo = transform.parent;

        if (enemigo == null)
            return false;

        Vector2 origen = enemigo.position;

        Vector2 direccion = (Vector2)jugador.position - origen;

        float distancia = direccion.magnitude;

        RaycastHit2D golpe = Physics2D.Raycast(
            origen,
            direccion.normalized,
            distancia,
            capaObstaculos
        );

        return golpe.collider != null;
    }

    void ReiniciarNivel()
    {
        SceneManager.LoadScene(
            SceneManager.GetActiveScene().name
        );
    }
}