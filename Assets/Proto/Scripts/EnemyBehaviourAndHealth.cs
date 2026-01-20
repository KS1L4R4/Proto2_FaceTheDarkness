using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviourAndHealth : MonoBehaviour
{
    public Transform player;
    public NavMeshAgent navMesh;
    public int enemyHealth;
    public int enemyDamage;

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
            Destroy(gameObject);
        }
    }

    void AttackPlayer()
    {
        Debug.Log("Enemy has attacked player");
    }
}
