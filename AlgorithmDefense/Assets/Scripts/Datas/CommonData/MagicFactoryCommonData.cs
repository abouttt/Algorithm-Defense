using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MagicFactoryCommonData", menuName = "ScriptableObject/MagicFactoryCommonData", order = 2)]
public class MagicFactoryCommonData : ScriptableObject
{
    [ArrayElementNamed(new string[] { "³«·Ú", "ºÐ³ë", "Ä¡À¯"})]
    [SerializeField]
    public int[] MagicCountData;
}
