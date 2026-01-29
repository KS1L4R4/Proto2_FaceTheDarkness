using NUnit.Framework;
using System.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PlayerController : MonoBehaviour
{
    //Elementos de la escena
    private PlayerInventory inventory;
    public Rigidbody rb;
    public WeaponBehaviour weaponBehaviour;
    public HealthManager healthManager;
    public Light lamp;
    public Volume fxVolume;
    public Volume light;
    public Volume darkness;
    Vignette vignette;
    public Transform camara;
    public float smoothtime = 5f;
    public bool dark = false;

    //Booleanos
    public bool isAiming;
    public bool isReloading;
    public bool lightOn;

    //Enteros
    public int bulletsLeft;
    public int maxBullets;

    //Flotantes
    public float playerSpeed;
    public float playerRotation;
    public float loadingTime;
    public float oil;
    public float maxOil;
    public float oilRate;


    private void Awake()
    {
        fxVolume.profile.TryGet(out vignette);
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        inventory = GetComponent<PlayerInventory>();
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
        Vector3 moveVector = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        rb.linearVelocity = moveVector.normalized * playerSpeed;

        if (moveVector.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(moveVector.x, moveVector.z) * Mathf.Rad2Deg + camara.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref smoothtime, smoothtime);

            transform.rotation = Quaternion.Euler(0, angle, 0);
            Vector3 movedir = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;

            rb.linearVelocity = movedir * playerSpeed + new Vector3(0, rb.linearVelocity.y, 0);
        }

        if (Input.GetKey(KeyCode.LeftShift)) //Player runs
        {
            playerSpeed = 5f;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift)) //Player walks
        {
            playerSpeed = 3f;
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
        if (oil > 0)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                lightOn = !lightOn;
                lamp.enabled = lightOn;
                light.gameObject.SetActive(true);
                darkness.gameObject.SetActive(false);
                dark = false;
                ToggleVignette(lightOn);
            }
        }
        else
        {
            lightOn = false;
            lamp.enabled = lightOn;
            ToggleVignette(lightOn);
            light.gameObject.SetActive(false);
            darkness.gameObject.SetActive(true);
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
        if (oil == 0 || dark == true)
        {
            healthManager.SanidadRes();
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

    private void OnCollisionEnter(Collision collision) //Revision con el inventario por las llaves de colores
    {
        if (collision.collider.CompareTag("RedDoor"))
        {
            if(inventory.redPicked == true)
            {
                Destroy(collision.gameObject);
            }
            else
            {
                Debug.Log("Red key missing");
            }
        }
        if (collision.collider.CompareTag("YellowDoor"))
        {
            if (inventory.yellowPicked == true)
            {
                Destroy(collision.gameObject);
            }
            else
            {
                Debug.Log("Yellow key missing");
            }
        }
        if (collision.collider.CompareTag("BlueDoor"))
        {
            if (inventory.bluePicked == true)
            {
                Destroy(collision.gameObject);
            }
            else
            {
                Debug.Log("Blue key missing");
            }
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

    public void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Dark"))
        {
            dark = true;
        } else if (other.gameObject.CompareTag("Light"))
        {
            dark = false;
        }
    }

    IEnumerator Reaload()
    {
        isReloading = true;
        yield return new WaitForSeconds(loadingTime);
        bulletsLeft = maxBullets;
        isReloading = false;
    }
}
