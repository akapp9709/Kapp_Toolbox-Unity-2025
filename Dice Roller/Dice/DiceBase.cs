using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO's/Dice")]
public class DiceBase : ScriptableObject
{
    public int Maximum;
    public enum WeightType
    {
        None,
        Minimum,
        Maximum,
        Middle,
    }
    public WeightType weightType;


}
