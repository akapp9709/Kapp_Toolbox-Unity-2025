using System.Collections;
using System.Collections.Generic;
using BehaviorTree.Interfaces;
using UnityEngine;

public class SteveTreeEvents : MonoBehaviour, IFUckYaMumEvents, ITestEvents
{
    public void OnFUckYaMumStart()
    {
        Debug.Log("FuckYa Mum!");
    }

    public void OnFUckYaMumStop()
    {
        Debug.Log("FuckYa Mum!");
    }

    public State OnFUckYaMumUpdate()
    {
        Debug.Log("FuckYa Mum! UPDATE");
        return State.RUNNING;
    }

    public void OnTestStart()
    {
        throw new System.NotImplementedException();
    }

    public void OnTestStop()
    {
        throw new System.NotImplementedException();
    }

    public State OnTestUpdate()
    {
        throw new System.NotImplementedException();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
