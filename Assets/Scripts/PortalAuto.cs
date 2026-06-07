using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalAuto : MonoBehaviour
{
    public string sceneToLoad; 
    public string portalName; 

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameState.CameFromIntro = true; 

            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
