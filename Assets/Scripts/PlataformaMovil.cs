using UnityEngine;

public class PlataformaMovil : MonoBehaviour
{
    [Header("Movimiento")]
    public Vector3 offsetMovimiento = new Vector3(0, 0, 5);
    public float velocidad = 2f;

    [Header("Modo de movimiento")]
    public bool idaYVuelta = true;

    [Header("Jugador")]
    public string etiquetaJugador = "Player";

    private Vector3 posicionInicial;
    private Vector3 posicionFinal;
    private float t = 0f;
    private bool yendo = true;

    private Transform jugadorActual = null;

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

    // ============================================================
    // PARENTING DINAMICO DEL JUGADOR SOBRE LA PLATAFORMA
    // ============================================================

    private void OnCollisionEnter(Collision collision)
    {
        // Comprobamos si el objeto que colisiona es el jugador
        if (collision.collider.CompareTag(etiquetaJugador))
        {
            // Verificar que esta encima de la plataforma (normal del contacto apunta hacia arriba)
            foreach (ContactPoint contact in collision.contacts)
            {
                if (Vector3.Dot(contact.normal, Vector3.up) > 0.5f)
                {
                    jugadorActual = collision.collider.transform;
                    jugadorActual.SetParent(transform); // Hacer hijo de la plataforma
                    return;
                }
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        // Cuando el jugador sale de la plataforma, quitar el parent
        if (jugadorActual != null && collision.collider.transform == jugadorActual)
        {
            jugadorActual.SetParent(null);   // Quitar parent
            jugadorActual = null;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        // Asegurarnos de que el jugador sigue siendo hijo mientras esta en contacto
        if (collision.collider.CompareTag(etiquetaJugador) && jugadorActual == null)
        {
            foreach (ContactPoint contact in collision.contacts)
            {
                if (Vector3.Dot(contact.normal, Vector3.up) > 0.5f)
                {
                    jugadorActual = collision.collider.transform;
                    jugadorActual.SetParent(transform);
                    return;
                }
            }
        }
    }
}