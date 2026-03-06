using UnityEngine;

public class HoleSpawn : MonoBehaviour
{
    public Transform player;
    public Transform spawn;
    
    public void OnTriggerEnter (Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player.transform.position = spawn.position;
        }
    }
}
