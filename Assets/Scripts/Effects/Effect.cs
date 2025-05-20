using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Effect : MonoBehaviour 
{
    protected Body targetBody;
    [SerializeField] protected float baseDuration;
    protected float duration;
    [SerializeField]
    [Tooltip("Allows stacking of the effect. If false, effects are applied separately.")]
    protected bool canStack;
    [SerializeField]
    [Tooltip("This only matters if canStack is false. Allows applying multiple instances of the same effect. Otherwise, duration resets if the effect is applied again.")]
    protected bool canApplyMultiple;
    protected int stacks = 1;
    protected float stackDuration;
    protected virtual void Awake()
    {
        Debug.Log($"Awake - baseDuration: {baseDuration}");
        targetBody = GetComponentInParent<Body>();
        if (targetBody == null)
        {
            Debug.LogError("Effect applied to object without Body component");
            Destroy(this);
            return;
        }

        // Try to find existing effect of the same type
        Effect existingEffect = targetBody.GetComponent(this.GetType()) as Effect;
        if (existingEffect != null && existingEffect != this)
        {
            // If effect exists and can stack, add stack and destroy this instance
            if (existingEffect.canStack)
            {
                existingEffect.AddStack();
                Destroy(this);
                return;
            }
            else if(!canApplyMultiple)
            {
                existingEffect.ResetDuration();
                Destroy(this);
                return;
            }
        }
    }

    protected virtual void Start()
    {
        duration = baseDuration;
        print(baseDuration);
        OnEffectStart();
    }

    protected virtual void Update()
    {
        if (duration > 0)
        {
            duration -= Time.deltaTime;
            
            if (duration <= 0)
            {
                OnEffectEnd();
                Destroy(this);
                return;
            }
        }
        OnEffectTick();
    }

    public virtual bool AddStack()
    {
        if (!canStack) return false;
        
        stacks++;
        OnStackAdded();
        return true;
    }

    protected virtual void OnStackAdded()
    {
        // Override this to handle stack-specific logic
    }

    // Called when effect is first applied
    protected virtual void OnEffectStart() { }

    // Called every frame while effect is active
    protected virtual void OnEffectTick() { }

    // Called when effect ends naturally (not when forcefully removed)
    protected virtual void OnEffectEnd() 
    {
        stacks = 0;
    }
    protected virtual void ResetDuration()
    {
        Debug.Log($"ResetDuration - Before reset - baseDuration: {baseDuration}, duration: {duration}");
        duration = baseDuration;
        Debug.Log($"ResetDuration - After reset - baseDuration: {baseDuration}, duration: {duration}");
    }
    protected virtual void StackDuration()
    {
        if (duration + stackDuration < baseDuration)
        {
            duration = baseDuration + stackDuration;
        }
        else
        {
            duration += stackDuration;
        }
    }
    public virtual void Initialize(float damage)
    {
        // Base implementation can be empty
    }

    public static bool TryStack<T>(GameObject target) where T : Effect
    {
        T existingEffect = target.GetComponent<T>();
        if (existingEffect != null && existingEffect.canStack)
        {
            return existingEffect.AddStack();
        }
        return false;
    }
}
