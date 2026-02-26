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
    public int missingLevers;
    [SerializeField] private AudioSource exitOpenSound;
    //public Transform dropLocation;
    //public GameObject drop;

    public void ActivateLever()
    {
        activeLevCount++;

        missingLevers--;

        Debug.Log("Lever activated");

        if (activeLevCount == 4)
        {
            exitOpenSound.Play();
            MessageManager.Instance.ShowExitOpenedMessage();
            //OpenExit();
        }
        else
        {
            MessageManager.Instance.UpdateLeversLeftText(missingLevers);
            MessageManager.Instance.ShowLeversLeftText();
        }
    }

    public void DropItem()
    {
        //Instantiate(drop, dropLocation.transform.position, Quaternion.identity);
    }
}
