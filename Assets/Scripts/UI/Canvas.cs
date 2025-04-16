using UnityEngine;

public class Canvas : MonoBehaviour
{
    public static Canvas c;
    private void Awake()
    {
        if (c != null && c != this)
        {
            Destroy(gameObject);
        }
        else
        {
            c = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }
}
