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
    private float enemySpeed;
    AudioSource shout;


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
        shout = GetComponent<AudioSource>();
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
        if(uIManager.pause == true || stunned == true)
        {
            navMesh.speed = 0f;
        }
        else
        {
            navMesh.speed = enemySpeed;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            shout.Play();
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            shout.Stop();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag(targetTag))
        {
            playerCaught = true;
            uIManager.ShowDefeatScreen();
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
                    playerSpotted = true;
                    enemyAnimator.SetBool("IsChasing", true);
                }
                else
                {
                    enemySpeed = 6f;
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

    IEnumerator enemyStun()
    {
        enemyAnimator.SetBool("IsStunned", true);
        stunned = true;
        navMesh.speed = 0f;
        yield return new WaitForSeconds(5);
        enemyAnimator.SetBool("IsStunned", false);
        stunned = false;
    }
}
