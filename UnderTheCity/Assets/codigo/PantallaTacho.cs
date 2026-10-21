using UnityEngine;

public class PantallaTacho : MonoBehaviour
{
    public GameObject canvasTerminos;

    public void Aceptar()
    {
        canvasTerminos.SetActive(false);
    }
}