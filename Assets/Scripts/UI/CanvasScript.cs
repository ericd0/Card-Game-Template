using UnityEngine;

public class CanvasScript : MonoBehaviour
{
    public static CanvasScript cs;
    private void Awake()
    {
        if (cs != null && cs != this)
        {
            Destroy(gameObject);
        }
        else
        {
            cs = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }
}