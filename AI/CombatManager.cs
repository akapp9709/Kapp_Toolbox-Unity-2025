using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    public int maxTickets = 5;
    [SerializeField] int currentTickets;
    // Start is called before the first frame update
    void Start()
    {
        currentTickets = maxTickets;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool RequestTickets(int amount)
    {
        if (amount <= currentTickets)
        {
            currentTickets -= amount;
            Debug.Log("Ticket(s) granted");
            return true;
        }
        else
        {
            Debug.Log("Ticket(s) denied");
            return false;
        }
    }
}
