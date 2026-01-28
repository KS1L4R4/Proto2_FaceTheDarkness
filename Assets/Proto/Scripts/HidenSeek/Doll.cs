using UnityEngine;

public class Doll : MonoBehaviour
{
    int found = 0;
    public GameObject[] spawnPoints;
    public bool moved = false;

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            moved = false;
            found++;
            if (moved == false)
            {
                    //transform.position = spawnPoints.transform.position;
                moved = true;
            }  
        }

        if (found == 3)
        {
            gameObject.SetActive(false);
        }
    }
}
