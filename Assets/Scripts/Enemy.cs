using UnityEditor;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float health = 100f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("PlayerProjectile"))
        {
            Projectile projectile = other.gameObject.GetComponent<Projectile>();
            if (projectile != null)
            {
                health -= projectile.damage;
                if (health <= 0)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}