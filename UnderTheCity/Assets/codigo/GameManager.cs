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

    public float tiempoTranscurrido = 0f;

    public TMP_Text textoTiempo;

    void Awake()
    {
        instancia = this;
    }

    void Update()
    {
        tiempoTranscurrido += Time.deltaTime;

        if (textoTiempo != null)
        {
            textoTiempo.text = "Tiempo: " + tiempoTranscurrido.ToString("F2") + " s";
        }

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

            // ✅ Guardar progreso antes de cambiar de escena
            int nivelActual = SceneManager.GetActiveScene().buildIndex;
            PlayerPrefs.SetInt("NivelDesbloqueado", Mathf.Max(PlayerPrefs.GetInt("NivelDesbloqueado", 1), nivelActual + 1));
            PlayerPrefs.Save();

            SceneManager.LoadScene(siguienteNivel);
        }
    }
}