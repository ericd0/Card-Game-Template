using UnityEngine;
using UnityEngine.SceneManagement; // Add this for scene management

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public GameObject player;
    public float smoothSpeed = 5f;
    public Vector3 offset = new Vector3(0, 0, -10);  // Z offset for 2D camera

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FindPlayer();
    }

    void Start()
    {
        FindPlayer();
    }

    void FindPlayer()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        target = player.transform;
    }

    private void LateUpdate()
    {
        if (target == null)
            return;

        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;
    }
}