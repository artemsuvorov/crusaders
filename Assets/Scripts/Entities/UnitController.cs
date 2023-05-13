using UnityEngine;

public class UnitController : MonoBehaviour
{
    private GameObject highlight;

    [SerializeField, Range(1.0f, 10.0f)]
    private float speed = 1.0f;

    private Vector3 targetPosition;

    private Animator animator;

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
        animator = GetComponentInChildren<Animator>();

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

        Animate();
    }

    private void Animate()
    {
        if (transform.position != targetPosition)
            animator.SetFloat("Speed", speed);
        else
            animator.SetFloat("Speed", 0.0f);
    }
}
