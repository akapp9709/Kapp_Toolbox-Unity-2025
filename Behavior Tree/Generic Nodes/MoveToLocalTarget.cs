using UnityEngine;

public class MoveToLocalTarget : ActionNode
{
    /// <summary>
    /// Moves the Enity to a specified position relative to their current position.
    /// </summary>
    /// <param name="blackBoard"></param>
    /// 

    public Vector3 TargetPosition = Vector3.zero;
    public float Speed = 2f;
    private Vector3 _pos, _dir;
    private Transform _transform;
    public MoveToLocalTarget(BlackBoard blackBoard) : base(blackBoard)
    {
    }

    protected override void OnStart()
    {
        if (blackBoard.GetValue<Transform>("Transform", out _transform))
        {
            _pos = _transform.position + TargetPosition;
            Debug.Log("Starting again");
            _dir = TargetPosition.normalized;
        }
        else
        {
            _pos = Vector3.zero;
            Debug.Log("<color=red><b>No Transform found</b></color> " + Time.fixedDeltaTime);//sfghj
        }

    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        _transform.position += _dir * Speed * Time.fixedDeltaTime;
        Debug.Log($"Moving, deltatime:\t{Time.fixedDeltaTime}\t\ttargetposition: {_pos}");

        if (Vector3.Distance(_transform.position, _pos) < 0.05f)
            return State.SUCCESS;

        return State.RUNNING;
    }
}
