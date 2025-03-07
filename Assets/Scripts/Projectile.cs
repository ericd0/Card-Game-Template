using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    public float velocity;
    public float lifespan;
    public float damage;
    protected Vector3 direction;
    protected virtual bool CanHitEnemy(GameObject enemy)
    {
        return true;
    }

    void Start()
    {
        Destroy(gameObject, lifespan);
        OnStart();
    }

    void Update()
    {
        OnUpdate();
    }

    protected abstract void OnStart();
    protected abstract void OnUpdate();

    public void SetStats(Card_data cardData)
    {
        damage = cardData.damage;
        velocity = cardData.velocity;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") && CanHitEnemy(other.gameObject))
        {
            // Process the hit
            // ...existing collision code...
        }
    }
}