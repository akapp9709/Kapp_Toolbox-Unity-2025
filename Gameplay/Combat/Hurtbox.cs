using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Hurtbox : MonoBehaviour
{
    public Health health;
    // Start is called before the first frame update
    void Start()
    {
        if (health == null)
        {
            health = GetComponentInParent<Health>();
        }
    }


}
