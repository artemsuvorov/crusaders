using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace UI
{
    public class AfterGameController : MonoBehaviour
    {
        public Button buttonExit;
        public Button buttonAgain;
        
        void Start()
        {
            buttonExit.onClick.AddListener(() => GoMenu());
        }

        private void GoMenu()
        {
            FindObjectOfType<AudioManager>()?.Play("Button Knob");
            SceneManager.LoadScene("Menu");
        }
    }
}