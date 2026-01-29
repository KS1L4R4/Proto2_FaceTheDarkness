using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;

public class Doll : MonoBehaviour
{
    int found = 0;
    public Transform[] spawnPoints;
    public GameObject drop;


    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            found++;
            int nextPoint = Random.Range(0, spawnPoints.Length);
            transform.position = spawnPoints[nextPoint].position;
        }

        if (found == 3)
        {
            gameObject.SetActive(false);
            DropItem();
        }
    }
    public void DropItem()
    {
        Instantiate(drop);
    }
}
