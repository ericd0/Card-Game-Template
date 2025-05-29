using UnityEngine;
using System.Reflection;

public class BasicStatusEffect : Effect
{
    private BasicStatusEffectConfig Config => config as BasicStatusEffectConfig;

    protected override void OnEffectStart()
    {
        if (targetBody == null || Config == null) return;
        ApplyStatModifiers();
    }

    protected override void OnEffectEnd()
    {
        if (targetBody == null || Config == null) return;
        RemoveStatModifiers();
    }

    protected override void OnStackAdded()
    {
        ApplyStatModifiers();
        ResetDuration();
    }

    private void ApplyStatModifiers()
    {
        var player = targetBody as Player;

        for (int i = 0; i < Config.StatModifiersName.Length; i++)
        {
            string statName = Config.StatModifiersName[i].ToLower() + "Multiplier";
            float value = Config.StatModifiers[i] * stacks;

            // Try to find property in targetBody first
            var property = targetBody.GetType().GetField(statName, 
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            if (property != null)
            {
                float currentValue = (float)property.GetValue(targetBody);
                property.SetValue(targetBody, currentValue + value);
            }
            // If not found in targetBody, try player-specific properties
            else if (player != null)
            {
                property = player.GetType().GetField(statName,
                    BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                
                if (property != null)
                {
                    float currentValue = (float)property.GetValue(player);
                    property.SetValue(player, currentValue + value);
                }
            }
        }
        
        targetBody.SetStats();
    }

    private void RemoveStatModifiers()
    {
        var player = targetBody as Player;

        for (int i = 0; i < Config.StatModifiersName.Length; i++)
        {
            string statName = Config.StatModifiersName[i].ToLower() + "Multiplier";
            float value = Config.StatModifiers[i] * stacks;

            var property = targetBody.GetType().GetField(statName,
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            if (property != null)
            {
                float currentValue = (float)property.GetValue(targetBody);
                property.SetValue(targetBody, currentValue - value);
            }
            else if (player != null)
            {
                property = player.GetType().GetField(statName,
                    BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                
                if (property != null)
                {
                    float currentValue = (float)property.GetValue(player);
                    property.SetValue(player, currentValue - value);
                }
            }
        }
        
        targetBody.SetStats();
    }
}
