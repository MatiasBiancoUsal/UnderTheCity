using UnityEngine;

public class Enemigo : MonoBehaviour
{
    [Header("MOVIMIENTO")]
    public Transform puntoA;
    public Transform puntoB;
    public float velocidad = 2f;

    [Header("CAMPO DE VISION")]
    public Transform conoVision;

    [Header("DIRECCION INICIAL")]
    public bool empiezaMirandoDerecha = true;

    private bool yendoAB = true;
    private bool detenido = false;

    private Vector3 posicionA;
    private Vector3 posicionB;

    private SpriteRenderer spriteRenderer;

    private Vector3 posicionOriginalCono;
    private Quaternion rotacionOriginalCono;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (puntoA != null)
            posicionA = puntoA.position;

        if (puntoB != null)
            posicionB = puntoB.position;

        if (puntoA != null)
            transform.position = posicionA;

        if (conoVision != null)
        {
            conoVision.SetParent(transform, true);

            posicionOriginalCono = conoVision.localPosition;
            rotacionOriginalCono = conoVision.localRotation;
        }

        if (empiezaMirandoDerecha)
            MirarDerecha();
        else
            MirarIzquierda();
    }

    void Update()
    {
        if (detenido)
            return;

        Mover();
    }

    void Mover()
    {
        if (puntoA == null || puntoB == null)
            return;

        Vector3 destino = yendoAB ? posicionB : posicionA;

        transform.position = Vector3.MoveTowards(
            transform.position,
            destino,
            velocidad * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, destino) < 0.01f)
        {
            transform.position = destino;

            yendoAB = !yendoAB;

            CambiarDireccion();
        }
    }

    void CambiarDireccion()
    {
        if (yendoAB)
        {
            if (posicionB.x > posicionA.x)
                MirarDerecha();
            else
                MirarIzquierda();
        }
        else
        {
            if (posicionA.x > posicionB.x)
                MirarDerecha();
            else
                MirarIzquierda();
        }
    }

    void MirarDerecha()
    {
        if (spriteRenderer != null)
            spriteRenderer.flipX = false;

        if (conoVision != null)
        {
            conoVision.localPosition = posicionOriginalCono;
            conoVision.localRotation = rotacionOriginalCono;
        }
    }

    void MirarIzquierda()
    {
        if (spriteRenderer != null)
            spriteRenderer.flipX = true;

        if (conoVision != null)
        {
            conoVision.localPosition = new Vector3(
                -posicionOriginalCono.x,
                posicionOriginalCono.y,
                posicionOriginalCono.z
            );

            conoVision.localRotation =
                rotacionOriginalCono * Quaternion.Euler(0f, 0f, 180f);
        }
    }

    public void Parar()
    {
        detenido = true;
    }

    public void Reanudar()
    {
        detenido = false;
    }
}