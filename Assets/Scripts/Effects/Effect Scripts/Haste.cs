using UnityEngine;
public class Haste : Effect
{
    public float castSpeedBonus = 0.2f;
    public float shuffleSpeedBonus = 0.2f;
    public float moveSpeedBonus = .2f;
    protected override void Start()
    {
        duration = 3f;
        base.Start();
    }
    protected override void OnEffectStart()
    {
        // Check if targetBody exists first
        if (targetBody == null) return;

        // Store original values and apply new ones if properties exist
        var player = targetBody as Player;
        if (player != null)
        {
            player.moveSpeedMultiplier += moveSpeedBonus;
            player.shuffleSpeedMultiplier += shuffleSpeedBonus;
            player.castSpeedMultiplier += castSpeedBonus;
            player.SetStats();
        }
        else 
        {
            targetBody.moveSpeedMultiplier += moveSpeedBonus;
            targetBody.SetStats();
        }
    }

    protected override void OnEffectEnd()
    {
        if (targetBody == null) return;
        var player = targetBody as Player;
        if (player != null)
        {
            player.moveSpeedMultiplier -= moveSpeedBonus;
            player.shuffleSpeedMultiplier -= shuffleSpeedBonus;
            player.castSpeedMultiplier -= castSpeedBonus;
            player.SetStats();
        }
        else
        {
            targetBody.moveSpeedMultiplier -= moveSpeedBonus;
            targetBody.SetStats();
        }
    }
    private void OnDestroy()
    {

    }
}
