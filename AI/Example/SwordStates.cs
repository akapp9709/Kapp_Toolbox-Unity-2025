using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using AIModels;
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

        public string GetName()
        {
            return "Patrol";
        }

        public void UpdateState(EnemyBehavior controller)
        {
            if (_agent.remainingDistance <= _agent.stoppingDistance)
            {
                Debug.Log("Ticking");
                _waitTimer.Tick(Time.deltaTime);
            }

            _anim.SetFloat(_hashSpeed, _agent.velocity.magnitude);

        }

        private void GoToNextPosition()
        {
            Debug.Log("Going tp nexzt position");
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
        int _hashSpeed, _hashX, _hashY;

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
            _hashY = Animator.StringToHash("YVelocity");
            _hashX = Animator.StringToHash("XVelocity");
        }

        public void ExitState(EnemyBehavior controller)
        {

        }

        public string GetName()
        {
            return "Combat";
        }

        public void UpdateState(EnemyBehavior controller)
        {
            //_requestTimer.Tick(Time.deltaTime);

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
            if (_manager.RequestTickets(2))
                _brain.ChangeState("Attack", _controller);
        }
    }

    public class AttackState : SwordState, IState
    {
        public AttackState(EnemyBrain brain) : base(brain)
        {
        }

        public void EnterState(EnemyBehavior controller)
        {
            Debug.Log("Entered Attack State");
        }

        public void ExitState(EnemyBehavior controller)
        {

        }

        public string GetName()
        {
            return "Attack";
        }

        public void UpdateState(EnemyBehavior controller)
        {

        }
    }

}
