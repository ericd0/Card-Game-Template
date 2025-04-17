using UnityEngine;

public abstract class Effect : MonoBehaviour 
{
    protected Body targetBody;
    public float duration; // Duration in seconds, 0 for infinite
    protected float remainingDuration;

    protected virtual void Start()
    {
        targetBody = GetComponentInParent<Body>();
        if (targetBody == null)
        {
            Debug.LogError("Effect applied to object without Body component");
            Destroy(this);
            return;
        }
        remainingDuration = duration; // Duration is now in seconds directly
        OnEffectStart(); // Call start event when effect is initialized
    }

    protected virtual void Update()
    {
        if (duration > 0) // Only count down if not infinite
        {
            remainingDuration -= Time.deltaTime;
            Debug.Log($"Effect remaining duration: {remainingDuration}"); // Debug line
            
            if (remainingDuration <= 0)
            {
                OnEffectEnd();
                Destroy(this);
                return;
            }
        }
        
        OnEffectTick();
    }

    // Called when effect is first applied
    protected virtual void OnEffectStart() { }

    // Called every frame while effect is active
    protected virtual void OnEffectTick() { }

    // Called when effect ends naturally (not when forcefully removed)
    protected virtual void OnEffectEnd() { }

    public virtual void Initialize(float damage)
    {
        // Base implementation can be empty
    }
}
