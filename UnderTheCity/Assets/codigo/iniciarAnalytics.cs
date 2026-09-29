using UnityEngine;
using Unity.Services.Core;
using UnityEngine.UnityConsent; // Para aceptar consentimiento de los usuarios
using Unity.Services.Analytics; // utilizar analytics

public class iniciarAnalytics: MonoBehaviour
{
    async void start()
    {
        await UnityServices.InitializeAsync();

    }
    public void RecoleccionDatos(bool consentimiento)

    { //Posteriormente, para la recolección de datos, Primero necesitamos crear una nueva variable de tipo ConsentState
        ConsentState estadoConsentimiento = new ConsentState
        {
            //  En el caso de que queramos utilizar los analytics para la recolección de datos, le decimos a la variable
            //AnalyticsIntent = ConsentStatus.Granted. // Osea, aca le decimos que el usuario dio su consentimiento
            //  Caso de que queramos decirle que no damos consentimiento, usamos  AnalyticsIntent = ConsentStatus.Denied
            AnalyticsIntent = consentimiento ? ConsentStatus.Granted : ConsentStatus.Denied, // (aviso por las dudas, cuando usamos ‘?’ es como un if/else)
            AdsIntent = ConsentStatus.Denied
            // Este caso de aca a lo último es para dar consentimiento o no para el uso de anuncios. En este caso le decimos que no
        };

        // Y despues usamos la siguiente funcion para avisar a los servicios de Unity que el usuario dio su consentimiento.


    }
}
