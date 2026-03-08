using System.Collections;
using Unity.VisualScripting;
using UnityEditor.Analytics;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public class EnemyBehaviourAndHealth : MonoBehaviour
{
    private NavMeshAgent navMesh;
    private UIManager uIManager;
    private Animator enemyAnimator;
    private PlayerController playerController;
    //private Vector3 startingPosition;
    public Transform targetPlayer;
    public Transform returnalPoint;
    public Transform targetPosition;
    private AudioSource shout;
    [SerializeField] private Transform origin;
    private string targetTag = "Player";
    private bool playerCaught = false;
    //private bool playerSpotted = false;
    private bool stunned = false;
    private float enemySpeed;
    public bool shouldReturn;
    public int rayCount;
    public float viewDistance;
    public float viewAngle;

    void Start()
    {
        navMesh = GetComponent<NavMeshAgent>();
        enemyAnimator = GetComponent<Animator>();
        uIManager = FindAnyObjectByType<UIManager>();
        playerController = FindAnyObjectByType<PlayerController>();
        enemySpeed = 2f;
        navMesh.speed = enemySpeed;
        shout = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if(playerCaught == false && stunned == false)
        {
            LookForPlayer();
            if (playerController.dark == true)
            {
                targetPosition = targetPlayer;
            }
            else
            {
                targetPosition = returnalPoint;
            }
            ChasePlayer(targetPosition);
        }
        if(uIManager.pause == true || stunned == true)
        {
            navMesh.speed = 0f;
        }
        else
        {
            navMesh.speed = enemySpeed;
        }
        Debug.Log(targetPosition);
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

    private void ChasePlayer(Transform targetPosition)
    {
        if (targetPlayer != null)
        {
            navMesh.SetDestination(targetPosition.position);
        }
    }

    private void LookForPlayer()
    {
		if (Vector3.Distance(transform.position, targetPlayer.position) > viewDistance)
		{
            navMesh.speed = 4f;
            enemyAnimator.SetBool("IsChasing", true);
			return;
		}
        else
        {
            navMesh.speed = 5.3f;
            enemyAnimator.SetBool("IsChasing", false);
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
