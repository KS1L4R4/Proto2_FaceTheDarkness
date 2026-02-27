using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public class EnemyBehaviourAndHealth : MonoBehaviour
{
    private NavMeshAgent navMesh;
    private UIManager uIManager;
    private Animator enemyAnimator;
    private Vector3 startingPosition;
    private string targetTag = "Player";
    private bool playerCaught = false;
    private bool playerSpotted = false;
    private bool stunned = false;
    private float maxWanderingTries = 5f;
    private float wanderingRadius = 100f;
    private float enemySpeed;


    public Transform targetPlayer;
    public int rayCount;
    public float viewDistance;
    public float viewAngle;

    void Start()
    {
        navMesh = GetComponent<NavMeshAgent>();
        enemyAnimator = GetComponent<Animator>();
        uIManager = FindAnyObjectByType<UIManager>();
        enemySpeed = 2f;
        navMesh.speed = enemySpeed;
    }

    void FixedUpdate()
    {
        if(playerCaught == false && stunned == false)
        {
            LookForPlayer();
        }
    }

    private void Update()
    {
    	ChasePlayer();
        SetAnimation();
        if(uIManager.pause == true)
        {
            navMesh.speed = 0f;
        }
        else
        {
            navMesh.speed = enemySpeed;
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
                    enemySpeed = 3.5f;
                    playerSpotted = true;
                }
                else
                {
                    enemySpeed = 2f;
                    playerSpotted = false;
                }
            }
            else
            {
                Debug.DrawRay(transform.position, rayDirection * viewDistance, Color.green);
            }
        }
    }

    public void StartStun()
    {
        StartCoroutine(enemyStun());
    }

    private void SetAnimation()
    {
        if (playerSpotted == true)
        {
            enemyAnimator.SetBool("IsChasing", true);
        }
        else
        {
            enemyAnimator.SetBool("IsChasing", false);
        }
    }
    IEnumerator enemyStun()
    {
        stunned = true;
        navMesh.speed = 0f;
        Debug.Log("Enemy is Stunned");
        yield return new WaitForSeconds(5);
        stunned = false;
        navMesh.speed = 2f;
        Debug.Log("Enemy hgas recovered");
    }
}
