using System.Collections;
using System.Collections.Generic;
using AIModels;
using BehaviorTree.Interfaces;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    public int maxTickets = 5;
    [SerializeField] int currentTickets;

    public float ReplenishTime = 2f, ticketCooldown = 1f;
    [Range(0, 1)]
    public float repeatAttackChance = 0.5f;
    Timer _replenishTimer, _coolDownTimer;
    bool _canGrantTicket = true;
    EnemyBehavior _lastAttacker;
    IBehaviorTreeEvents _lastTreeEntity;
    // Start is called before the first frame update
    void Start()
    {
        _replenishTimer = new Timer(ReplenishTime, ReplenishTicket);
    }

    // Update is called once per frame
    void Update()
    {
        _coolDownTimer?.Tick(Time.deltaTime);
        if (currentTickets <= maxTickets)
            _replenishTimer?.Tick(Time.deltaTime);
    }

    private void ReplenishTicket()
    {
        currentTickets++;

        _replenishTimer = new Timer(ReplenishTime, ReplenishTicket);
    }

    private void ToggleGranting()
    {
        _canGrantTicket = true;
    }

    public bool RequestTicket(int amount, IBehaviorTreeEvents _events)
    {
        bool canAttack = (_events != _lastTreeEntity)
            || Random.Range(0, 1f) > repeatAttackChance;

        if (!canAttack)
            return false;

        if (amount <= currentTickets && _canGrantTicket)
        {
            _canGrantTicket = false;
            currentTickets -= amount;
            _coolDownTimer = new Timer(ticketCooldown, ToggleGranting);
            _lastTreeEntity = _events;
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool RequestTickets(int amount, EnemyBehavior entity)
    {
        //Weighted attack selection
        bool canAttack = (entity != _lastAttacker)
            || Random.Range(0, 1f) > repeatAttackChance;

        if (!canAttack)
            return false;

        if (amount <= currentTickets && _canGrantTicket)
        {
            _canGrantTicket = false;
            currentTickets -= amount;
            _coolDownTimer = new Timer(ticketCooldown, ToggleGranting);
            _lastAttacker = entity;
            return true;
        }
        else
        {
            return false;
        }
    }
}
