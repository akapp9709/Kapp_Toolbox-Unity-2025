using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterHitboxManager : MonoBehaviour
{
    List<Hitbox> _hitboxes = new List<Hitbox>();
    List<Hurtbox> _hurtboxes = new List<Hurtbox>();

    void Start()
    {
        _hitboxes = GetComponentsInChildren<Hitbox>().ToList();
        _hurtboxes = GetComponentsInChildren<Hurtbox>().ToList();
    }


    public void OnActivateHitBox(string name)
    {
        foreach (var hitbox in _hitboxes.Where(h => h.Name == name))
        {
            hitbox.active = true;
        }
    }

    public void DisableHitBoxes(string name = "")
    {

        if (name == "")
        {
            foreach (var h in _hitboxes)
            {
                h.active = false;
            }
            return;
        }

        foreach (var hitbox in _hitboxes.Where(h => h.Name == name))
        {
            hitbox.active = false;
        }
    }

    public void EnableHurtbox(string name)
    {
        Debug.Log("Trying to enable");
        foreach (var hurtbox in _hurtboxes.Where(h => h.Name == name))
        {
            hurtbox.ActivateHurtbox();
        }
    }

    public void DisableHurtboxes(string name = "")
    {
        Debug.Log("Trying to disable");

        if (name == "")
        {
            foreach (var h in _hurtboxes)
            {
                h.DeactivateHurtbox();
            }
            return;
        }

        foreach (var hurtbox in _hurtboxes.Where(h => h.Name == name))
        {
            hurtbox.DeactivateHurtbox();
        }
    }
}
