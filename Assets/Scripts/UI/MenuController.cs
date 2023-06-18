using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public void PlayGame()
    {
        FindObjectOfType<AudioManager>().Play("Button Knob");
        SceneManager.LoadScene("SelectLevel");
    }

    public void ExitGame()
    {
        FindObjectOfType<AudioManager>().Play("Button Knob");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
