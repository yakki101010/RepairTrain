using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance;

    [SerializeField] GameObject stageLoading;
    [SerializeField] GameObject stationLoading;

    public enum Scene
    {
        Stage,
        Station,
    }

    private void Awake()
    {
        Singleton();
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
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

    /// <summary>
    /// ローディングを表示
    /// </summary>
    /// <param name="scene">どのシーンへ行くローディングか？</param>
    public void Loading(Scene scene)
    {
        switch (scene)
        {
            case Scene.Stage:
                stageLoading.SetActive(true);
                break;

            case Scene.Station:
                stationLoading.SetActive(true);
                break;
        }
    }

    /// <summary>
    /// ロード画面を非表示
    /// </summary>
    public void EndLoading()
    {
        stageLoading.SetActive(false);
        stationLoading.SetActive(false);
    }

}
