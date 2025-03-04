using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    public float velocity = 0f;
    public float lifespan = 5f;
    public float damage;
    protected Vector3 direction;

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
}