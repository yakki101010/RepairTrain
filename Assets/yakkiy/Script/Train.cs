using UnityEngine;

public class Train : MonoBehaviour
{
    public static Train Instance;
    private void Awake()
    {
        Singleton();
    }

    void Singleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }



}
