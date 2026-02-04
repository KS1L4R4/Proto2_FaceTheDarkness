using UnityEngine;

public class DoorDesignator : MonoBehaviour
{
    public InteractuableID requiredKey;

    public void OpenDoor()
    {
        gameObject.SetActive(false);
    }
}
