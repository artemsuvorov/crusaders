using UnityEngine;

public class UnitController : MonoBehaviour
{
    [SerializeField]
    private InputController inputController;

    [SerializeField]
    private Transform target;

    [SerializeField, Range(1.0f, 10.0f)]
    private float speed;

    private void Start()
    {
        inputController.Clicked += OnPointerClicked;
    }

    private void OnPointerClicked(Vector3 position)
    {
        target.position = position;
    }

    private void Update()
    {
        var destination = Vector2.MoveTowards(
            transform.position,
            target.position,
            speed * Time.deltaTime
            );

        transform.position = destination;
    }
}
