using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float healthPoints;

    public Action OnDeath;

    public void DeductHealthPts(float damage)
    {
        healthPoints -= damage;

        if (healthPoints <= 0)
            OnDeath?.Invoke();
    }
}
