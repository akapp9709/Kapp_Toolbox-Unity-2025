using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RagdollHandler : MonoBehaviour
{
    private Health _health;
    [SerializeField] private Animator animator;
    [SerializeField] private NavMeshAgent Agent;
    [SerializeField] private Transform RagDollRoot;

    private Collider[] _ragColliders;
    private Rigidbody[] _ragRigids;
    // Start is called before the first frame update
    void Start()
    {
        _ragColliders = RagDollRoot.GetComponentsInChildren<Collider>();
        foreach (var col in _ragColliders)
        {
            col.enabled = false;
        }

        _ragRigids = RagDollRoot.GetComponentsInChildren<Rigidbody>();
        foreach (var col in _ragRigids)
        {
            col.useGravity = false;
        }

        _health = GetComponent<Health>();
        _health.OnDeath += RagdollTransition;
    }

    private void RagdollTransition()
    {
        if (animator == null)
            animator = GetComponent<Animator>();

        animator.enabled = false;

        if (Agent == null)
            Agent = GetComponent<NavMeshAgent>();

        Agent.enabled = false;

        foreach (var col in _ragColliders)
        {
            col.enabled = true;
        }

        foreach (var col in _ragRigids)
        {
            col.useGravity = true;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
