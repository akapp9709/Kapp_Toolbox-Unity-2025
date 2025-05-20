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
        public Transform target;
        [SerializeField] LayerMask _layerMask;
        private Vector3 debugPos;

        LineOfSight _sight;

        void OnDrawGizmos()
        {
            if (target != null)
                Gizmos.DrawSphere(target.position, 1f);

            Gizmos.DrawCube(debugPos, Vector3.one);
        }

        protected override void Awake()
        {
            base.Awake();
            tree.behaviorTreeEvents = GetComponent<IBehaviorTreeEvents>();
        }

        protected override void Start()
        {
            _sight = GetComponent<LineOfSight>();

            WriteToBlackBoard("line-of-sight", _lineOfSight);
            target = GameObject.FindGameObjectWithTag("Player").transform;
            base.Start();
        }

        protected override void Update()
        {
            base.Update();

            var pos = transform.position + vertOffset;
            var targetPos = target.position + vertOffset;

            // if (Physics.Raycast(pos, targetPos - pos, out RaycastHit hitInfo, 100, ~_layerMask))
            // {
            //     Debug.Log(hitInfo.transform.tag);
            //     if (hitInfo.transform.tag == "Player")
            //     {
            //         _lineOfSight = true;
            //     }
            //     else
            //     {
            //         _lineOfSight = false;
            //     }
            //     debugPos = hitInfo.point;
            // }

            _lineOfSight = _sight.FindVisibleTargets();

            WriteToBlackBoard("line-of-sight", _lineOfSight);

            var distanceToPlayer = Vector3.Distance(targetPos, pos);
            WriteToBlackBoard("distance-to-player", distanceToPlayer);
        }
    }
}
