using UnityEngine;

public class IceSlow : Effect
{
        private const int SPEED_REDUCTION_INDEX = 0;
        protected override void OnEffectStart()
        {
                if (targetBody == null) return;
                targetBody.moveSpeedMultiplier -= config.StatModifiers[SPEED_REDUCTION_INDEX];
                targetBody.SetStats();
        }
        protected override void OnEffectEnd()
        {
                if (targetBody == null) return;
                targetBody.moveSpeedMultiplier += config.StatModifiers[SPEED_REDUCTION_INDEX] * stacks;
                targetBody.SetStats();
        }
}