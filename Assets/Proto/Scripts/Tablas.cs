using UnityEngine;
using DG.Tweening;

public class Tablas : MonoBehaviour
{
    public float rotation = 0f;
    public Transform hinge;
    int once = 0;
    public void OnCollisionEnter(Collision collision)
    {
        if (once == 0)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                once++;

                hinge.DOLocalRotate(new Vector3(0, 0, 90), 0.5f).SetEase(Ease.OutBounce);
            }  
        }
        
    }
}
