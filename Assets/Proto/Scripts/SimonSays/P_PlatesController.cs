using UnityEngine;

public class P_PlatesController : MonoBehaviour
{
    private Material plateMat;

    public int plateNum;

    private SimonManager simonManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        plateMat = GetComponent<MeshRenderer>().material;
        simonManager = gameObject.transform.GetComponentInParent<SimonManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            plateMat.EnableKeyword("_EMISSION");
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            plateMat.DisableKeyword("_EMISSION");
            simonManager.ColourPressed(plateNum);
        }
    }
}
