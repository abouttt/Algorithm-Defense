using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JobTrainingCenterCommonData : ScriptableObject
{
    [field: SerializeField]
    public int WarriorCount { get; private set; }

    [field: SerializeField]
    public int ArcherCount { get; private set; }

    [field: SerializeField]
    public int WizardCount { get; private set; }

    [field: SerializeField]
    public int GolemCount { get; private set; }

    [field: SerializeField]
    public int SniperCount { get; private set; }

    [field: SerializeField]
    public int FreezeWizardCount { get; private set; }
}
