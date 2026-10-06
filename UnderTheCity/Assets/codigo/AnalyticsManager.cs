using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UnityConsent;
using Unity.Services.Core;
using Unity.Services.Analytics;

public class AnalyticsManager : MonoBehaviour
{
    public static AnalyticsManager Instance { get; private set; }

    // true cuando Unity Services terminó de inicializar
    public bool Listo { get; private set; }

    // 0 = todavía no eligió, 1 = aceptó, 2 = rechazó
    const string ClaveConsentimiento = "analytics_consent";

    [Header("Arrastrá acá el objeto que indica que se pasó el nivel (ej: Canvas Victoria)")]
    [SerializeField] GameObject objetoVictoria;
    [SerializeField] int puntaje = 0;

    float tiempoInicioNivel;
    int muertesNivel;
    bool victoriaEnviada = false;

    void Start()
    {
        IniciarNivel();
    }

    void Update()
    {
        // Si el objeto fue destruido (cambio de escena) o no se asignó, no hace nada
        if (victoriaEnviada || objetoVictoria == null) return;

        // Cuando el objeto se activa, se considera que el jugador pasó el nivel
        if (objetoVictoria.activeInHierarchy)
        {
            victoriaEnviada = true;
            Victoria(puntaje);
        }
    }

    // ---------------------------------------------------------------
    // INICIALIZACIÓN
    // ---------------------------------------------------------------
    async void Awake()
    {
        // Singleton: solo una copia, que sobrevive al cambio de escena
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        try
        {
            await UnityServices.InitializeAsync();
            Listo = true;

            // Si el jugador ya eligió antes, aplicamos su decisión automáticamente
            int guardado = PlayerPrefs.GetInt(ClaveConsentimiento, 0);
            if (guardado != 0)
            {
                AplicarConsentimiento(guardado == 1);
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error al iniciar Unity Services: " + e.Message);
        }
    }

    // ---------------------------------------------------------------
    // CONSENTIMIENTO
    // ---------------------------------------------------------------

    // Devuelve true si el jugador ya eligió (para mostrar el panel solo la primera vez)
    public bool YaEligioConsentimiento()
    {
        return PlayerPrefs.GetInt(ClaveConsentimiento, 0) != 0;
    }

    // Llamala desde los botones Aceptar (true) / Rechazar (false)
    public void RecoleccionDatos(bool consentimiento)
    {
        PlayerPrefs.SetInt(ClaveConsentimiento, consentimiento ? 1 : 2);
        PlayerPrefs.Save();
        AplicarConsentimiento(consentimiento);
    }

    void AplicarConsentimiento(bool consentimiento)
    {
        ConsentState estado = new ConsentState
        {
            AnalyticsIntent = consentimiento ? ConsentStatus.Granted : ConsentStatus.Denied,
            AdsIntent = ConsentStatus.Denied
        };
        EndUserConsent.SetConsentState(estado);
    }

    // ---------------------------------------------------------------
    // EVENTOS
    // ---------------------------------------------------------------

    // Llamar al empezar cada nivel
    public void IniciarNivel()
    {
        tiempoInicioNivel = Time.time;
        muertesNivel = 0;

        Enviar(new CustomEvent("level_started")
        {
            { "level_name", SceneManager.GetActiveScene().name }
        });
    }

    // Llamar cada vez que el jugador muere
    public void RegistrarMuerte(string causa)
    {
        muertesNivel++;

        Enviar(new CustomEvent("player_died")
        {
            { "level_name", SceneManager.GetActiveScene().name },
            { "cause", causa },
            { "time_seconds", Time.time - tiempoInicioNivel },
            { "deaths_so_far", muertesNivel }
        });
    }

    // Llamar cuando el jugador gana (junto con mostrar el canvas de victoria)
    public void Victoria(int puntaje = 0)
    {
        Enviar(new CustomEvent("NivelCompletado")
        {
            { "level_name", SceneManager.GetActiveScene().name },
            { "time_seconds", Time.time - tiempoInicioNivel },
            { "deaths", muertesNivel },
            { "score", puntaje }
        });
    }

    // Llamar cuando el jugador pierde definitivamente
    public void Derrota()
    {
        Enviar(new CustomEvent("defeat")
        {
            { "level_name", SceneManager.GetActiveScene().name },
            { "time_seconds", Time.time - tiempoInicioNivel },
            { "deaths", muertesNivel }
        });
    }

    // Para cualquier evento nuevo que quieras agregar
    public void Enviar(CustomEvent evento)
    {
        if (!Listo)
        {
            Debug.LogWarning("Analytics todavía no está listo, evento descartado.");
            return;
        }

        AnalyticsService.Instance.RecordEvent(evento);
        AnalyticsService.Instance.Flush(); // envío inmediato (útil para probar)
    }
}