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
        if (playerController.isAiming == true)
        {
            if (playerController.bulletsLeft >= 1)
            {
                laserLine.startColor = Color.red;
                laserLine.startColor = Color.red;
            }
            if (playerController.bulletsLeft == 0)
            {
                laserLine.startColor = Color.grey;
                laserLine.startColor = Color.grey;
            }
            if (playerController.isReloading == true)
            {
                laserLine.startColor = Color.yellow;
                laserLine.startColor = Color.yellow;
            }
            DrawLaser();
        }
        if(playerController.isAiming == false && playerController.isReloading == false)
        {
            laserLine.enabled = false;
        }
    }

    public void Shoot()
    {
        Ray ray = new Ray(firePoint.position, transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, range, hitLayers))
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                hit.transform.GetComponent<EnemyBehaviourAndHealth>().HarmEnemy();
            }
        }
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
