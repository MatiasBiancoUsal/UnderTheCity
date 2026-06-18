using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instancia;

    public bool vagabundoListo = false;
    public bool rataLista = false;

    public string siguienteNivel;

    bool yaCargo = false;

    public int cervezas = 0;
    public int quesos = 0;

    public int cervezasNecesarias = 3;
    public int quesosNecesarios = 3;

    // TEMPORIZADOR
    public float tiempoTranscurrido = 0f;

    // Texto del temporizador
    public TMP_Text textoTiempo;

    void Awake()
    {
        instancia = this;
    }

    void Update()
    {
        // Cronómetro
        tiempoTranscurrido += Time.deltaTime;

        // Actualizar texto en pantalla
        if (textoTiempo != null)
        {
            textoTiempo.text = "Tiempo: " + tiempoTranscurrido.ToString("F2") + " s";
        }

        // Reiniciar nivel con R
        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            int indiceActual = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(indiceActual);
        }

        bool vagabundoCompleto = cervezas >= cervezasNecesarias;
        bool rataCompleta = quesos >= quesosNecesarios;

        if (!yaCargo && vagabundoListo && rataLista && vagabundoCompleto && rataCompleta)
        {
            yaCargo = true;

            Debug.Log("Tiempo completado: " + tiempoTranscurrido.ToString("F2") + " segundos");

            SceneManager.LoadScene(siguienteNivel);
        }
    }
}