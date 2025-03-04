using UnityEngine;
//very wip
//TODO: actually make art and prefab and then test
public class ChainLightning : Projectile
{
    public int chains = 3;
    public float range = 5f;
    private GameObject currentTarget;

    protected override void OnStart()
    {
        // Find closest enemy if no target
        if (currentTarget == null)
        {
            FindNextTarget();
        }

        // Calculate direction to target
        if (currentTarget != null)
        {
            direction = (currentTarget.transform.position - transform.position).normalized;
        }
        else
        {
            // Default direction if no target (forward)
            float angle = transform.rotation.eulerAngles.z * Mathf.Deg2Rad;
            direction = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f);
        }
    }

    protected override void OnUpdate()
    {
        transform.position += direction * velocity * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") && chains > 0)
        {
            // Spawn next chain
            GameObject nextChain = Instantiate(gameObject, transform.position, Quaternion.identity);
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
        Destroy(gameObject);
    }

    void FindNextTarget()
    {
        float closestDistance = range;
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        
        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                currentTarget = enemy;
            }
        }
    }
}