using System.Collections;
using System.Collections.Generic;
using BehaviorTree.Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace BehaviorTree
{
    public class SteveNewBrain : Brain
    {
        Vector3 vertOffset = new Vector3(0, 1, 0);
        [SerializeField] bool _lineOfSight;
        Transform _target;
        [SerializeField] LayerMask _layerMask;
        private Vector3 debugPos;

        void OnDrawGizmos()
        {
            if (_target != null)
                Gizmos.DrawSphere(_target.position, 1f);

            Gizmos.DrawCube(debugPos, Vector3.one);
        }

        protected override void Awake()
        {
            base.Awake();
            tree.behaviorTreeEvents = GetComponent<IBehaviorTreeEvents>();
        }

        protected override void Start()
        {

            WriteToBlackBoard("line-of-sight", _lineOfSight);
            _target = GameObject.FindGameObjectWithTag("Player").transform;
            base.Start();
        }

        protected override void Update()
        {
            base.Update();

            var pos = transform.position + vertOffset;
            var targetPos = _target.position + vertOffset;

            if (Physics.Raycast(pos, targetPos - pos, out RaycastHit hitInfo, 100, ~_layerMask))
            {
                if (hitInfo.transform.tag == "Player")
                {
                    _lineOfSight = true;
                }
                else
                {
                    _lineOfSight = false;
                }
                debugPos = hitInfo.point;
            }

            WriteToBlackBoard("line-of-sight", _lineOfSight);
        }
    }
}
