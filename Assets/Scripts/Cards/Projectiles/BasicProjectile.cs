using UnityEngine;

public class BasicProjectile : Projectile
{
    private SpriteRenderer spriteRenderer;

    public override void SetStats(Card_data cardData)
    {
        base.SetStats(cardData);
        
        // Set up sprite renderer if needed
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer == null)
            {
                spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
            }
        }

        // Set sprite and name from card data
        spriteRenderer.sprite = cardData.sprite;
        gameObject.name = cardData.card_name;

        // Set up collider based on sprite
        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        if (collider == null)
        {
            collider = gameObject.AddComponent<BoxCollider2D>();
        }
        collider.isTrigger = true;
        
        if (spriteRenderer.sprite != null)
        {
            // Size collider to match sprite bounds
            collider.size = spriteRenderer.sprite.bounds.size;
        }
    }

    protected override void OnStart()
    {
        float angle = transform.rotation.eulerAngles.z * Mathf.Deg2Rad;
        direction = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f);
    }

    protected override void OnUpdate()
    {
        transform.position += direction * velocity * Time.deltaTime;
    }
}