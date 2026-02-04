using System.Collections.Generic;
using UnityEngine;

public class Doll : MonoBehaviour
{
    int found = 0;
    public GameObject drop;
    public List<Transform> locationTargets;

    public void Start()
    {
        int start = Random.Range(0, locationTargets.Count);
        transform.position = locationTargets[start].position;
        locationTargets.Remove(locationTargets[start]);
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            found++;
            int next = Random.Range(0, locationTargets.Count -1);
            transform.position = locationTargets[next].position;
            locationTargets.Remove(locationTargets[next]);
        }

        if (found == 3)
        {
            gameObject.SetActive(false);
            DropItem();
        }
    }
    public void DropItem()
    {
        Instantiate(drop,transform.position,Quaternion.identity);
    }
}
