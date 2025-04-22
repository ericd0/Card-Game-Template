using System.Buffers.Text;
using UnityEngine;
//TODO: some structural changes to the deck and player might be needed to have the shuffle speed and cast speed not be a total mess
public class Haste : Effect
{
    public float castSpeedMultiplier = 0.9f;
    //public float shuffleSpeedMultiplier = 1.2f; adding this might be harder than it looks, leave it for now
    public float moveSpeedMultiplier = 1.15f;
    protected override void Start()
    {
        duration = 3f;
        base.Start();
    }
    protected override void OnEffectStart()
    {
        //targetBody.moveSpeed *= moveSpeedMultiplier;
        GameManager.OnProjectileCast += OnProjectileCast;

    }

    protected override void OnEffectEnd()
    {
        //GameManager.OnProjectileCast -= OnProjectileCast;
        targetBody.moveSpeed /= moveSpeedMultiplier;
    }

    private void OnProjectileCast(Projectile projectile)
    {
        //not yet
        //projectile.castspeed *= castSpeedMultiplier;
    }

    private void OnDestroy()
    {
        //GameManager.OnProjectileCast -= OnProjectileCast;
    }
}
