using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //private Vector2 startPosition, endPosition;
    private readonly List<UnitController> selectedUnits = new();

    [SerializeField]
    private Resources resources;

    [SerializeField]
    private Transform target;

    //[SerializeField]
    //private Transform selectionArea;

    public void OnAreaSelected(SelectionEventArgs args)
    {
        DeselectAllUnits();
        var colliders = Physics2D.OverlapAreaAll(args.Start, args.End);
        SelectUnits(colliders);
    }

    private void Update()
    {
        if (Input.GetMouseButton(1))
        {
            MoveTargetPoint();
            MoveSelectedUnits();
        }
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
            Debug.Log(unit);
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
