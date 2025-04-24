using UnityEngine;

public abstract class Effect : MonoBehaviour 
{
    protected Body targetBody;
    [SerializeField] 
    protected float duration = 0f;
    protected float remainingDuration;
    [SerializeField]
    protected bool canStack;
    protected int stacks = 1;
    protected float stackDuration;
    protected virtual void Awake()
    {
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
        }
    }

    protected virtual void Start()
    {
        remainingDuration = duration;
        stackDuration = duration;
        OnEffectStart();
    }

    protected virtual void Update()
    {
        if (duration > 0)
        {
            remainingDuration -= Time.deltaTime;
            
            if (remainingDuration <= 0)
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
