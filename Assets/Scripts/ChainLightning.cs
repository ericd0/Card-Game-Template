using UnityEngine;
using System.Collections.Generic;
//very wip
//TODO: actually prefab and then test
public class ChainLightning : Projectile
{
    public int chains = 3;
    public float range = 8f;
    private GameObject currentTarget;
    private static HashSet<GameObject> chainHitEnemies = new HashSet<GameObject>();
    private bool hasChained = false;
    protected override void OnStart()
    {
        lifespan = 0.5f;
        
        // Find closest enemy and chain immediately if found
        FindNextTarget();
        if (currentTarget != null && !hasChained)
        {
            Chain();
        }

        // Calculate direction to target
        if (currentTarget != null)
        {
            direction = (currentTarget.transform.position - transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
        else
        {
            float angle = transform.rotation.eulerAngles.z * Mathf.Deg2Rad;
            direction = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f);
        }
    }
    protected override void OnUpdate()
    {
        if (lifespan <= 0f)
        {
            Destroy(gameObject);
        }
        lifespan -= Time.deltaTime;
    }
    private void Chain()
    {
        if (chains > 0 && currentTarget != null && !hasChained)
        {
            hasChained = true;
            
            // Calculate spawn position in front of the current target
            Vector3 directionToTarget = (currentTarget.transform.position - transform.position).normalized;
            Vector3 spawnPosition = currentTarget.transform.position + directionToTarget * 0.5f;
            
            GameObject nextChain = Instantiate(gameObject, spawnPosition, transform.rotation);
            ChainLightning nextLightning = nextChain.GetComponent<ChainLightning>();
            if (nextLightning != null)
            {
                nextLightning.chains = chains - 1;
                nextLightning.damage = damage;
                nextLightning.velocity = velocity;
                nextLightning.range = range;
                nextLightning.lifespan = lifespan;
            }
        }
    }
    protected override bool CanHitEnemy(GameObject enemy)
    {
        return !chainHitEnemies.Contains(enemy);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") && CanHitEnemy(other.gameObject))
        {
            chainHitEnemies.Add(other.gameObject);
            FindNextTarget();
            if (currentTarget != null && !hasChained)
            {
                Chain();
            }
        }
    }
    void FindNextTarget()
    {
        float closestDistance = range;
        currentTarget = null;
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        
        foreach (GameObject enemy in enemies)
        {
            if (chainHitEnemies.Contains(enemy)) continue;
            
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                currentTarget = enemy;
            }
        }
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}