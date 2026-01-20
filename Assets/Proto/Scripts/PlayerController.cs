using NUnit.Framework;
using System.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PlayerController : MonoBehaviour
{
    public Rigidbody rb;
    public WeaponBehaviour weaponBehaviour;
    public HealthManager healthManager;
    public Light lamp;
    public Volume fxVolume;
    public bool isAiming;
    public bool isReloading;
    public bool lightOn;
    public int bulletsLeft;
    public int maxBullets;
    public float playerSpeed;
    public float playerRotation;
    public float loadingTime;
    public float oil;
    public float maxOil;
    public float oilRate;
    Vignette vignette;

    private void Awake()
    {
        fxVolume.profile.TryGet(out vignette);
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerSpeed = 3f;
        playerRotation = 0.5f;
        maxBullets = 6;
        bulletsLeft = maxBullets;
        isAiming = false;
        isReloading = false;
        loadingTime = 1f;
        maxOil = 10f;
        oil = maxOil;
        oilRate = 1f;
        lightOn = false;
        lamp.enabled = false;
        vignette.intensity.value = 1f;
    }

    void Update()
    {
        //Movement and rotation related
        if (isReloading == false && healthManager.isAlive == true) //Player can move
        {
            rb.linearVelocity = transform.forward * (Input.GetAxis("Vertical") * playerSpeed);
            transform.Rotate(new Vector3 (0, Input.GetAxis("Horizontal") * playerRotation, 0));    
        }
        if (Input.GetKey(KeyCode.LeftShift)) //Player runs
        {
            if (isAiming == false && isReloading == false)
            {
                playerSpeed = 5f;
            }
        }
        if (Input.GetKeyUp(KeyCode.LeftShift)) //Player walks
        {
            if (isAiming == false && isReloading == false)
            {
                playerSpeed = 3f;
            }
        }

        //Weapons related
        if (Input.GetKeyDown(KeyCode.Mouse1)) //Player aims
        {
            Aiming();
            weaponBehaviour.DrawLaser();
        }
        if (Input.GetKeyUp(KeyCode.Mouse1)) //Player stops aiming
        {
            Unaim();
        }
        if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.E) && isAiming == true) //Player shoots
        {
            if (bulletsLeft > 0)
            {
                weaponBehaviour.Shoot();
                bulletsLeft--;
            }

        }
        if (Input.GetKeyDown(KeyCode.R)) //Player reloads
        {
            if (isAiming == true && bulletsLeft < maxBullets)
            {
                StartCoroutine(Reaload());
            }
        }

        //Oil and lamp related
        if(oil > 0)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                lightOn = !lightOn;
                lamp.enabled = lightOn;
                ToggleVignette(lightOn);
            }
        }
        else
        {
            lightOn = false;
            lamp.enabled = lightOn;
            ToggleVignette(lightOn);
            oil = 0;
        }
        if (lightOn == true)
        {
            oil -= oilRate * Time.deltaTime;
        }

        //Testing
        if (Input.GetKeyDown(KeyCode.B))
        {
            oil = maxOil;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BatteryPack"))
        {
            oil = maxOil;
            Destroy(other.gameObject);
        }
    }

    private void Aiming()
    {
        isAiming = true;
        playerSpeed = 0f;
        playerRotation = 0.1f;
    }

    private void Unaim()
    {
        isAiming = false;
        playerSpeed = 3f;
        playerRotation = 0.5f;
    }

    void ToggleVignette(bool on)
    {
        vignette.intensity.value = on ? 0.5f : 1f;
    }

    IEnumerator Reaload()
    {
        isReloading = true;
        yield return new WaitForSeconds(loadingTime);
        bulletsLeft = maxBullets;
        isReloading = false;
    }
}
