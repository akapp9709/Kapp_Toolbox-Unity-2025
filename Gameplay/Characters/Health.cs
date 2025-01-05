using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float healthPoints;

    public void DeductHealthPts(float damage)
    {
        healthPoints -= damage;
    }
}
