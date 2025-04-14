using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Firebolt : Projectile
{
    private float initialDamage;
    private float initialVelocity;
    public float shrinkRate = 0.1f;
    public float shrinkInterval = 0.1f;
    private float shrinkTimer;

    protected override void OnStart()
    {
        float angle = transform.rotation.eulerAngles.z * Mathf.Deg2Rad;
        direction = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f);
        initialDamage = damage;
        initialVelocity = velocity;
        shrinkTimer = shrinkInterval;
        ScaleStats();
    }

    protected override void OnUpdate()
    {
        transform.position += direction * velocity * Time.deltaTime;
        
        // Handle shrinking over time
        shrinkTimer -= Time.deltaTime;
        if (shrinkTimer <= 0)
        {
            Shrink();
            shrinkTimer = shrinkInterval;
            ScaleStats();
        }
    }

    private void Shrink()
    {
        transform.localScale *= (1f - shrinkRate);
        if (transform.localScale.x < 0.15f)
        {
            Destroy(gameObject);
        }
    }
    private void ScaleStats()
    {
        damage = initialDamage * transform.localScale.x;
        velocity = initialVelocity * transform.localScale.x *0.87f;
    }
}