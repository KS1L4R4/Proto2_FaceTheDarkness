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
    float turnVelocity;

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
        maxOil = 20f;
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
            float targetAngle = Mathf.Atan2(moveVector.x, moveVector.z) * Mathf.Rad2Deg + camara.rotation.eulerAngles.y;

            float angle = Mathf.SmoothDampAngle(
                transform.eulerAngles.y,
                targetAngle,
                ref turnVelocity,
                playerRotation
            );

            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            rb.linearVelocity = moveDir * playerSpeed;
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
                UIManager.instance.UpdateBullets(bulletsLeft);
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

        //Simplificar el código y encapsular la lógica de la linterna en una sola función
        if (Input.GetKeyDown(KeyCode.F))
        {
            ToggleLight(!lightOn);
		}

        //if (oil > 0)
        //{
        //    if (Input.GetKeyDown(KeyCode.F))
        //    {
        //        lightOn = !lightOn;
        //        lamp.enabled = lightOn;
        //        light.gameObject.SetActive(true);
        //        darkness.gameObject.SetActive(false);
        //        dark = false;
        //        ToggleVignette(lightOn);
        //    }
        //}
        //else
        //{
        //    lightOn = false;
        //    lamp.enabled = lightOn;
        //    ToggleVignette(lightOn);
        //    light.gameObject.SetActive(false);
        //    darkness.gameObject.SetActive(true);
        //    oil = 0;
        //}

        if (lightOn == true)
        {
            oil -= oilRate * Time.deltaTime;
            UIManager.instance.UpdateOil(oil);

            if(oil <= 0)
            {
                ToggleLight(false);
			}
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

    private void ToggleLight(bool active)
    {
        //checar si el jugador intenta prender la linterna, si no tiene aceite no ejecutar el resto
        if(oil <= 0)
        {
            active = false;
            oil = 0;
			UIManager.instance.UpdateOil(oil);
			return;
        }

        lightOn = active;
		lamp.enabled = active;

        //Cuidar referencias nulas
		light?.gameObject?.SetActive(active);
		darkness?.gameObject?.SetActive(active);

		dark = active;
		ToggleVignette(active);
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
        DoorDesignator door = collision.gameObject.GetComponent<DoorDesignator>();
        if(door == null)
        {
            return;
        }
        for(int i = 0; i < inventory.keyList.Count; i++)
        {
            InteractuableDesignator key = inventory.keyList[i];
            if(key.Designation == door.requiredKey)
            {
                door.OpenDoor();
                inventory.RemoveKey(key);
                return;
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
