using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class Hitbox : MonoBehaviour
{

    public string Name;
    public float damage;
    public bool active;
    public LayerMask layerMask;

    private void OnTriggerEnter(Collider other)
    {
        var hurtbox = other.GetComponent<Hurtbox>();

        if (hurtbox != null && active)
        {
            Debug.Log("Hit Something");
            hurtbox.health.DeductHealthPts(damage);
        }

    }
}
