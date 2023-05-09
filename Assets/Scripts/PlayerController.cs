using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Vector2 startPosition, endPosition;
    private readonly List<UnitController> selectedUnits = new();

    [SerializeField]
    private Resources resources;

    [SerializeField]
    private Transform target;

    [SerializeField]
    private Transform selectionArea;

    private void Awake()
    {
        selectionArea.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startPosition = GetMousePositionInWorld();
            selectionArea.gameObject.SetActive(true);
        }

        if (Input.GetMouseButton(0))
            ResizeSelectionArea();

        if (Input.GetMouseButtonUp(0))
        {
            DeselectAllUnits();
            endPosition = GetMousePositionInWorld();
            var colliders = Physics2D.OverlapAreaAll(startPosition, endPosition);
            SelectUnits(colliders);
            selectionArea.gameObject.SetActive(false);
            //Debug.Log(selectedUnits.Count <= 0 ? "None" : selectedUnits.Count);
        }

        if (Input.GetMouseButton(1))
        {
            MoveTargetPoint();
            MoveSelectedUnits();
        }
    }

    private void ResizeSelectionArea()
    {
        var currentPosition = GetMousePositionInWorld();

        var minX = Mathf.Min(startPosition.x, currentPosition.x);
        var minY = Mathf.Min(startPosition.y, currentPosition.y);
        var maxX = Mathf.Max(startPosition.x, currentPosition.x);
        var maxY = Mathf.Max(startPosition.y, currentPosition.y);

        var lowerLeft = new Vector2(minX, minY);
        var upperRight = new Vector2(maxX, maxY);

        selectionArea.position = lowerLeft;
        selectionArea.localScale = upperRight - lowerLeft;
    }

    private void SelectUnits(Collider2D[] colliders)
    {
        foreach (var collider in colliders)
        {
            var unit = collider.GetComponent<UnitController>();
            if (unit is null)
                continue;

            selectedUnits.Add(unit);
            unit.Select();
            //Debug.Log(unit);
        }
    }

    private void DeselectAllUnits()
    {
        foreach (var unit in selectedUnits)
            unit.Deselect();
        selectedUnits.Clear();
    }

    private void MoveTargetPoint()
    {
        var position = GetMousePositionInWorld();
        target.position = position;
    }

    private void MoveSelectedUnits()
    {
        var position = GetMousePositionInWorld();
        foreach (var unit in selectedUnits)
            unit.MoveTo(position);
    }

    private Vector2 GetMousePositionInWorld()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
}
