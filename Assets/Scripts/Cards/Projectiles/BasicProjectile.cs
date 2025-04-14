using Unity.VisualScripting;
using UnityEngine;

public class BasicProjectile : Projectile
{
    private SpriteRenderer spriteRenderer;

    private void EnsureSpriteRenderer()
    {
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer == null)
            {
                spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
                spriteRenderer.sortingOrder = 1;
            }
        }
    }

    protected override void OnStart()
    {
        EnsureSpriteRenderer();
        float angle = transform.rotation.eulerAngles.z * Mathf.Deg2Rad;
        direction = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f);
    }

    public override void SetStats(Card_data cardData)
    {
        if (cardData == null)
        {
            Debug.LogError("Card data is null");
            return;
        }

        base.SetStats(cardData);
        EnsureSpriteRenderer();
        spriteRenderer.sprite = cardData.sprite;
        spriteRenderer.sortingOrder = 1;
    }

    protected override void OnUpdate()
    {
        transform.position += direction * velocity * Time.deltaTime;
    }
}