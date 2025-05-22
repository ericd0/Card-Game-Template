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

    // Public properties to access private fields
    public float BaseDuration => baseDuration;
    public float StackDuration => stackDuration;
    public bool CanStack => canStack;
    public bool CanApplyMultiple => canApplyMultiple;
}