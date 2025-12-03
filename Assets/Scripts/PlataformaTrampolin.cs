using UnityEngine;

public class PlataformaTrampolin : MonoBehaviour
{
    [Header("Fuerza del impulso hacia arriba")]
    public float fuerzaImpulso = 20f;

    [Header("Modo de impulso")]
    public ForceMode modoFuerza = ForceMode.Impulse;

    [Header("Cooldown entre impulsos")]
    public float tiempoEntreImpulsos = 0.2f;

    private float tiempoUltimoImpulso = -999f;

    private void OnCollisionEnter(Collision collision)
    {
        Rigidbody rb = collision.rigidbody;

        // Solo si hay Rigidbody y pasó el cooldown
        if (rb != null && Time.time >= tiempoUltimoImpulso + tiempoEntreImpulsos)
        {
            tiempoUltimoImpulso = Time.time;

            // Resetea la velocidad vertical para que el impulso sea consistente
            Vector3 vel = rb.linearVelocity;
            vel.y = 0;
            rb.linearVelocity = vel;

            // Aplica el impulso hacia arriba
            rb.AddForce(Vector3.up * fuerzaImpulso, modoFuerza);
        }
    }
}
