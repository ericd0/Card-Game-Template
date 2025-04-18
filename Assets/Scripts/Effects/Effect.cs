using UnityEngine;

public abstract class Effect : MonoBehaviour 
{
    protected Body targetBody;
    [SerializeField] // Add this attribute to ensure Unity serializes the field
    protected float duration = 0f; // Add default value
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

        remainingDuration = duration;
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
