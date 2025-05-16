using UnityEngine;
using UnityEngine.PlayerLoop;

public class Burning : Effect
{
    private int burnTick = 0;
    private int damageTick = 45; //the tick that damage is applied
    private float initialHit; //Damage of the hit that caused the burn
    private float burnDamage;

    public override void Initialize(float damage)
    {
        initialHit = damage;
    }

    protected override void Start()
    {
        base.Start();
        burnDamage = initialHit * 0.1f;
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void OnEffectTick()
    {
        if(burnTick == damageTick)
        {
            targetBody.TakeDamage(burnDamage); // Apply burn damage
            burnTick = 0; // Reset tick after applying damage
        }
        else
        {
            burnTick+= 1;
        }
    }
}
