using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class UnitController : EntityController
{
    private bool attacking = false;

    private Vector3 targetPosition;
    private EntityController attackTarget;

    private GameObject highlight;
    private NavMeshAgent agent;
    private Animator animator;

    public float Damage { get; protected set; } = 10.0f;

    public bool Selectable { get; set; } = true;

    public bool Selected
    { 
        get => Selectable && highlight.activeSelf;
        set => highlight.SetActive(Selectable && value);
    }

    public void MoveTo(Vector3 position)
    {
        if (!Alive)
            return;
        agent.isStopped = false;
        targetPosition = position;
        //if (Selectable && Selected)
        //    Debug.Log("move position: " + targetPosition);
    }

    public void MoveTo(EntityController target)
    {
        // TODO: fix close position make realtive to current position of unit
        var closePosition = target.Position - 
            0.5f * target.Size.magnitude * target.Position.normalized;
        if (DistanceTo(target) <= Vector2.Distance(target.Position, closePosition))
            return;
        MoveTo(closePosition);
    }

    public void SelectAttackTarget(EntityController target)
    {
        if (!Alive)
            return;
        attackTarget = target;
        //Debug.Log("target selected: " + target);
    }

    public void DeselectAttackTarget()
    {
        if (!Alive)
            return;
        attackTarget = null;
        //Debug.Log("target deselected");
    }

    private void Awake()
    {
        Died += () => StopMovement();

        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        animator = GetComponentInChildren<Animator>();

        targetPosition = transform.position;
        highlight = transform.Find("Highlight").gameObject;
        Selected = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
            TakeDamage(10);

        if (attackTarget is not null && attackTarget.Alive)
        {
            MoveTo(attackTarget);
            StartAttackingTarget(attackTarget);
        }

        var destination = new Vector3(
            targetPosition.x, 
            targetPosition.y, 
            transform.position.z
            );

        if (Alive)
            agent.SetDestination(destination);

        Animate();
    }

    private void StartAttackingTarget(EntityController target)
    {
        var distance = DistanceTo(target);
        var attackRange = 0.6f * target.Size.magnitude;
        if (distance > attackRange)
            return;
        var routine = AttackRoutine(target);
        StartCoroutine(routine);
    }

    private IEnumerator<YieldInstruction> AttackRoutine(EntityController target)
    {
        if (attacking || !Alive)
            yield break;

        attacking = true;
        StopMovement();
        yield return new WaitForSeconds(0.5f);

        target.TakeDamage(Damage);
        yield return new WaitForSeconds(0.4f);
        attacking = false;
    }

    private void Animate()
    {
        animator.SetBool("Alive", Alive);
        animator.SetBool("Attack", attacking);

        var direction = agent.velocity.normalized;
        if (direction.magnitude < 0.0001f)
        {
            animator.SetFloat("Speed", 0.0f);
            return;
        }

        animator.SetFloat("Speed", 1.0f);

        var directionX = Mathf.RoundToInt(direction.x);
        var directionY = Mathf.RoundToInt(direction.y);

        animator.SetInteger("DirectionX", directionX);
        animator.SetInteger("DirectionY", directionY);
    }

    private void StopMovement()
    {
        agent.isStopped = true;
        targetPosition = transform.position;
    }
}
