using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitController : EntityController
{
    private GameObject highlight;

    [SerializeField, Range(1.0f, 10.0f)]
    private float speed = 1.0f;

    [SerializeField, Range(0.0f, 100.0f)]
    private float damage = 10.0f;

    private bool attacking = false;

    private Vector3 targetPosition;

    private NavMeshAgent agent;
    private Animator animator;

    public void MoveTo(Vector3 position)
    {
        if (!Alive)
            return;
        agent.isStopped = false;
        targetPosition = position;
    }

    public void Attack(EntityController closest)
    {
        if (!Alive)
            return;
        var routine = AttackRoutine(closest);
        StartCoroutine(routine);
    }

    private IEnumerator<YieldInstruction> AttackRoutine(EntityController closest)
    {
        if (attacking)
            yield break;
        
        attacking = true;
        StopMovement();
        animator.SetTrigger("Attack");
        yield return new WaitForSeconds(0.5f);

        closest.TakeDamage(damage);
        yield return new WaitForSeconds(0.4f);
        attacking = false;
    }

    public void Select()
    {
        highlight.SetActive(true);
    }

    public void Deselect()
    {
        highlight.SetActive(false);
    }

    protected override void OnDead()
    {
        base.OnDead();
        StopMovement();
    }

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

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

        if (Alive)
            agent.SetDestination(destination);

        Animate();
    }

    private void Animate()
    {
        animator.SetBool("Alive", Alive);

        if (agent.velocity.magnitude > 0.0f)
            animator.SetFloat("Speed", speed);
        else
            animator.SetFloat("Speed", 0.0f);
    }

    private void StopMovement()
    {
        agent.isStopped = true;
        targetPosition = transform.position;
    }
}
