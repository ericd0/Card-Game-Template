using UnityEngine;

public class Firebolt : Projectile
{
    protected override void OnStart()
    {
        float angle = transform.rotation.eulerAngles.z * Mathf.Deg2Rad;
        direction = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f);
    }

    protected override void OnUpdate()
    {
        transform.position += direction * speed * Time.deltaTime;
    }
}