using UnityEngine;

public class Intensify : Effect
{
    public float damageMultiplier = 1.5f;
    public float speedMultiplier = 1.2f;
    public float sizeMultiplier = 1.5f;

    protected override void OnEffectStart()
    {
        GameManager.OnProjectileCast += OnProjectileCast;
        Debug.Log("Intensify effect active for next projectile!");
    }

    protected override void OnEffectEnd()
    {
        GameManager.OnProjectileCast -= OnProjectileCast;
    }

    private void OnProjectileCast(Projectile projectile)
    {
        projectile.damage *= damageMultiplier;
        projectile.velocity *= speedMultiplier;
        projectile.transform.localScale *= sizeMultiplier;
        
        Debug.Log("Projectile intensified!");
        Destroy(this); // Remove effect after one use
    }

    private void OnDestroy()
    {
        GameManager.OnProjectileCast -= OnProjectileCast;
    }
}