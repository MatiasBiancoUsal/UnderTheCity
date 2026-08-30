using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Debug = UnityEngine.Debug;

public class GameManager : MonoBehaviour
{
    public static GameManager instancia;

    [Header("ESTADO DE LOS JUGADORES")]
    public bool vagabundoListo = false;
    public bool rataLista = false;

    [Header("SIGUIENTE NIVEL")]
    public string siguienteNivel;

    [Header("OBJETOS VAGABUNDO")]
    public int cervezas = 0;
    public int cervezasNecesarias = 3;

    [Header("OBJETOS RATA")]
    public int quesos = 0;
    public int quesosNecesarios = 3;

    [Header("TIEMPO")]
    public float tiempoTranscurrido = 0f;
    public TMP_Text textoTiempo;

    private bool yaCargo = false;

    private void Awake()
    {
        instancia = this;
    }

    private void Update()
    {
        tiempoTranscurrido += Time.deltaTime;

        if (textoTiempo != null)
        {
            textoTiempo.text = $"Tiempo: {tiempoTranscurrido:F2} s";
        }

        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void IntentarCambiarDeNivel()
    {
        if (yaCargo)
            return;

        bool vagabundoCompleto = cervezas >= cervezasNecesarias;
        bool rataCompleta = quesos >= quesosNecesarios;

        Debug.Log(
            $"COMPROBANDO SALIDA -> Vagabundo listo: {vagabundoListo} | " +
            $"Rata lista: {rataLista} | Cervezas: {cervezas}/{cervezasNecesarias} | " +
            $"Quesos: {quesos}/{quesosNecesarios}"
        );

        if (!(vagabundoListo && rataLista && vagabundoCompleto && rataCompleta))
        {
            Debug.Log("NO SE PUEDE CAMBIAR DE NIVEL TODAVIA.");
            return;
        }

        yaCargo = true;

        Debug.Log($"NIVEL COMPLETADO -> Tiempo: {tiempoTranscurrido:F2} segundos");

        if (string.IsNullOrEmpty(siguienteNivel))
        {
            Debug.LogError("No asignaste el campo 'Siguiente Nivel' en el GameManager.");
            yaCargo = false;
            return;
        }

        int nivelActual = SceneManager.GetActiveScene().buildIndex;

        PlayerPrefs.SetInt(
            "NivelDesbloqueado",
            Mathf.Max(PlayerPrefs.GetInt("NivelDesbloqueado", 1), nivelActual + 1)
        );
        PlayerPrefs.Save();

        SceneManager.LoadScene(siguienteNivel);
    }
}