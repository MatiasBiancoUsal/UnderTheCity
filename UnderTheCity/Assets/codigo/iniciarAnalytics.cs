using System.Threading.Tasks;
using UnityEngine;
using Unity.Services.Core;
using UnityEngine.UnityConsent;

public class iniciarAnalytics : MonoBehaviour
{
    async void Start()
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

        if (PlayerPrefs.GetInt("TerminosAceptados", 0) == 1)
        {
            RecoleccionDatos(true);
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