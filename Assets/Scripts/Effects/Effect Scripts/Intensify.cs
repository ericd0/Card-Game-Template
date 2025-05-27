using UnityEngine;

public class Intensify : Effect
{
    // Using indices for clarity
    private const int DAMAGE_INDEX = 0;
    private const int SPEED_INDEX = 1;
    private const int SIZE_INDEX = 2;

    protected override void Start()
    {
        base.Start();
    }

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
        projectile.damage *= (config.StatModifiers[DAMAGE_INDEX] * stacks) + 1f;
        projectile.velocity *= (config.StatModifiers[SPEED_INDEX] * stacks) + 1f;
        projectile.transform.localScale *= (config.StatModifiers[SIZE_INDEX] * stacks) + 1f;
        Debug.Log("Projectile intensified!");
        Destroy(this); // Remove effect after one use
    }

    private void OnDestroy()
    {
        GameManager.OnProjectileCast -= OnProjectileCast;
    }
}