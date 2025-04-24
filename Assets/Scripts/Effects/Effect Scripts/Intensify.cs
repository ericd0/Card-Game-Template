using UnityEngine;

public class Intensify : Effect
{
    [Header("Intensify Effect Settings")]
    [Tooltip("Already accounts for stacks and initial amount.")]
    public float damageMultiplier;
    public float speedMultiplier;
    public float sizeMultiplier;

    protected override void Start()
    {
        base.Start();
        canStack = true;
        duration = 0f;
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
        projectile.damage *= (damageMultiplier * stacks)+1f;
        projectile.velocity *= (speedMultiplier * stacks)+1f;
        projectile.transform.localScale *= (sizeMultiplier *stacks)+1f;
        Debug.Log("Projectile intensified!");
        Destroy(this); // Remove effect after one use
    }

    private void OnDestroy()
    {
        GameManager.OnProjectileCast -= OnProjectileCast;
    }
}