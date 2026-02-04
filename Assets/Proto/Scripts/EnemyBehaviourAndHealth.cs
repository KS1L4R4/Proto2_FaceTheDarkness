using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviourAndHealth : MonoBehaviour
{
    //Elementos de la escena
    public Transform player;
    public NavMeshAgent navMesh;

    //Enteros
    public int enemyHealth;
    public int enemyDamage;

    //Flotantes
    private float attackCooldown;
    private float attackTimer;

    void Start()
    {
        player = GameObject.Find("Player").transform;
        navMesh = GetComponent<NavMeshAgent>();
        enemyHealth = 3;
        enemyDamage = 1;
        attackCooldown = 0;
        attackTimer = 0;
    }

    void Update()
    {
        navMesh.SetDestination(player.position);    
        if (IsPlayerInHarmRange())
        {
            AttackPlayer();
        }
    }

    bool IsPlayerInHarmRange()
    {
        return !navMesh.pathPending && navMesh.remainingDistance <= navMesh.stoppingDistance;
    }

    public void HarmEnemy()
    {
        enemyHealth--;
        if(enemyHealth <= 0)
        {
            //Destroy(gameObject);
            gameObject.SetActive(false);
        }
    }

    void AttackPlayer()
    {
        Debug.Log("Enemy has attacked player");
    }
}
