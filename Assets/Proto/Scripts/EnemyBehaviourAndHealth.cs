using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviourAndHealth : MonoBehaviour
{
    private NavMeshAgent navMesh;
    private Vector3 startingPosition;
    private string targetTag = "Player";
    private bool playerCaught = false;
    private float maxWanderingTries = 5f;
    private float wanderingRadius = 100f;

    public Transform targetPlayer;
    public int rayCount;
    public float viewDistance;
    public float viewAngle;

    void Start()
    {
        navMesh = GetComponent<NavMeshAgent>();
    }

    void FixedUpdate()
    {
        if(playerCaught == false)
        {
            LookForPlayer();
            if (!navMesh.pathPending && navMesh.remainingDistance <= navMesh.stoppingDistance)
            {
		    	navMesh.SetDestination(GetWanderingPoint());
		    }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag(targetTag))
        {
            playerCaught = true;
        }
    }

    private void ChasePlayer()
    {
        if (targetPlayer != null)
        {
            navMesh.SetDestination(targetPlayer.position);
        }
    }

    private void LookForPlayer()
    {
        for (int i = 0; i < rayCount; i++)
        {
            float rotationAngle = -(viewAngle / 2) + (viewAngle / (rayCount - 1)) * i;
            Quaternion rotation = Quaternion.AngleAxis(rotationAngle, Vector3.up);
            Vector3 rayDirection = rotation * transform.forward;
            RaycastHit hit;
            if (Physics.Raycast(transform.position, rayDirection, out hit, viewDistance))
            {
                if (hit.collider.CompareTag(targetTag))
                {
                    Debug.DrawRay(transform.position, rayDirection * hit.distance, Color.red);
                    ChasePlayer();
                    navMesh.speed = 3.5f;
                }
            }
            else
            {
                Debug.DrawRay(transform.position, rayDirection * viewDistance, Color.green);
            }
        }
    }

    private Vector3 GetWanderingPoint()
    {
        navMesh.speed = 2f;
        NavMeshHit hit;
        Vector3 newDestination = startingPosition;
        for (int i = 0; i < maxWanderingTries; i++)
        {
            Vector3 randomPoint = SetRandomDestination();
            if(NavMesh.SamplePosition(randomPoint, out hit, wanderingRadius, 1))
            {
                newDestination = hit.position;
                break;
            }
        }
        return newDestination;
    }

    private Vector3 SetRandomDestination()
    {
        Vector3 randomPoint = Random.insideUnitSphere * wanderingRadius;
        randomPoint += transform.position;
        return randomPoint;
    }
}
