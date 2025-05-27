using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Effect : MonoBehaviour
{
    [SerializeField] protected EffectConfig config;
    protected Body targetBody;
    protected float duration;
    protected int stacks = 1;

    protected virtual void Awake()
    {
        if (config == null)
        {
            Debug.LogError($"Effect {GetType().Name} is missing configuration!");
            Destroy(this);
            return;
        }

        targetBody = GetComponentInParent<Body>();
        if (targetBody == null)
        {
            Debug.LogError("Effects cannot be applied to an object without a Body component!");
            Destroy(this);
            return;
        }

        // Try to find existing effect of the same type
        Effect existingEffect = targetBody.GetComponent(this.GetType()) as Effect;
        if (existingEffect != null && existingEffect != this)
        {
            if (existingEffect.config.CanStack)
            {
                existingEffect.AddStack();
                Destroy(this);
                return;
            }
            else if (!config.CanApplyMultiple)
            {
                existingEffect.ResetDuration();
                Destroy(this);
                return;
            }
        }
    }

    protected virtual void Start()
    {
        duration = config.BaseDuration;
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
        if (!config.CanStack)
        {
            return false;
        }
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
        duration = config.BaseDuration;
        print(duration);
    }
    protected virtual void StackDuration()
    {
        if (duration + config.StackDuration < config.BaseDuration)
        {
            duration = config.BaseDuration + config.StackDuration;
        }
        else
        {
            duration += config.StackDuration;
        }
    }
    public virtual void Initialize(float damage)
    {
        // Base implementation can be empty
    }

    public static bool TryStack<T>(GameObject target) where T : Effect
    {
        T existingEffect = target.GetComponent<T>();
        if (existingEffect != null && existingEffect.config.CanStack)
        {
            return existingEffect.AddStack();
        }
        return false;
    }

    public static T CreateEffect<T>(GameObject target, T prefab) where T : Effect
    {
        if (prefab == null)
        {
            Debug.LogError($"Prefab is null when trying to create effect of type {typeof(T)}");
            return null;
        }

        // Temporarily disable the target GameObject
        bool wasActive = target.activeSelf;
        target.SetActive(false);

        // Get the concrete type and create component
        System.Type effectType = prefab.GetType();
        T effect = (T)target.AddComponent(effectType);
        
        if (effect != null)
        {
            // Copy the config before enabling
            effect.config = prefab.config;
            
            // Re-enable and return
            target.SetActive(wasActive);
            return effect;
        }
        else
        {
            Debug.LogError($"Failed to create effect of type {effectType}");
            target.SetActive(wasActive);
            return null;
        }
    }
}
