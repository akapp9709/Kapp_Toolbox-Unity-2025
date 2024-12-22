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
        Timer _waitTimer;
        int _posIndex = 0;

        public PatrolState(EnemyBrain brain) : base(brain)
        {
        }

        public void EnterState(EnemyBehavior controller)
        {
            _brain.TryGetValue("Patrol-Points", out _patrolPoints);
            if (_brain.TryGetValue("Nav-Agent", out _agent))
                _agent.SetDestination(_patrolPoints[_posIndex]);
            else
                Debug.LogWarning("<color=yellow>Unable to retrieve Agent</color>");

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

        public void EnterState(EnemyBehavior controller)
        {

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

        }
    }

}
