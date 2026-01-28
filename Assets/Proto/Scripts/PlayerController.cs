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
	Vignette vignette;
	public Transform camara;
	public float smoothtime = 5f;

	//Booleanos
	public bool isAiming;
	public bool isReloading;
	public bool lightOn;

	//Enteros
	public int bulletsLeft;
	public int maxBullets;

	//Flotantes
	public float playerSpeed;
	public float playerRotation, regularRotation, aimRotation;
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
		playerRotation = regularRotation;
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
		Movement();


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
			oil = Mathf.Clamp(oil, 0, maxOil);
		}

		//Testing
		if (Input.GetKeyDown(KeyCode.B))
		{
			oil = maxOil;
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		Pickable pickable = other.GetComponent<Pickable>();
		if (pickable != null)
		{
			pickable.PickupEffect();
			other.gameObject.SetActive(false);
		}

		//if (other.CompareTag("BatteryPack"))
		//{
		//	oil = maxOil;
		//	Destroy(other.gameObject);
		//}
	}



	private void OnCollisionEnter(Collision collision) //Revision con el inventario por las llaves de colores
	{
		Door door = collision.gameObject.GetComponent<Door>();

		if(door == null)
		{
			return;
		}

		for(int i = 0; i < inventory.keyList.Count; i++)
		{
			Key key = inventory.keyList[i];
			if (key.keyId == door.requiredKey)
			{
				door.OpenDoor();
				inventory.RemoveKey(key);
				return;
			}
		}
	}

	private void Movement()
	{
		Vector3 inputVector = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
		float cameraForward = Camera.main.transform.eulerAngles.y;

		//Ajustar el input a la dirección forward de la cámara para que los movimientos sean relativas a esta
		Vector3 relativeToCameraDirection = (Quaternion.Euler(0, cameraForward, 0) * inputVector).normalized;

		//Aplicar movimineto
		Vector3 moveVector = relativeToCameraDirection * playerSpeed;
		rb.linearVelocity = new Vector3(moveVector.x, rb.linearVelocity.y, moveVector.z);

		//Solo rotar si el jugador se está moviendo
		if(inputVector.sqrMagnitude > 0.1f)
		{
			Rotation(relativeToCameraDirection);
		}
	}

	private void Rotation(Vector3 direction)
	{
		Quaternion targetRotation = Quaternion.LookRotation(direction); //relativeToCameraDirection
		transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, playerRotation * Time.deltaTime);
	}

	private void Aiming()
	{
		isAiming = true;
		playerSpeed = 0f;
		playerRotation = aimRotation;
	}

	private void Unaim()
	{
		isAiming = false;
		playerSpeed = 3f;
		playerRotation = regularRotation;
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
