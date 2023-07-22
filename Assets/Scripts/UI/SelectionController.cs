using UnityEngine;
using UnityEngine.Events;

public class SelectionEventArgs
{
    public Vector2 Start { get; private set; }
    public Vector2 End { get; private set; }

    public SelectionEventArgs(Vector2 start, Vector2 end)
    {
        Start = start;
        End = end;
    }
}

public class SelectionController : MonoBehaviour
{
    private Vector2 start, end;
    private GameObject sprite;

    public UnityEvent<SelectionEventArgs> AreaSelected;

    private void Awake()
    {
        sprite = transform.Find("Sprite").gameObject;
        sprite.SetActive(false);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            start = GetMousePositionInWorld();
            sprite.SetActive(true);
        }

        if (Input.GetMouseButton(0))
            ResizeSelectionArea();

        if (Input.GetMouseButtonUp(0))
        {
            end = GetMousePositionInWorld();
            sprite.SetActive(false);
            var args = new SelectionEventArgs(start, end);
            AreaSelected?.Invoke(args);
        }
    }

    private void ResizeSelectionArea()
    {
        var currentPosition = GetMousePositionInWorld();

        var minX = Mathf.Min(start.x, currentPosition.x);
        var minY = Mathf.Min(start.y, currentPosition.y);
        var maxX = Mathf.Max(start.x, currentPosition.x);
        var maxY = Mathf.Max(start.y, currentPosition.y);

        var lowerLeft = new Vector2(minX, minY);
        var upperRight = new Vector2(maxX, maxY);

        var offset = 0.5f * (upperRight - lowerLeft);
        sprite.transform.position = offset + lowerLeft;
        sprite.transform.localScale = upperRight - lowerLeft;
    }

    private Vector2 GetMousePositionInWorld()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
}
