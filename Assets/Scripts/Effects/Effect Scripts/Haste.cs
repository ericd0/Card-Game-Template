using System.Buffers.Text;
using UnityEngine;
//TODO: 
public class Haste : Effect
{
    //public float castSpeedMultiplier = 1.25f;
    //public float shuffleSpeedMultiplier = 1.2f; adding this might be harder than it looks, leave it for now
    public float moveSpeedMultiplier = 1.15f;
    protected override void Start()
    {
        duration = 3f;
        base.Start();
    }
    protected override void OnEffectStart()
    {
        targetBody.moveSpeed *= moveSpeedMultiplier;
        //GameManager.OnProjectileCast += OnProjectileCast;
    }

    protected override void OnEffectEnd()
    {
        //GameManager.OnProjectileCast -= OnProjectileCast;
        targetBody.moveSpeed /= moveSpeedMultiplier;
    }

    private void OnProjectileCast(Projectile projectile)
    {

    }

    private void OnDestroy()
    {
        //GameManager.OnProjectileCast -= OnProjectileCast;
    }
}
