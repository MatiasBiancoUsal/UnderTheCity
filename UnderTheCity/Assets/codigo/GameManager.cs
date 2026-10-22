using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using TMPro;
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

    [Header("cantidadEstrellas")]
    public int cantidadEstrellas;

    [Header("reinicio")]
    public int reinicio;


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
            textoTiempo.text = $" {tiempoTranscurrido:F2} S";
        }

        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

            int reinicio = PlayerPrefs.GetInt("R", 0);
            PlayerPrefs.SetInt("R", reinicio + 1);

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
        string nivActualName = SceneManager.GetActiveScene().name;

        PlayerPrefs.SetInt(
            "NivelDesbloqueado",
            Mathf.Max(PlayerPrefs.GetInt("NivelDesbloqueado", 1), nivelActual + 1)
        );
        PlayerPrefs.Save();

        if (PantallaVictoria.instancia != null)
        {
            PantallaVictoria.instancia.Mostrar(tiempoTranscurrido, siguienteNivel);
            AnalyticsManager.Instance.RegistrarNivelCompletado(nivActualName,
                tiempoTranscurrido,
                PlayerPrefs.GetInt("MV", 0)+ PlayerPrefs.GetInt("MR", 0),
                PlayerPrefs.GetInt("MV", 0),
                PlayerPrefs.GetInt("MR", 0),
                cantidadEstrellas,
                PlayerPrefs.GetInt("R", 0));
            PlayerPrefs.SetInt("MR", 0);
            PlayerPrefs.SetInt("MV", 0);
            PlayerPrefs.SetInt("R", 0);


        }
        else
        {
            Debug.LogWarning("No se encontró PantallaVictoria en la escena, cargando nivel directo.");
            SceneManager.LoadScene(siguienteNivel);
        }

    }
}