using UnityEngine;
using UnityEngine.SceneManagement;

public class MissionSelectController : MonoBehaviour
{
    public void PlayAintab() => LoadScene("Aintab");
    public void PlayNicopolis() => LoadScene("Nicopolis");
    public void PlayHarim() => LoadScene("Harim");
    public void ExitToMenu() => LoadScene("Menu");

    private void LoadScene(string name)
    {
        FindObjectOfType<AudioManager>()?.Play("Button Knob");
        SceneManager.LoadScene(name);
    }
}
