using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class InputController : MonoBehaviour
{
    public event UnityAction<Vector3> Clicked;

    private void OnMouseOver()
    {
        if (!Input.GetMouseButton(1))
            return;
        var position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Clicked?.Invoke(new Vector2(position.x, position.y));
    }
}
