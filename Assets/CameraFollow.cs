using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;  // Riferimento al giocatore
    public float smoothSpeed = 0.125f;  // Velocità di transizione della telecamera
    public Vector3 offset;  // Offset tra la telecamera e il giocatore

    void FixedUpdate()
    {
        // Calcola la posizione desiderata della telecamera, con l'offset
        Vector3 desiredPosition = player.position + offset;
        
        // Calcola la posizione finale della telecamera con smoothing
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        
        // Imposta la posizione della telecamera
        transform.position = smoothedPosition;
    }
}
