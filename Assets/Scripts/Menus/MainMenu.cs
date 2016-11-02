using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenuUI;
    public GameObject buttons;

    private void Start()
    {
        mainMenuUI.SetActive(false);
        buttons.SetActive(true);
    }

    public void Play()
    {
        SceneManager.LoadScene("Game");
    }

    public void Credit()
    {
        mainMenuUI.SetActive(true);
        buttons.SetActive(false);
    }

    public void CreditBack()
    {
        mainMenuUI.SetActive(false);
        buttons.SetActive(true);
    }

    public void Exit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}
