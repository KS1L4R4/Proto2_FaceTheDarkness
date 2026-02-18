using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviourAndHealth : MonoBehaviour
{
	//Elementos de la escena
	public Transform player;
	public NavMeshAgent navMesh;

	private Vector3 startPosition; //la posición inicial del enemigo
	private Vector3 currentDestination; //el destino que seguirá en el nav mesh

	public bool aggro; // determinar si el enemigo es agresivo o no.
	public bool sawPlayer;

	//Enteros
	public int enemyHealth;
	public int enemyDamage;

	//Flotantes
	private float attackCooldown;
	private float attackTimer;
	private float wanderPointsMaxTries = 5; //número de intentos máximos para obtener un punto aleatorio
	public float wanderRadius = 15; // radio en el que buscará un punto aletorio
	public float viewDistance = 15; 
	public float viewAngle = 45;

	void Start()
	{
		//player = GameObject.Find("Player").transform;
		navMesh = GetComponent<NavMeshAgent>();
		enemyHealth = 3;
		enemyDamage = 1;
		attackCooldown = 0;
		attackTimer = 0;
		startPosition = transform.position;

		UpdateDestination();
	}

	void Update()
	{
		//navMesh.SetDestination(player.position);
		if (!aggro)
		{
			sawPlayer = Vision();
		}

		if (!navMesh.pathPending && navMesh.remainingDistance <= navMesh.stoppingDistance)
		{
			UpdateDestination();
		}

		if (IsPlayerInHarmRange() && aggro)
		{
			AttackPlayer();
		}
	}

	private bool Vision()
	{
		// Solo ejecutar resto de la función si el jugador está en el rango
		if (Vector3.Distance(transform.position, player.position) > viewDistance)
		{
			return false;
		}

		Vector3 directionToTarget = (player.position - transform.position).normalized;
		Vector3 ClampeddirectionToTarget = new Vector3(directionToTarget.x, 0, directionToTarget.z); //eliminar la altura, el eje y, interfiere con el cálculo de ángulo.
		float angle = Vector3.Angle(transform.forward, ClampeddirectionToTarget); //obtener el ángulo entre el jugador y el enemigo
		Debug.Log("Looking");

		// Si el ángulo es menor o igual al ángulo de visión, disparar un rayo al jugador, si este le pega, es decir no hay algo físicamente los estorbe, entonces el enemigo "ve"
		// al jugador.
		if (angle <= viewAngle)
		{
			Debug.Log("Detected");
			Ray visionRay = new Ray(transform.position, directionToTarget);
			RaycastHit hit;

			Debug.DrawRay(transform.position, directionToTarget * viewDistance, Color.cyan);

			if (Physics.Raycast(visionRay, out hit, viewDistance))
			{

				Debug.Log($"VisionRay hitted {hit.collider.name}");
				PlayerController player = hit.collider.GetComponent<PlayerController>();

				if (player != null)
				{
					Debug.Log("Found");
					aggro = true;
					UpdateDestination();

					return true;
				}
			}
		}

		return false;
	}

	bool IsPlayerInHarmRange()
	{
		return !navMesh.pathPending && navMesh.remainingDistance <= navMesh.stoppingDistance;
	}

	public void HarmEnemy()
	{
		enemyHealth--;
		if (enemyHealth <= 0)
		{
			//Destroy(gameObject);
			gameObject.SetActive(false);
		}
	}

	void AttackPlayer()
	{
		Debug.Log("Enemy has attacked player");
	}

	/// <summary>
	/// Actualiza el punto de navegación al que se moverá el enemigo, si esta aggro será la posición del jugador.
	/// De lo contrario será un punto aleatorio.
	/// </summary>
	public void UpdateDestination()
	{
		if (aggro)
		{
			navMesh.SetDestination(player.position);
			return;
		}

		navMesh.SetDestination(GetWanderPoint());
	}

	private void OnTriggerEnter(Collider other)
	{
		PlayerController player = other.GetComponent<PlayerController>();
		if (player != null)
		{
			aggro = true;
			UpdateDestination();
		}
	}

	/// <summary>
	/// Obtiene un punto aleatorio al rededor del jugador especificado por su wanderRadius
	/// </summary>
	private Vector3 GetRandomPoint()
	{
		//crear una esfera con en radio específicado, esta siempre se crea en la posición Vector.zero del mudno
		Vector3 randomPoint = Random.insideUnitSphere * wanderRadius;

		//Para centrar la esfera a la posición del jugador, se le suma la posición de este
		randomPoint += transform.position;
		return randomPoint;
	}


	/// <summary>
	/// Obtiene un punto aleatorio al que el enemigo se moverá
	/// </summary>
	/// <returns>un punto alcanzable en el navMesh o la posición inicial si no lo encuentra</returns>
	private Vector3 GetWanderPoint()
	{
		NavMeshHit hit; //variable similar a Raycasthit, es para verificar si un punto está sobre on navMesh
		Vector3 finalPoint = startPosition; //variable que determinará la posición obtenida


		for (int i = 0; i < wanderPointsMaxTries; i++)
		{
			//Obtener un punto aleatorio al rededor del enemigo
			Vector3 randomPoint = GetRandomPoint();

			//SamplePosition verifica si el punto obtenido está dentro de un nav mesh
			if (NavMesh.SamplePosition(randomPoint, out hit, wanderRadius, 1))
			{
				finalPoint = hit.position;
				break; //si se obtiene un punto válido, detener el ciclo for.
			}

			//Si no se obtuvo un punto válido el for continúa intentando obtener otro. El número de intentos
			//dependerá del valor de wanderPointsMaxTries.
		}
		return finalPoint;
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(transform.position, viewDistance);

		Vector3 visionLine1 = Quaternion.AngleAxis(viewAngle, transform.up) * transform.forward * viewDistance;
		Vector3 visionLine2 = Quaternion.AngleAxis(-viewAngle, transform.up) * transform.forward * viewDistance;

		Gizmos.color = Color.red;
		Gizmos.DrawRay(transform.position, visionLine1);
		Gizmos.DrawRay(transform.position, visionLine2);
	}

}

