using UnityEngine;

public class Intensify : BuffEffect
{
    public float damageMultiplier = 1.5f;
    public float speedMultiplier = 1.2f;
    public float sizeMultiplier = 1.5f;

    public override void Apply(GameObject target)
    {
        GameManager.OnProjectileCast += OnProjectileCast;
        Debug.Log("Intensify buff active for next projectile!");
    }

    private void OnProjectileCast(Projectile projectile)
    {
        // Apply buffs directly to the new projectile
        projectile.damage *= damageMultiplier;
        projectile.velocity *= speedMultiplier;
        projectile.transform.localScale *= sizeMultiplier;
        
        Debug.Log("Projectile intensified!");

        // Remove buff after one use
        GameManager.OnProjectileCast -= OnProjectileCast;
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        GameManager.OnProjectileCast -= OnProjectileCast;
    }
}