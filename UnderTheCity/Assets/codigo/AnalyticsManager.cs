using System;
using System.Threading.Tasks;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Analytics;

public class AnalyticsManager : MonoBehaviour
{
    public static AnalyticsManager Instance { get; private set; }

    private bool serviciosListos;

    private async void Awake()
    {

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        await InicializarServicios();
    }

    private void Start()
    {


    }

    private async Task InicializarServicios()
    {
        try
        {
            if (UnityServices.State == ServicesInitializationState.Uninitialized)
            {
                await UnityServices.InitializeAsync();
            }
            else
            {
                while (UnityServices.State == ServicesInitializationState.Initializing)
                {
                    await Task.Yield();
                }
            }

            serviciosListos = UnityServices.State == ServicesInitializationState.Initialized;

            if (serviciosListos)
                Debug.Log("AnalyticsManager: servicios inicializados.");
        }
        catch (Exception e)
        {
            serviciosListos = false;
            Debug.LogError("Error al inicializar Analytics: " + e.Message);
        }
    }

    private async Task<bool> VerificarServicios()
    {
        if (!serviciosListos)
        {
            await InicializarServicios();
        }

        return serviciosListos;
    }





    public async void RegistrarNivelIniciado(string nombreNivel)
    {
        if (!await VerificarServicios())
            return;

        CustomEvent evento = new CustomEvent("NivelIniciado")
        {
            { "nombreNivel", nombreNivel }
        };

        AnalyticsService.Instance.RecordEvent(evento);
        AnalyticsService.Instance.Flush(); 

        Debug.Log("Evento NivelIniciado. nombreNivel: " + nombreNivel);
    }

    public async void RegistrarNivelCompletado(
        string nombreNivel,
        float tiempoSegundos,
        int muertes,
        int muertesVagabundo,
        int muertesRata,
        int puntaje)
    {
        if (!await VerificarServicios())
            return;

        CustomEvent evento = new CustomEvent("NivelCompletado")
        {
            { "nombreNivel", nombreNivel },
            { "tiempoSegundos", tiempoSegundos },
            { "muertes", muertes },
            { "muertesVagabundo", muertesVagabundo },
            { "muertesRata", muertesRata },
            { "puntaje", puntaje }
        };

        AnalyticsService.Instance.RecordEvent(evento);
        AnalyticsService.Instance.Flush();

        Debug.Log("Evento NivelCompletado. Nivel: " + nombreNivel);
        Debug.Log("Evento NivelCompletado. tiempoTranscurrido: " + tiempoSegundos);
        Debug.Log("Evento NivelCompletado. MatarVagabundo: " + muertesVagabundo);
        Debug.Log("Evento NivelCompletado. MatarRata: " + muertesRata);
        Debug.Log("Evento NivelCompletado. MatarVagabundo+MatarRata: " + muertes);
        Debug.Log("Evento NivelCompletado. cantidadEstrellas: " + puntaje);

    }

    public async void RegistrarJugadorMurio(
        string personaje,
        string nombreNivel,
        string enemigos,
        float posicionX,
        float posicionY,
        int muertesPersonaje)
    {
        if (!await VerificarServicios())
            return;

        CustomEvent evento = new CustomEvent("JugadorMurio")
        {
            { "personaje", personaje },
            { "nombreNivel", nombreNivel },
            { "enemigos", enemigos },
            { "posicionX", posicionX },
            { "posicionY", posicionY },
            { "muertesPersonaje", muertesPersonaje }
        };

        AnalyticsService.Instance.RecordEvent(evento);
        AnalyticsService.Instance.Flush();

        Debug.Log("Evento JugadorMurio. personaje: " + personaje);
        Debug.Log("Evento JugadorMurio. nombreNivel: " + nombreNivel);
        Debug.Log("Evento JugadorMurio. enemigos: " + enemigos);
        Debug.Log("Evento JugadorMurio. posicionX: " + posicionX);
        Debug.Log("Evento JugadorMurio. posicionY: " + posicionY);
        Debug.Log("Evento JugadorMurio. muertesPersonaje: " + muertesPersonaje); 

        
    }

    public async void RegistrarDerrota(
        string nombreNivel,
        float tiempoSegundos,
        int muertes)
    {
        if (!await VerificarServicios())
            return;

        CustomEvent evento = new CustomEvent("Derrota")
        {
            { "nombreNivel", nombreNivel },
            { "tiempoSegundos", tiempoSegundos },
            { "muertes", muertes }
        };

        AnalyticsService.Instance.RecordEvent(evento);
        AnalyticsService.Instance.Flush();

        Debug.Log("Evento Derrota. Nivel: " + nombreNivel);
    }
}