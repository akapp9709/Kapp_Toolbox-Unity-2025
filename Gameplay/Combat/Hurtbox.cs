using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Hurtbox : MonoBehaviour
{
    public Health health;

    public string Name;
    public float ZoneDamageReduction;

    public bool IsActive;
    public float EffectiveDamageReduction;
    // Start is called before the first frame update
    void Start()
    {
        if (health == null)
        {
            health = GetComponentInParent<Health>();
        }
    }

    void Update()
    {
        EffectiveDamageReduction = IsActive ? ZoneDamageReduction : 0;
    }

    public void ActivateHurtbox()
    {
        IsActive = true;
        Debug.Log("Activating Hurtbox");
    }

    public void DeactivateHurtbox()
    {
        IsActive = false;
        Debug.Log("Deactivating Hurtbox");
    }

}
