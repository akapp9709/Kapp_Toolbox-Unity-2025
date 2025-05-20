using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using AIModels;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class SwordStates
{
    public class SwordState
    {
        protected EnemyBrain _brain;
        public SwordState(EnemyBrain brain)
        {
            _brain = brain;
        }
    }
    public class PatrolState : SwordState, IState
    {
        List<Vector3> _patrolPoints;
        NavMeshAgent _agent;
        Animator _anim;
        Timer _waitTimer;
        int _posIndex = 0;

        int _hashSpeed;
        int _hashMotionSpeed;

        public PatrolState(EnemyBrain brain) : base(brain)
        {
        }

        public string Name => "Patrol";

        public void EnterState(EnemyBehavior controller)
        {
            _brain.TryGetValue("Patrol-Points", out _patrolPoints);
            _brain.TryGetValue("Animator", out _anim);
            if (_brain.TryGetValue("Nav-Agent", out _agent))
                _agent.SetDestination(_patrolPoints[_posIndex]);
            else
                Debug.LogWarning("<color=yellow>Unable to retrieve Agent</color>");

            _hashSpeed = Animator.StringToHash("Speed");
            _hashMotionSpeed = Animator.StringToHash("MotionSpeed");


            _waitTimer = new Timer(5f, GoToNextPosition);
        }

        public void ExitState(EnemyBehavior controller)
        {

        }

        public void UpdateState(EnemyBehavior controller)
        {
            if (_agent.remainingDistance <= _agent.stoppingDistance)
            {
                _waitTimer.Tick(Time.deltaTime);
            }

            _anim.SetFloat(_hashSpeed, _agent.velocity.magnitude);

        }

        private void GoToNextPosition()
        {
            _posIndex++;
            _posIndex = _posIndex % _patrolPoints.Count;

            _agent.SetDestination(_patrolPoints[_posIndex]);
            _waitTimer = new Timer(5f, GoToNextPosition);
        }
    }

    public class CombatState : SwordState, IState
    {
        public CombatState(EnemyBrain brain) : base(brain)
        {

        }

        private Timer _waitTimer;
        private Timer _requestTimer;
        private NavMeshAgent _agent;
        private Transform _targetTrans, _transform;
        private float _strafeDist;
        private Vector3 _targetPos;
        private bool _strafing;
        private CombatManager _manager;
        private EnemyBehavior _controller;
        Animator _anim;
        int _hashSpeed, _hashX, _hashY, _hashCombatFlag;

        public string Name => "Combat";

        public void EnterState(EnemyBehavior controller)
        {

            _controller = controller;
            _brain.TryGetValue("Target", out _targetTrans);
            _brain.TryGetValue("Strafe-Distance", out _strafeDist);
            _brain.TryGetValue("Nav-Agent", out _agent);
            _brain.TryGetValue("Combat-Manager", out _manager);
            _brain.TryGetValue("Animator", out _anim);

            _agent.stoppingDistance = _strafeDist;
            _targetPos = _targetTrans.position;
            if (NavMesh.SamplePosition(_targetPos, out NavMeshHit navMeshHit, 1f, NavMesh.AllAreas))
            {
                _agent.SetDestination(navMeshHit.position);
            }

            _waitTimer = new Timer(3, ChooseNewPosition);
            _requestTimer = new Timer(5, RequestTicket);
            _transform = controller.transform;

            _hashSpeed = Animator.StringToHash("Speed");
            _hashCombatFlag = Animator.StringToHash("inCombat");
            _hashY = Animator.StringToHash("YVelocity");
            _hashX = Animator.StringToHash("XVelocity");

            _anim.SetFloat(_hashCombatFlag, 1);
        }

        public void ExitState(EnemyBehavior controller)
        {
            _anim.SetFloat(_hashCombatFlag, 0);
        }

        public void UpdateState(EnemyBehavior controller)
        {
            _requestTimer.Tick(Time.deltaTime);

            if (!_strafing && _agent.remainingDistance > _strafeDist)
            {
                _strafing = true;
                Approach();
                return;
            }

            if (_strafing && _agent.remainingDistance > (_strafeDist + 2f))
            {
                _strafing = false;
            }

            _agent.speed = 3;
            _agent.updateRotation = false;
            _transform.LookAt(_targetTrans, Vector3.up);

            _waitTimer.Tick(Time.deltaTime);
            _anim.SetFloat(_hashSpeed, _agent.velocity.magnitude);
            _anim.SetFloat(_hashX, Vector3.Dot(_agent.velocity, _transform.right));
            _anim.SetFloat(_hashY, Vector3.Dot(_agent.velocity, _transform.forward));
        }

        private void Approach()
        {

            _agent.updateRotation = true;
            _agent.stoppingDistance = _strafeDist;
            _targetPos = _targetTrans.position;
            if (NavMesh.SamplePosition(_targetPos, out NavMeshHit navMeshHit, 1f, NavMesh.AllAreas))
            {
                _agent.SetDestination(navMeshHit.position);
            }

        }

        private void ChooseNewPosition()
        {
            var ray = _targetTrans.forward;
            var theta = Random.Range(0, 360) * Mathf.Deg2Rad;

            var dir = new Vector3(ray.x * Mathf.Cos(theta), ray.y, ray.z * Mathf.Sin(theta));
            dir.Normalize();
            _agent.ResetPath();
            _agent.stoppingDistance = 0.5f;
            _targetPos = dir * _strafeDist + _targetTrans.position;
            Debug.DrawLine(_transform.position, _targetPos, Color.red, 0.5f);
            if (NavMesh.SamplePosition(_targetPos, out NavMeshHit navMeshHit, 1f, NavMesh.AllAreas))
            {
                _agent.SetDestination(navMeshHit.position);
            }

            _waitTimer = new Timer(3, ChooseNewPosition);
        }

        private void RequestTicket()
        {
            if (_manager.RequestTickets(2, _controller))
                _brain.ChangeState("Attack", _controller);

            _requestTimer = new Timer(5, RequestTicket);
        }
    }

    public class AttackState : SwordState, IState
    {
        NavMeshAgent _agent;
        Animator _anim;
        float _attackRange;
        Transform _targetTrans;
        int _hashAttackTrigger;
        bool _attacking;

        public string Name => "Attack";

        public AttackState(EnemyBrain brain) : base(brain)
        {
        }

        public void EnterState(EnemyBehavior controller)
        {
            _brain.TryGetValue("Nav-Agent", out _agent);
            _brain.TryGetValue("Animator", out _anim);
            _brain.TryGetValue("Attack-Range", out _attackRange);
            _brain.TryGetValue("Target", out _targetTrans);

            Animator.StringToHash("Attack");

            _agent.stoppingDistance = _attackRange;
            _agent.destination = _targetTrans.position;
        }

        public void UpdateState(EnemyBehavior controller)
        {
            if (_agent.remainingDistance < _agent.stoppingDistance && !_attacking)
            {
                _agent.ResetPath();
                _anim.SetTrigger("Attack");
                _attacking = true;
                return;
            }

            var state = _anim.GetCurrentAnimatorStateInfo(0);

            if (_attacking && state.normalizedTime > 0.4f && state.IsName("OneHand_Up_Attack_1"))
                _brain.ChangeState("Retreat", controller);
        }

        public void ExitState(EnemyBehavior controller)
        {
            _attacking = false;
        }
    }

    public class RetreatState : SwordState, IState
    {
        Transform _transform, _targetTrans;
        NavMeshAgent _agent;
        Animator _anim;
        float _strafeDistance;

        int _hashSpeed, _hashY, _hashX;

        public RetreatState(EnemyBrain brain) : base(brain) { }

        public string Name => "Retreat";

        public void EnterState(EnemyBehavior controller)
        {
            // Debug.Log("Entering Retreat State");

            _transform = controller.transform;
            _brain.TryGetValue("Target", out _targetTrans);
            _brain.TryGetValue("Nav-Agent", out _agent);
            _brain.TryGetValue("Animator", out _anim);
            _brain.TryGetValue("Strafe-Distance", out _strafeDistance);
            _agent.ResetPath();

            var randomAngle = Random.Range(-30f, 30f);
            Quaternion rotate = Quaternion.Euler(0, randomAngle, 0);

            var rotatedForward = rotate * _transform.forward;

            var targetPos = (rotatedForward * -_strafeDistance) + _targetTrans.position;
            Debug.DrawLine(_transform.position, targetPos, Color.blue, 1f);
            _agent.stoppingDistance = 0.5f;

            _agent.SetDestination(targetPos);

            _hashSpeed = Animator.StringToHash("Speed");
            _hashX = Animator.StringToHash("XVelocity");
            _hashY = Animator.StringToHash("YVelocity");
        }

        public void ExitState(EnemyBehavior controller)
        {

        }

        public void UpdateState(EnemyBehavior controller)
        {
            if (_agent.remainingDistance < _agent.stoppingDistance)
            {
                _brain.ChangeState("Combat", controller);
            }

            _anim.SetFloat(_hashSpeed, _agent.velocity.magnitude);
            _anim.SetFloat(_hashX, Vector3.Dot(_agent.velocity, _transform.right));
            _anim.SetFloat(_hashY, Vector3.Dot(_agent.velocity, _transform.forward));
        }
    }

}
