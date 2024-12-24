using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AIModels;

public class SwordBrain : EnemyBrain
{
    public bool LineOfSight { get; set; }

    private Transform _target;
    private LayerMask _layerMask;

    public SwordBrain()
    {
        AddState("Patrol", new SwordStates.PatrolState(this));
        AddState("Combat", new SwordStates.CombatState(this));
    }

    public override void StartFSM(string startState, EnemyBehavior controller)
    {
        base.StartFSM(startState, controller);

        TryGetValue<Transform>("Target", out _target);
        TryGetValue<LayerMask>("Sight-Layer", out _layerMask);
    }

    public override void UpdateFSM(EnemyBehavior controller)
    {
        base.UpdateFSM(controller);
        var vertOffset = new Vector3(0, 1, 0);

        Debug.DrawLine(controller.transform.position + vertOffset, _target.position + vertOffset);


        if (!Physics.Linecast(controller.transform.position + vertOffset, _target.position + vertOffset, out RaycastHit hitInfo, _layerMask))
        {
            LineOfSight = true;

        }


        if (LineOfSight)
            ChangeState("Combat", controller);

    }
}
