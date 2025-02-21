using UnityEngine;

public class Firebolt : MonoBehaviour
{
    public float speed = 10f;
    public float lifespan = 5f;
    private Vector3 direction;

    void Start()
    {
        // Get initial direction from rotation
        float angle = transform.rotation.eulerAngles.z * Mathf.Deg2Rad;
        direction = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f);
        
        // Destroy after lifespan
        Destroy(gameObject, lifespan);
    }

    void Update()
    {
        // Move forward
        transform.position += direction * speed * Time.deltaTime;
    }
}