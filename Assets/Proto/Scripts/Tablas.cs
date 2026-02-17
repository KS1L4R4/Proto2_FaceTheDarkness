using UnityEngine;


public class Tablas : MonoBehaviour
{
    public float rotation = 0f;
    public Transform hinge;
    int once = 0;

    public void OnCollisionStay(Collision collision)
    {
        if (once == 0)
        {
          if (collision.gameObject.CompareTag("Player"))
        {
            once++;
            transform.RotateAround(hinge.position, hinge.forward, rotation);
        }  
        }
        
    }
}
