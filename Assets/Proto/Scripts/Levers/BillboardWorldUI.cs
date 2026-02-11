using UnityEngine;

public class BillboardWorldUI : MonoBehaviour
{
    [SerializeField] Camera pCam;

    private void LateUpdate()
    {
        if (pCam != null)
        {
            transform.LookAt(pCam.transform.position + pCam.transform.forward);
        }
    }
}
