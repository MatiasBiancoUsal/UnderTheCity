using UnityEngine;
using Unity.Services.Core;
using UnityEngine.UnityConsent;
using Unity.Services.Analytics;

public class iniciarAnalytics : MonoBehaviour
{
    async void Start()
    {
        await UnityServices.InitializeAsync();
    }

    public void RecoleccionDatos(bool consentimiento)
    {
        ConsentState estadoConsentimiento = new ConsentState
        {
            AnalyticsIntent = consentimiento ? ConsentStatus.Granted : ConsentStatus.Denied,
            AdsIntent = ConsentStatus.Denied
        };

        // Avisa a Unity cuál es la decisión del usuario
        EndUserConsent.SetConsentState(estadoConsentimiento);
    }
}