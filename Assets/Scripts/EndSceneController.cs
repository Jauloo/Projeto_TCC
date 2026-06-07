using UnityEngine;
using UnityEngine.SceneManagement;

public class EndSceneController : MonoBehaviour
{
    public void VoltarAoMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }
}
