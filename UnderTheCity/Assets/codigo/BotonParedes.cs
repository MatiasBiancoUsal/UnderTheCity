using UnityEngine;

public class BotonParedes : MonoBehaviour
{
    public Paredes[] paredes;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        foreach (Paredes pared in paredes)
        {
            pared.Desaparecer();
        }
    }
}
