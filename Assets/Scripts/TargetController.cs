using UnityEngine;
using UnityEngine.Events;

public class TargetController : MonoBehaviour
{
    private GameObject sprite;

    public UnityEvent<Vector2> Moved;

    private void Awake()
    {
        sprite = transform.Find("Sprite").gameObject;
        sprite.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
            sprite.SetActive(true);

        if (Input.GetMouseButton(1))
            MoveTargetPoint();
    }

    private void MoveTargetPoint()
    {
        var position = GetMousePositionInWorld();
        transform.position = position;
        Moved?.Invoke(position);
    }

    private Vector2 GetMousePositionInWorld()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
}
