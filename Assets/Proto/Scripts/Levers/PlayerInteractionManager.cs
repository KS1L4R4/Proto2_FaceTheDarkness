using UnityEngine;

public class PlayerInteractionManager : MonoBehaviour
{
    public static PlayerInteractionManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private int activeLevCount = 0;
    [SerializeField] private AudioSource exitOpenSound;
    //public Transform dropLocation;
    //public GameObject drop;

    public void ActivateLever()
    {
        activeLevCount++;
        Debug.Log("Lever activated");


        if (activeLevCount == 4)
        {
            exitOpenSound.Play();
            //OpenExit();
        }
    }

    public void DropItem()
    {
        //Instantiate(drop, dropLocation.transform.position, Quaternion.identity);
    }
}
