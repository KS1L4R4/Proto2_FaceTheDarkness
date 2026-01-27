using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public bool redPicked;
    public bool yellowPicked;
    public bool bluePicked;

    private void Start()
    {
        redPicked = false;
        yellowPicked = false;
        bluePicked = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("RedKey"))
        {
            redPicked = true;
            Debug.Log("Red key was picked");
            Destroy(other.gameObject);
        }
        if (other.CompareTag("YellowKey"))
        {
            yellowPicked = true;
            Debug.Log("Yellow key was picked");
            Destroy(other.gameObject);
        }
        if (other.CompareTag("BlueKey"))
        {
            bluePicked = true;
            Debug.Log("Blue key was picked");
            Destroy(other.gameObject);
        }
    }
}
