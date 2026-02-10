using System.Collections.Generic;
using UnityEngine;

public class Doll : MonoBehaviour
{
    int found = 0;
    public GameObject drop;
    public List<Transform> locationTargets;
    public Transform dropLocation;
    public Transform player;

    public void Start()
    {
        int start = Random.Range(0, locationTargets.Count);
        transform.position = locationTargets[start].position;
        locationTargets.Remove(locationTargets[start]);
    }

    public void Update()
    {
        transform.LookAt(player);
    }

    private void LateUpdate()
    {
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            found++;
            if (found == 3)
            {
                DropItem();
                gameObject.SetActive(false);
                return;
            }
            int next = Random.Range(0, locationTargets.Count -1);
            transform.position = locationTargets[next].position;
            locationTargets.Remove(locationTargets[next]);
        }

        
    }
    public void DropItem()
    {
        Instantiate(drop,dropLocation.transform.position,Quaternion.identity);
    }
}
