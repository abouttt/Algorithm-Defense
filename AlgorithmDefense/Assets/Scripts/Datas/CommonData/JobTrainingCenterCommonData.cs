using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "JobTrainingCenterCommonData", menuName = "ScriptableObject/JobTrainingCenterCommonData", order = 1)]
public class JobTrainingCenterCommonData : ScriptableObject
{
    [ArrayElementNamed(new string[] { "����", "�ü�", "������", "��", "���ݼ�", "�������" })]
    [SerializeField]
    public int[] JobCountData;
}
