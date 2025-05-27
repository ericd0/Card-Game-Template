using Unity.VisualScripting;
using UnityEngine;
public class Haste : Effect
{
    private const int CAST_SPEED_INDEX = 0;
    private const int SHUFFLE_SPEED_INDEX = 1;
    private const int MOVE_SPEED_INDEX = 2;

    protected override void Start()
    {
        base.Start();
    }
    protected override void OnEffectStart()
    {
        AddHaste();
    }

    protected override void OnEffectEnd()
    {
        if (targetBody == null) return;

        var player = targetBody as Player;
        if (player != null)
        {
            player.moveSpeedMultiplier -= config.StatModifiers[MOVE_SPEED_INDEX] * stacks;
            player.shuffleSpeedMultiplier -= config.StatModifiers[SHUFFLE_SPEED_INDEX] * stacks;
            player.castSpeedMultiplier -= config.StatModifiers[CAST_SPEED_INDEX] * stacks;
            player.SetStats();
            print("Haste effect ended, removing speed bonuses.");
        }
        else
        {
            targetBody.moveSpeedMultiplier -= config.StatModifiers[MOVE_SPEED_INDEX] * stacks;
            targetBody.SetStats();
        }
    }
    void AddHaste()
    {
        if (targetBody == null) return;

        var player = targetBody as Player;
        if (player != null)
        {
            player.moveSpeedMultiplier += config.StatModifiers[MOVE_SPEED_INDEX] * stacks;
            player.shuffleSpeedMultiplier += config.StatModifiers[SHUFFLE_SPEED_INDEX] * stacks;
            player.castSpeedMultiplier += config.StatModifiers[CAST_SPEED_INDEX] * stacks;
            player.SetStats();
        }
        else 
        {
            targetBody.moveSpeedMultiplier += config.StatModifiers[MOVE_SPEED_INDEX] * stacks;
            targetBody.SetStats();
        }
    }
    protected override void OnStackAdded()
    {
        AddHaste();
        ResetDuration();
    }

}
