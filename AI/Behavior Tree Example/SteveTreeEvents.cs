using System.Collections;
using System.Collections.Generic;
using BehaviorTree;
using BehaviorTree.Interfaces;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class SteveTreeEvents : MonoBehaviour,
                                IMoveToPointEvents,
                                INextPositionEvents,
                                IApproachEvents,
                                IStrafeAroundEvents,
                                IRequestActionTicketEvents,
                                IAttackEvents,
                                IRetreatEvents
{
    private NavMeshAgent _agent;
    private Transform _target;
    private CombatManager _manager;
    private Animator _anim;

    #region Animator Values
    private int _hashSpeed, _hashCombatFlag,
                _hashXVelocity, _hashYVelocity,
                _hashAttack;

    #endregion

    [Header("Patrol Properties")]
    public List<Vector3> waypoints = new List<Vector3>();
    private int currentIndex = 0;

    [Header("Combat Properties")]
    public float strafeDistance;
    [Range(0, 360)]
    public float strafeAngle;
    public float attackRange;
    [Range(0, 180)]
    public float retreatAngle;

    void OnDrawGizmos()
    {
        foreach (var pnt in waypoints)
        {
            Gizmos.DrawCube(pnt, Vector3.one);
        }
    }

    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _target = GetComponent<SteveNewBrain>().target;
        _manager = FindObjectOfType<CombatManager>();
        _anim = GetComponent<Animator>();

        _hashSpeed = Animator.StringToHash("Speed");
        _hashCombatFlag = Animator.StringToHash("inCombat");
        _hashYVelocity = Animator.StringToHash("YVelocity");
        _hashXVelocity = Animator.StringToHash("XVelocity");
        _hashAttack = Animator.StringToHash("Attack");
    }

    void Update()
    {
        _anim.SetFloat(_hashSpeed, _agent.velocity.magnitude);
    }

    #region MoveToPoint
    public void OnMoveToPointStart()
    {
        _agent.SetDestination(waypoints[currentIndex]);
        // Debug.Log("Going to " + waypoints[currentIndex]);
    }

    public void OnMoveToPointStop()
    {
        // _agent.ResetPath();
    }

    public State OnMoveToPointUpdate()
    {
        _anim.SetFloat(_hashSpeed, _agent.velocity.magnitude);
        Debug.Log("MoveToPointUpdate");

        if (_agent.remainingDistance >= _agent.stoppingDistance)
        {


            return State.RUNNING;
        }

        return State.SUCCESS;
    }
    #endregion

    #region NextPosition
    public void OnNextPositionStart()
    {
        currentIndex++;
        currentIndex = currentIndex % waypoints.Count;
    }

    public void OnNextPositionStop()
    {

    }

    public State OnNextPositionUpdate()
    {
        return State.SUCCESS;
    }
    #endregion

    #region Approach
    public void OnApproachStart()
    {
        _agent.ResetPath();
        _agent.stoppingDistance = strafeDistance;
        _agent.SetDestination(_target.position);
    }

    public void OnApproachStop()
    {

    }

    public State OnApproachUpdate()
    {
        if (_agent.remainingDistance <= strafeDistance)
            return State.SUCCESS;

        Debug.Log("Stuck in Approaching State");
        return State.RUNNING;
    }
    #endregion

    public void OnStrafeAroundStart()
    {
        _agent.ResetPath();

        _agent.stoppingDistance = strafeDistance;

        ChoosePosition();
    }

    private void ChoosePosition()
    {
        var ray = _target.forward;
        var theta = Random.Range(-strafeAngle / 2, strafeAngle / 2) * Mathf.Deg2Rad;

        var dir = new Vector3(ray.x * Mathf.Sin(theta), ray.y, ray.z * Mathf.Cos(theta));
        dir.Normalize();
        _agent.stoppingDistance = 0.5f;

        var targetPos = dir * strafeDistance + _target.position;

        if (NavMesh.SamplePosition(targetPos, out NavMeshHit navMeshHit, 1f, NavMesh.AllAreas))
        {
            _agent.SetDestination(navMeshHit.position);
        }

        _anim.SetFloat(_hashCombatFlag, 1);
    }

    public void OnStrafeAroundStop()
    {

    }

    public State OnStrafeAroundUpdate()
    {
        _agent.updateRotation = false;
        transform.LookAt(_target, Vector3.up);

        _anim.SetFloat(_hashSpeed, _agent.velocity.magnitude);
        _anim.SetFloat(_hashXVelocity, Vector3.Dot(_agent.velocity, transform.right));
        _anim.SetFloat(_hashYVelocity, Vector3.Dot(_agent.velocity, transform.forward));

        if (_agent.remainingDistance < _agent.stoppingDistance)
        {
            _anim.SetFloat(_hashCombatFlag, 0);
            return State.SUCCESS;
        }

        return State.RUNNING;
    }

    private bool _request;
    public void OnRequestActionTicketStart()
    {
        Debug.Log("Requesting Ticket For Steve");
        if (_manager.RequestTicket(1, this))
        {
            _request = true;
        }
        else
            _request = false;
    }

    public void OnRequestActionTicketStop()
    {

    }

    public State OnRequestActionTicketUpdate()
    {
        if (_request)
            return State.SUCCESS;

        return State.FAILURE;
    }


    private bool _attacking;
    public void OnAttackStart()
    {
        Debug.Log("Attack Node Start - " + Time.time);
        Debug.Log("Steve Initiating Attack");
        _agent.SetDestination(_target.position);
        _agent.stoppingDistance = attackRange;


    }

    public void OnAttackStop()
    {

    }

    public State OnAttackUpdate()
    {
        Debug.Log("Attack Node Update - " + Time.time);

        if (_agent.remainingDistance < attackRange && !_attacking)
        {
            Debug.Log("Getting into Range");
            _agent.ResetPath();
            _anim.SetTrigger(_hashAttack);
            _attacking = true;
            return State.RUNNING;
        }

        var state = _anim.GetCurrentAnimatorStateInfo(0);

        if (_attacking && state.normalizedTime > 0.4f && state.IsName("OneHand_Up_Attack_1"))
        {
            _attacking = false;
            return State.SUCCESS;
        }

        if (_agent.remainingDistance > attackRange)
            Debug.Log("Not close enough - need more time");

        if (!_attacking)
            Debug.Log("Attack Flag is fucking out");

        if (state.normalizedTime < 0.4f)
            Debug.Log("State time should be read at a differnt point");

        if (!state.IsName("OneHand_Up_Attack_1"))
            Debug.Log("Check Attack Animation State Name");

        // Debug.Log("IDK whats up but this failed");
        return State.FAILURE;
    }

    public void OnRetreatStart()
    {
        Debug.Log("Starting Retreat");

        var randomAngle = Random.Range(-retreatAngle / 2, retreatAngle / 2);
        Quaternion rotation = Quaternion.Euler(0, randomAngle, 0);

        var rotatedForward = rotation * transform.forward;

        var targetPos = (rotatedForward * -strafeDistance) + _target.position;

        _agent.updateRotation = false;
        _agent.stoppingDistance = 0.5f;
        _agent.SetDestination(targetPos);
    }

    public void OnRetreatStop()
    {

    }

    public State OnRetreatUpdate()
    {
        _agent.updateRotation = false;
        transform.LookAt(_target, Vector3.up);

        _anim.SetFloat(_hashSpeed, _agent.velocity.magnitude);
        _anim.SetFloat(_hashXVelocity, Vector3.Dot(_agent.velocity, transform.right));
        _anim.SetFloat(_hashYVelocity, Vector3.Dot(_agent.velocity, transform.forward));

        if (_agent.remainingDistance < _agent.stoppingDistance)
            return State.SUCCESS;

        return State.RUNNING;
    }
}
