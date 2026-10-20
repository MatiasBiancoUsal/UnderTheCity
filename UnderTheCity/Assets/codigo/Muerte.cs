using UnityEngine;
using UnityEngine.SceneManagement;


public class ZonaMuerte : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Vagabundo"))
        {
            MatarVagabundo(collision.gameObject);
        }
        else if (collision.CompareTag("Rata"))
        {
            MatarRata(collision.gameObject);
        }
    }
    private void Start()
    {
         nivActualName = SceneManager.GetActiveScene().name;

    }
    string nivActualName;

    private void MatarVagabundo(GameObject jugador)
    {
        VagabundoControl vagabundo = jugador.GetComponent<VagabundoControl>();


        if (vagabundo != null)
        {
            vagabundo.Morir();
            int numeroMuertes = PlayerPrefs.GetInt("MV",0);
            PlayerPrefs.SetInt("MV",numeroMuertes+1);
            RegistrarDerrota();



            AnalyticsManager.Instance.RegistrarJugadorMurio("vagabundo",
                nivActualName,
                gameObject.tag,
                vagabundo.gameObject.transform.position.x,
                vagabundo.gameObject.transform.position.y,
                PlayerPrefs.GetInt("MV", 0));

        }
        else
        {
            MostrarPantallaDerrota();
        }

    }

    private void MatarRata(GameObject jugador)
    {
        RataControl rata = jugador.GetComponent<RataControl>();

        if (rata != null)
        {
            rata.Morir();
            int numeroMuertes = PlayerPrefs.GetInt("MR", 0);
            PlayerPrefs.SetInt("MR", numeroMuertes + 1);
            RegistrarDerrota();


            AnalyticsManager.Instance.RegistrarJugadorMurio("rata",
                nivActualName,
                gameObject.tag,
                rata.gameObject.transform.position.x,
                rata.gameObject.transform.position.y,
                PlayerPrefs.GetInt("MR", 0));

        }
        else
        {
            MostrarPantallaDerrota();
        }
    }

    private void RegistrarDerrota()
    {
        AnalyticsManager.Instance.RegistrarDerrota(
            nivActualName,
            GameManager.instancia.tiempoTranscurrido,
            PlayerPrefs.GetInt("MV", 0) + PlayerPrefs.GetInt("MR", 0)
        );
    }


    private void MostrarPantallaDerrota()
    {
        if (PantallaDerrota.instancia != null)
        {
            PantallaDerrota.instancia.Mostrar();
        }
        else
        {
            Debug.LogWarning("No se encontró PantallaDerrota en la escena.");
        }
    }

}