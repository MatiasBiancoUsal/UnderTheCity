using UnityEngine;
using Unity.Services.Core;
using UnityEngine.UnityConsent;


public class iniciarAnalytics : MonoBehaviour
{
    async void Start()
    {
        try
        {
            await UnityServices.InitializeAsync();

            if (PlayerPrefs.GetInt("TerminosAceptados", 0) == 1)
            {
                RecoleccionDatos(true);
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error al iniciar Unity Services: " + e.Message);
        }
    }

 
    public void RecoleccionDatos(bool consentimiento)
    {
        ConsentState estadoConsentimiento = new ConsentState
        {
            AnalyticsIntent = consentimiento ? ConsentStatus.Granted : ConsentStatus.Denied,
            AdsIntent = ConsentStatus.Denied
        };

      
        EndUserConsent.SetConsentState(estadoConsentimiento);
    }
}