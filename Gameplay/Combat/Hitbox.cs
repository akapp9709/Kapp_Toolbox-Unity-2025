using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    public float damage;
    public LayerMask layerMask;

    private void OnTriggerEnter(Collider other)
    {
        var hurtbox = other.GetComponent<Hurtbox>();

        if (hurtbox != null)
        {
            Debug.Log("Hit Something");
            hurtbox.health.DeductHealthPts(damage);
        }

    }
}
