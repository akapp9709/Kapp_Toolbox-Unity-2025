using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AIModels;

namespace AIModels
{
    public abstract class EnemyFSM
    {
        private Dictionary<string, IState> _states = new Dictionary<string, IState>();
        protected IState _currentState;
        public bool isActive;


        public void AddState(string name, IState state)
        {
            if (!_states.ContainsKey(name))
            {
                _states[name] = state;
            }
        }

        public virtual void StartFSM(string startState, EnemyBehavior controller)
        {
            if (_states.ContainsKey(startState))
            {
                _currentState = _states[startState];
                _currentState.EnterState(controller);
            }
        }

        public virtual void UpdateFSM(EnemyBehavior controller)
        {
            if (_currentState != null)
            {
                _currentState.UpdateState(controller);
            }
        }

        public void ChangeState(string state, EnemyBehavior controller)
        {
            if (_states.ContainsKey(state) && state != _currentState.Name)
            {
                _currentState.ExitState(controller);
                _currentState = _states[state];
                _currentState.EnterState(controller);
            }
            else
            {
                Debug.LogWarning($"{state} State Not Defined in Brain");
            }
        }
    }

    public interface IState
    {
        string Name { get; }
        void EnterState(EnemyBehavior controller);
        void UpdateState(EnemyBehavior controller);
        void ExitState(EnemyBehavior controller);
    }
}

