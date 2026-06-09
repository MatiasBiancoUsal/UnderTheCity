using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

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

    void Awake()
    {
        instancia = this;
    }

    void Update()
    {
        
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
            SceneManager.LoadScene(siguienteNivel);
        }
    }
}