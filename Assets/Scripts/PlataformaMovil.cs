using UnityEngine;

public class PlataformaMovil : MonoBehaviour
{
    [Header("Movimiento")]
    public Vector3 offsetMovimiento = new Vector3(0, 0, 5);
    public float velocidad = 2f;

    [Header("Modo de movimiento")]
    public bool idaYVuelta = true;

    private Vector3 posicionInicial;
    private Vector3 posicionFinal;
    private float t = 0f;
    private bool yendo = true;

    void Start()
    {
        posicionInicial = transform.position;
        posicionFinal = posicionInicial + offsetMovimiento;
    }

    void Update()
    {
        if (idaYVuelta)
        {
            MovimientoIdaVuelta();
        }
        else
        {
            MovimientoSoloIda();
        }
    }

    void MovimientoIdaVuelta()
    {
        t += (yendo ? 1 : -1) * velocidad * Time.deltaTime;
        t = Mathf.Clamp01(t);

        transform.position = Vector3.Lerp(posicionInicial, posicionFinal, t);

        if (t == 1f) yendo = false;
        if (t == 0f) yendo = true;
    }

    void MovimientoSoloIda()
    {
        t += velocidad * Time.deltaTime;
        transform.position = Vector3.Lerp(posicionInicial, posicionFinal, Mathf.PingPong(t, 1f));
    }
}
