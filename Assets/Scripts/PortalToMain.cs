using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalToMain : MonoBehaviour
{
    public string sceneToLoad = "MainScene";
    public bool markAsIntroExit = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (markAsIntroExit)
                GameState.CameFromIntro = true;

            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
