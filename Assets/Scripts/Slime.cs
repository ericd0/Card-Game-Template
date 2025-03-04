using Unity.VisualScripting;
using UnityEngine;

public class Slime : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float rotateSpeed = 2f;
    private Vector3 moveDirection;
    private GameObject player;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        moveDirection = Vector3.right;
    }

    void Update()
    {
        if (player != null)
        {
            Vector3 targetDirection = (player.transform.position - transform.position).normalized;
            moveDirection = Vector3.Lerp(moveDirection, targetDirection, rotateSpeed * Time.deltaTime);
            transform.position += moveDirection * moveSpeed * Time.deltaTime;
        }
    }
}