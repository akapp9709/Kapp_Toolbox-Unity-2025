using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AIModels;
using UnityEngine.AI;

public class SwordBehavior : EnemyBehavior
{
    [SerializeField] List<Vector3> PatrolPoints = new List<Vector3>();
    [SerializeField] float StrafeDistance = 3;
    public LayerMask SightLayerMask;

    int _hashMotionSpeed;

    void OnDrawGizmos()
    {
        foreach (var pos in PatrolPoints)
        {
            Gizmos.DrawCube(pos, Vector3.one * 0.5f);
        }
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        _brain = new SwordBrain();
        _brain.AddToDictionary("Patrol-Points", PatrolPoints);
        _brain.AddToDictionary("Nav-Agent", GetComponent<NavMeshAgent>());
        _brain.AddToDictionary("Target", GameObject.FindGameObjectWithTag("Player").transform);
        _brain.AddToDictionary("Sight-Layer", SightLayerMask);
        _brain.AddToDictionary("Strafe-Distance", StrafeDistance);
        _brain.AddToDictionary("Combat-Manager", FindObjectOfType<CombatManager>());
        _brain.AddToDictionary("Animator", GetComponent<Animator>());

        _hashMotionSpeed = Animator.StringToHash("MotionSpeed");
        GetComponent<Animator>().SetFloat(_hashMotionSpeed, 1);

        _brain.StartFSM("Patrol", this);
    }

    // Update is called once per frame
    protected override void Update()
    {
        _brain.UpdateFSM(this);
    }
}
