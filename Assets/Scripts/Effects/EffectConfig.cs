using UnityEngine;

[CreateAssetMenu(fileName = "New Effect Config", menuName = "Effects/Effect Configuration")]
public class EffectConfig : ScriptableObject
{
    [Header("Duration Settings")]
    [SerializeField] private float baseDuration = 3f;
    [SerializeField] private float stackDuration = 1f;

    [Header("Stack Settings")]
    [SerializeField] private bool canStack = false;
    [SerializeField] private bool canApplyMultiple = false;

    [Header("Stat Modifiers")]
    [Tooltip("Generic modifiers that effects can use (e.g. damage, speed, size)")]
    [SerializeField] private float[] statModifiers = new float[0];

    // Public properties
    public float BaseDuration => baseDuration;
    public float StackDuration => stackDuration;
    public bool CanStack => canStack;
    public bool CanApplyMultiple => canApplyMultiple;
    public float[] StatModifiers => statModifiers;
}