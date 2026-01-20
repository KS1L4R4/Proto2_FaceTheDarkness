using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public Vector3 offset;
    //public float smoothSpeed = 5f;

    void LateUpdate()
    {
        transform.position = player.position + offset;
        /*Vector3 desiredPosition = player.position + offset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);*/
    }
}
