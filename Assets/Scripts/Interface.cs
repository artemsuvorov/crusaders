using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Interface : MonoBehaviour
{
    [SerializeField]
    private Text resourcesText;

    public UnityEvent<GameObject> CreateEntityButtonPressed;

    public void OnResourceIncreased(Resources resources)
    {
        resourcesText.text = "Resources\r\n" + resources.ToString();
    }

    public void OnCreateEntityButtonPressed(GameObject entity)
    {
        CreateEntityButtonPressed?.Invoke(entity);
    }

    public void OnResourceIncreaseButtonPressed(Resources resources)
    {
        resources.IncreaseResource(Resource.Wood, 100);
        resources.IncreaseResource(Resource.Stone, 100);
    }
}
