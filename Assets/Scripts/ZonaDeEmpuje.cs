using UnityEngine;

public class ZonaDeEmpuje : MonoBehaviour
{
    [Header("Fuerza aplicada al jugador")]
    public float fuerzaEmpuje = 10f;

    [Header("Tipo de fuerza")]
    public ForceMode modoFuerza = ForceMode.Impulse;

    [Header("Dirección del empuje (local a la zona)")]
    public Vector3 direccionLocal = new Vector3(0, 0, -1);

    [Header("Aplicar fuerza continuamente mientras esté dentro")]
    public bool empujeContinuo = false;

    private void OnTriggerEnter(Collider other)
    {
        Rigidbody rb = other.attachedRigidbody;

        if (rb != null && !empujeContinuo)
        {
            Vector3 direccion = transform.TransformDirection(direccionLocal.normalized);
            rb.AddForce(direccion * fuerzaEmpuje, modoFuerza);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!empujeContinuo) return;

        Rigidbody rb = other.attachedRigidbody;

        if (rb != null)
        {
            Vector3 direccion = transform.TransformDirection(direccionLocal.normalized);
            rb.AddForce(direccion * fuerzaEmpuje * Time.deltaTime, ForceMode.Acceleration);
        }
    }
}
