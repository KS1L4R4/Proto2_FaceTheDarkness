using Unity.VisualScripting;
using UnityEngine;

public class WeaponBehaviour : MonoBehaviour
{   
    //Elementos de la escena
    public LayerMask hitLayers;
    public PlayerController playerController;
    public Transform firePoint;
    LineRenderer laserLine;

    //Enteros
    public float range =10000f;

    //Flotantes
    public int damage = 10;


    private void Awake()
    {
        laserLine = GetComponentInChildren<LineRenderer>();
        laserLine.enabled = false;
    }

    void Update()
    {

    }

    public void DrawLaser()
    {
        laserLine.enabled = true;
        Vector3 start = firePoint.position;
        Vector3 direction = transform.forward;
        laserLine.SetPosition(0, start);
        if (Physics.Raycast(start, direction, out RaycastHit hit, range, hitLayers))
        {
            laserLine.SetPosition(1, hit.point);
        }
        else
        {
            laserLine.SetPosition(1, start + direction * range);
        }
    }
}
