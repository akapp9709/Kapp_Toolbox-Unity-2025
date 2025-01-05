using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterHitboxManager : MonoBehaviour
{
    List<Hitbox> _hitboxes = new List<Hitbox>();

    void Start()
    {
        _hitboxes = GetComponentsInChildren<Hitbox>().ToList();
    }


    public void OnActivateHitBox(string name)
    {
        Debug.Log("Activating Hitboxes");
        foreach (var hitbox in _hitboxes.Where(h => h.Name == name))
        {
            hitbox.active = true;
        }
    }

    public void DisableHitBoxes(string name = "")
    {
        Debug.Log("Deactivating Hitboxes");

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
}
