using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class ChainLightning : Projectile
{
    public int chains = 4;
    public float range = 8f;
    public float lineWidth = 0.1f;
    public float chainDelay = 0.1f; // Add delay between chains
    public float chainOffset = 2f; // Add overshoot distance
    public Color lightningColor = Color.cyan;
    
    private List<LineRenderer> lightningLines;
    private List<GameObject> targets;
    private static HashSet<GameObject> chainHitEnemies = new HashSet<GameObject>();
    private float currentLifespan;

    protected override void OnStart()
    {
        lightningLines = new List<LineRenderer>();
        StartCoroutine(CreateChains());
    }

    private IEnumerator CreateChains()
    {
        Vector3 currentPos = transform.position;
        Vector3 currentDirection = transform.right; // Get initial direction

        for (int i = 0; i < chains; i++)
        {
            GameObject lineObj = new GameObject($"LightningLine_{i}");
            lineObj.transform.SetParent(transform);
            LineRenderer line = lineObj.AddComponent<LineRenderer>();
            SetupLineRenderer(line);
            
            GameObject target = FindNextTarget(currentPos, chainHitEnemies);
            if (target != null)
            {
                // Chain to enemy with overshoot
                Vector3 targetPos = target.transform.position;
                Vector3 direction = (targetPos - currentPos).normalized;
                Vector3 endPos = targetPos + (direction * chainOffset); // Overshoot by chainOffset units
                
                line.SetPosition(0, currentPos);
                line.SetPosition(1, endPos);
                chainHitEnemies.Add(target);
                currentPos = endPos;
                currentDirection = direction;
            }
            else
            {
                // Fire forward if no target found
                Vector3 endPos = currentPos + (currentDirection * range);
                line.SetPosition(0, currentPos);
                line.SetPosition(1, endPos);
                currentPos = endPos;
            }
            
            CreateColliderForLine(line, lineObj);
            lightningLines.Add(line);

            if (i < chains - 1) // Don't delay after the last chain
            {
                yield return new WaitForSeconds(chainDelay);
            }
        }
    }

    private void SetupLineRenderer(LineRenderer line)
    {
        line.startWidth = lineWidth * transform.localScale.x;
        line.endWidth = lineWidth * transform.localScale.x;
        line.material = new Material(Shader.Find("Sprites/Default"));
        line.startColor = lightningColor;
        line.endColor = lightningColor;
        line.positionCount = 2;
    }

    private void CreateColliderForLine(LineRenderer line, GameObject lineObj)
    {
        EdgeCollider2D edgeCollider = lineObj.AddComponent<EdgeCollider2D>();
        Vector3[] linePositions = new Vector3[2];
        line.GetPositions(linePositions);
        
        // Convert world positions to local positions for the edge collider
        Vector2[] colliderPoints = new Vector2[2];
        colliderPoints[0] = lineObj.transform.InverseTransformPoint(linePositions[0]);
        colliderPoints[1] = lineObj.transform.InverseTransformPoint(linePositions[1]);
        
        edgeCollider.points = colliderPoints;
        edgeCollider.isTrigger = true;
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
        Body[] bodies = FindObjectsByType<Body>(FindObjectsSortMode.None);
        
        foreach (Body targetBody in bodies)
        {
            // Skip if already hit or if same team
            if (hitEnemies.Contains(targetBody.gameObject) || targetBody.team == team || targetBody.team == 2)
                continue;
            
            float distance = Vector3.Distance(position, targetBody.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                nextTarget = targetBody.gameObject;
            }
        }
        
        return nextTarget;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}