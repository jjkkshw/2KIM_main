using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    // 게임 시작
    public void StartGame()
    {
        SceneManager.LoadScene("Game");
    }

    // 게임 종료
    public void QuitGame()
    {
        Debug.Log("게임 종료");

        Application.Quit();
    }
}