using UnityEngine;

public class Paredes : MonoBehaviour
{
    [Header("Movimiento")]
    public float alturaSubida = 3f;
    public float velocidad = 3f;

    private Vector3 posicionInicial;
    private Vector3 posicionFinal;

    private bool moviendose = false;

    void Start()
    {
        posicionInicial = transform.position;

        posicionFinal = new Vector3(
            posicionInicial.x,
            posicionInicial.y + alturaSubida,
            posicionInicial.z
        );
    }

    void Update()
    {
        if (!moviendose)
            return;

        transform.position = Vector3.MoveTowards(
            transform.position,
            posicionFinal,
            velocidad * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, posicionFinal) < 0.01f)
        {
            transform.position = posicionFinal;
            moviendose = false;
        }
    }

    public void ActivarMovimiento()
    {
        moviendose = true;
    }
}