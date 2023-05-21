using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class MenuController : MonoBehaviour
{
    public Button buttonPlay;
    public Button buttonExit;
    // Start is called before the first frame update
    void Start()
    {
        buttonPlay.onClick.AddListener(PlayGame);
        buttonExit.onClick.AddListener(ExitGame);
    }

    private void ExitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    private void PlayGame()
    {
        SceneManager.LoadScene("PreGame");
    }

    

    // Update is called once per frame
    void Update()
    {
        
    }
}
