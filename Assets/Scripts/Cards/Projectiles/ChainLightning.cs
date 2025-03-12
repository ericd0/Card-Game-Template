using UnityEngine;
using System.Collections.Generic;

public class ChainLightning : Projectile
{
    public int chains = 4;
    public float range = 8f;
    public float lineWidth = 0.1f;
    public Color lightningColor = Color.cyan;
    
    private List<LineRenderer> lightningLines;
    private List<GameObject> targets;
    private static HashSet<GameObject> chainHitEnemies = new HashSet<GameObject>();
    private float currentLifespan;

        protected override void OnStart()
        {
            lightningLines = new List<LineRenderer>();
            Vector3 currentPos = transform.position;

            // Create and setup all chain segments
            for (int i = 0; i < chains; i++)
            {
                GameObject lineObj = new GameObject($"LightningLine_{i}");
                lineObj.transform.SetParent(transform);
                LineRenderer line = lineObj.AddComponent<LineRenderer>();
                SetupLineRenderer(line);
                
                GameObject target = FindNextTarget(currentPos, chainHitEnemies);
                if (target != null)
                {
                    Vector3 endPos = target.transform.position;
                    line.SetPosition(0, currentPos);
                    line.SetPosition(1, endPos);
                    chainHitEnemies.Add(target);
                    currentPos = endPos;
                }
                else
                {
                    // No target found - hide this line
                    line.SetPosition(0, Vector3.zero);
                    line.SetPosition(1, Vector3.zero);
                }
                
                lightningLines.Add(line);
            }
        }

    private void SetupLineRenderer(LineRenderer line)
    {
        line.startWidth = lineWidth;
        line.endWidth = lineWidth;
        line.material = new Material(Shader.Find("Sprites/Default"));
        line.startColor = lightningColor;
        line.endColor = lightningColor;
        line.positionCount = 2;
    }

    protected override void OnUpdate()
    {
        // Only handle destruction
        if (lifespan <= 0)
        {
            Destroy(gameObject);
        }
    }
    private void OnDestroy()
    {
        chainHitEnemies.Clear();
    }
    private GameObject FindNextTarget(Vector3 position, HashSet<GameObject> hitEnemies)
    {
        float closestDistance = range;
        GameObject nextTarget = null;
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        
        foreach (GameObject enemy in enemies)
        {
            if (hitEnemies.Contains(enemy)) continue;
            
            float distance = Vector3.Distance(position, enemy.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                nextTarget = enemy;
            }
        }
        
        return nextTarget;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, range);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy") && !chainHitEnemies.Contains(other.gameObject))
        {
            chainHitEnemies.Add(other.gameObject);
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.health -= damage;
                if (enemy.health <= 0)
                {
                    Destroy(other.gameObject);
                }
            }
        }
    }
}