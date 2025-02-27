using Unity.VisualScripting;
using UnityEngine;

public class Slime : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float rotateSpeed = 2f;
    private Vector3 moveDirection;
    private GameObject player;
    public float health = 50f;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        moveDirection = Vector3.right;
    }

    void Update()
    {
        if (player != null)
        {
            Vector3 targetDirection = (player.transform.position - transform.position).normalized;
            moveDirection = Vector3.Lerp(moveDirection, targetDirection, rotateSpeed * Time.deltaTime);
            transform.position += moveDirection * moveSpeed * Time.deltaTime;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("PlayerProjectile"))
        {
            Projectile projectile = other.gameObject.GetComponent<Projectile>();
            if (projectile != null)
            {
                health -= projectile.damage;
                Debug.Log($"Slime took {projectile.damage} damage. Health: {health}");
                
                if (health <= 0)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}