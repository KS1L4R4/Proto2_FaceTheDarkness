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
    private PlayerInventory playerInventory;
    private PlayerInventory inventory;
    public EnemyBehaviourAndHealth enemyBehaviour;
    public Rigidbody rb;
    public HealthManager healthManager;
    public Light lamp;
    public Volume fxVolume;
    public Volume safeLight;
    public Volume darkness;
    private Vignette vignette;
    public Transform camara;
    public bool dark = false;
    public bool lightOn;
    public float playerSpeed;
    public float playerRotation;
    public float loadingTime;
    public float oil;
    public float maxOil;
    public float oilRate;
    public float smoothtime = 5f;
    public float turnVelocity;
    private string enemyTag = "Enemy";

    private void Awake()
    {
        fxVolume.profile.TryGet(out vignette);
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerInventory = GetComponent<PlayerInventory>();
        playerSpeed = 3f;
        playerRotation = 0.5f;
        loadingTime = 1f;
        maxOil = 20f;
        oil = maxOil;
        oilRate = 1f;
        lightOn = false;
        lamp.enabled = false;
        vignette.intensity.value = 0.2f;
    }

    void Update()
    {


        //Vector3 moveVector = new Vector3(Input.GetAxisRaw("Horizontal"),0, Input.GetAxisRaw("Vertical")).normalized;
        //rb.linearVelocity = Vector3.up * Physics.gravity.y;
        Vector3 moveVector = new Vector3(Input.GetAxisRaw("Horizontal"),  rb.linearVelocity.y, Input.GetAxisRaw("Vertical"));
        Vector3 velocity = new Vector3(moveVector.x * playerSpeed,rb.linearVelocity.y,moveVector.z * playerSpeed);

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
            rb.linearVelocity = new Vector3(moveDir.x * playerSpeed,rb.linearVelocity.y,moveDir.z * playerSpeed);
        }

        if (Input.GetKey(KeyCode.LeftShift)) //Player runs
        {
            playerSpeed = 5f;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift)) //Player walks
        {
            playerSpeed = 3f;
        }

        //Oil, lamp and lavender related
        if (oil > 0)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                lightOn = !lightOn;
                lamp.enabled = lightOn;
                SetLight(true);
                SetDark(false);
                dark = false;
            }
        }
        else
        {
            lightOn = false;
            lamp.enabled = lightOn;
            SetLight(false);
            SetDark(true);
            oil = 0;

        }
        if (lightOn == true)
        {
            oil -= oilRate * Time.deltaTime;
            SetDark(false);
        } else
        {
            SetDark(true);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            if(playerInventory.oilCounter > 0)
            {
                RechargeLamp();
            }
        }
        if (oil == 0 || dark == true)
        {
            healthManager.SanidadRes();
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            if(playerInventory.lavenderCounter > 0)
            {
                UseLavender();
            }
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            enemyBehaviour.StartStun();
        }
    }

    private void OnCollisionEnter(Collision collision) //Revision con el inventario por las llaves de colores
    {
        DoorDesignator door = collision.gameObject.GetComponent<DoorDesignator>();
        if(door == null)
        {
            return;
        }
        for(int i = 0; i < inventory.pickablesList.Count; i++)
        {
            InteractuableDesignator key = inventory.pickablesList[i];
            if(key.Designation == door.requiredKey)
            {
                door.OpenDoor();
                inventory.RemovePickable(key);
                return;
            }
        }
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

    void RechargeLamp()
    {
        oil = maxOil;
        playerInventory.oilCounter--;
    }

    private void SetLight(bool LightOn)
    {
        if(safeLight == null)
        {
            return;
        }
        safeLight.gameObject.SetActive(LightOn);
    }

    private void SetDark(bool DarkOn)
    {
        if(darkness == null)
        {
            return;
        }
        darkness.gameObject.SetActive(DarkOn);
    }

    void UseLavender()
    {
        healthManager.sanidad += 5; //Falta definir la cantidad
        playerInventory.lavenderCounter--;
    }

    private void ShineLight()
    {
        
    }
}
