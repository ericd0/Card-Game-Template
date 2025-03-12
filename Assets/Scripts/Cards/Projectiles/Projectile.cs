using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    public float velocity;
    public float lifespan;
    public float damage;
    protected Vector3 direction;

    void Start()
    {
        gameObject.layer = LayerMask.NameToLayer("Projectile");
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
        if (cardData is ProjectileCard_data projectileData)
        {
            damage = projectileData.damage;
            velocity = projectileData.velocity;
        }
        else
        {
            Debug.LogError($"Tried to set projectile stats with non-projectile card: {cardData.GetType()}");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            // Process the hit
            // ...existing collision code...
        }
    }
}