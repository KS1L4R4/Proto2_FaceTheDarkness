using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;


public class PlayerController : MonoBehaviour
{
    //Elementos de la escena
    private PlayerInventory playerInventory;
    public EnemyBehaviourAndHealth enemyBehaviour;
    public Rigidbody rb;
    public HealthManager healthManager;
    public Light lamp;
    public Light stunLight;
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
    public int rayCount;
    public int shineAngle;
    public int shineDistance;
    public float baseIntensity;
    public float baseIntStun;

    private string enemyTag = "Enemy";
    public Animator animator;

    private UIManager uimanager;

    private void Awake()
    {
        fxVolume.profile.TryGet(out vignette);
        uimanager = FindAnyObjectByType<UIManager>();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerInventory = GetComponent<PlayerInventory>();
        playerSpeed = 3f;
        playerRotation = 0.1f;
        loadingTime = 1f;
        maxOil = 100f;
        oil = maxOil;
        oilRate = 1f;
        lightOn = false;
        lamp.enabled = false;
        vignette.intensity.value = 0.2f;
        baseIntensity = lamp.intensity;
        baseIntStun = stunLight.intensity;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
    }

    void Update()
    {
        //healthManager.SanidadRes();
        if (uimanager.pause != true)
        {
            Vector3 moveVector = new Vector3(Input.GetAxisRaw("Horizontal"), rb.linearVelocity.y, Input.GetAxisRaw("Vertical"));
            Vector3 velocity = new Vector3(moveVector.x * playerSpeed, rb.linearVelocity.y, moveVector.z * playerSpeed);

            bool isMoving = moveVector.x != 0 || moveVector.z != 0;
            bool isRunning = isMoving && Input.GetKey(KeyCode.LeftShift);

            animator.SetBool("Walk", isMoving && !isRunning);
            animator.SetBool("Run", isRunning);

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
                rb.linearVelocity = new Vector3(moveDir.x * playerSpeed, rb.linearVelocity.y, moveDir.z * playerSpeed);
            }
        }
        else
        {
            playerSpeed = 3f;
        }

        //Vector3 moveVector = new Vector3(Input.GetAxisRaw("Horizontal"),0, Input.GetAxisRaw("Vertical")).normalized;
        //rb.linearVelocity = Vector3.up * Physics.gravity.y;

        if (Input.GetKey(KeyCode.LeftShift))//Player runs
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
            if(uimanager.pause != true)
            {
                oil -= oilRate * Time.deltaTime;
            }
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
        if (lightOn == false && dark == true)
        {
            if(uimanager.pause != true)
            {
                healthManager.SanidadRes();
            }
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
            ShineLampLight(500, 0.5f);
            ShineStunLight(5000f, 0.5f);
            ShineLamp();
        }
    }

    private void OnCollisionEnter(Collision collision) //Revision con el inventario por las llaves de colores
    {
        DoorDesignator door = collision.gameObject.GetComponent<DoorDesignator>();
        if(door == null)
        {
            return;
        }
        for(int i = 0; i < playerInventory.pickablesList.Count; i++)
        {
            InteractuableDesignator key = playerInventory.pickablesList[i];
            if(key.Designation == door.requiredKey)
            {
                door.OpenDoor();
                playerInventory.RemovePickable(key);
                return;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Exit"))
        {
            uimanager.ShowVictoryScreen();
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
        uimanager.UpdateOilIconUI();
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
        healthManager.sanity += 5; //Falta definir la cantidad
        playerInventory.lavenderCounter--;
        uimanager.UpdateLavenderIconUI();
    }

    private void ShineLamp()
    {
        for (int i = 0; i < rayCount; i++)
        {
            float rotationAngle = -(shineAngle / 2) + (shineAngle / (rayCount - 1)) * i;
            Quaternion rotation = Quaternion.AngleAxis(rotationAngle, Vector3.up);
            Vector3 rayDirection = rotation * transform.forward;
            RaycastHit hit;
            if (Physics.Raycast(transform.position, rayDirection, out hit, shineDistance))
            {
                if (hit.collider.CompareTag(enemyTag))
                {
                    Debug.DrawRay(transform.position, rayDirection * hit.distance, Color.red);
                    enemyBehaviour.StartStun();
                }
            }
            else
            {
                Debug.DrawRay(transform.position, rayDirection * shineDistance, Color.green);
            }
        }
    }

    private void ShineLampLight(float pulseIntensity, float duration)
    {
        lamp.DOKill(); // prevent stacking tweens

        Sequence s = DOTween.Sequence();
        s.Append(lamp.DOIntensity(pulseIntensity, duration));
        s.Append(lamp.DOIntensity(baseIntensity, duration));
    }

    private void ShineStunLight(float pulseIntensity, float duration)
    {
        stunLight.DOKill(); // prevent stacking tweens

        Sequence s = DOTween.Sequence();
        s.Append(stunLight.DOIntensity(pulseIntensity, duration));
        s.Append(stunLight.DOIntensity(baseIntStun, duration));
    }
}
