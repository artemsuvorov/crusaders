using UnityEngine;

public class UnitController : MonoBehaviour
{
    [SerializeField]
    private GameObject highlight;

    [SerializeField, Range(1.0f, 10.0f)]
    private float speed;

    private Vector3 targetPosition;

    public void MoveTo(Vector3 position)
    {
        targetPosition = position;
    }

    public void Select()
    {
        highlight.SetActive(true);
    }

    public void Deselect()
    {
        highlight.SetActive(false);
    }

    private void Awake()
    {
        targetPosition = transform.position;
        highlight = transform.Find("Highlight").gameObject;
        Deselect();
    }

    private void Update()
    {
        var destination = Vector2.MoveTowards(
            transform.position,
            targetPosition,
            speed * Time.deltaTime
            );

        transform.position = destination;
    }
}
