using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public GameObject howToPanel;

    public void StartGame()
    {
        SceneManager.LoadScene("IntroScene");
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void ShowHowTo()
    {
        if (howToPanel != null) howToPanel.SetActive(true);
    }

    public void CloseHowTo()
    {
        if (howToPanel != null) howToPanel.SetActive(false);
    }
}
