using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStarter : MonoBehaviour
{
    [SerializeField] GameObject gameManagerPrefab;

    public void GameStart()
    {
        Instantiate(gameManagerPrefab);
        Player.Instance.Loading(Player.Scene.Stage);
        SceneManager.LoadScene("Stage");
    }
}
