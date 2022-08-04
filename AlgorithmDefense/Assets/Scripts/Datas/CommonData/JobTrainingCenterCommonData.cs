using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "JobTrainingCenterCommonData", menuName = "ScriptableObject/JobTrainingCenterCommonData", order = 1)]
public class JobTrainingCenterCommonData : ScriptableObject
{
    [ArrayElementNamed(new string[] { "Àü»ç", "±Ã¼ö", "¸¶¹ý»ç", "°ñ·½", "Àú°Ý¼ö", "ºù°á¼ú»ç" })]
    [SerializeField]
    public int[] JobCountData;
}
