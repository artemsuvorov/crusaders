using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Interface : MonoBehaviour
{
    [SerializeField]
    private Text resourcesText;

    public UnityEvent CreateWorkerButtonPressed;
    public UnityEvent CreateKnightButtonPressed;

    public UnityEvent CreateSawmillButtonPressed;
    public UnityEvent CreateQuarryButtonPressed;

    public void OnResourceIncreased(Resources resources)
    {
        resourcesText.text = "Resources\r\n" + resources.ToString();
    }

    public void OnCreateWorkerButtonPressed()
    {
        CreateWorkerButtonPressed?.Invoke();
    }

    public void OnCreateKnightButtonPressed()
    {
        CreateKnightButtonPressed?.Invoke();
    }

    public void OnCreateSawmillButtonPressed()
    {
        CreateSawmillButtonPressed?.Invoke();
    }

    public void OnCreateQuarryButtonPressed()
    {
        CreateQuarryButtonPressed?.Invoke();
    }

    public void OnResourceIncreaseButtonPressed(Resources resources)
    {
        resources.IncreaseResource(Resource.Wood, 100);
        resources.IncreaseResource(Resource.Stone, 100);
    }
}
