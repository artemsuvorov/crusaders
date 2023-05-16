using System;
using UnityEngine;
using UnityEngine.AI;

public class UnitController : EntityController
{
    private GameObject highlight;

    [SerializeField, Range(1.0f, 10.0f)]
    private float speed = 1.0f;

    [SerializeField, Range(0.0f, 100.0f)]
    private float damage = 10.0f;

    private Vector3 targetPosition;

    private NavMeshAgent agent;
    private Animator animator;

    public void MoveTo(Vector3 position)
    {
        if (!Alive)
            return;
        targetPosition = position;
    }

    public void Attack(EntityController closest)
    {
        if (!Alive)
            return;
        closest.TakeDamage(damage);
    }

    public void Select()
    {
        highlight.SetActive(true);
    }

    public void Deselect()
    {
        highlight.SetActive(false);
    }

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
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
        if (Input.GetKeyDown(KeyCode.P))
            TakeDamage(10);

        var destination = new Vector3(
            targetPosition.x, 
            targetPosition.y, 
            transform.position.z
            );

        agent.SetDestination(destination);

        Animate();
    }

    private void Animate()
    {
        if (agent.velocity.magnitude > 0.0f)
            animator.SetFloat("Speed", speed);
        else
            animator.SetFloat("Speed", 0.0f);
    }
}
