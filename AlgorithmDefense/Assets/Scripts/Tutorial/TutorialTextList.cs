using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TutorialTextData
{
    public List<string> TextList;
}

[CreateAssetMenu(fileName = "TutorialTextList", menuName = "ScriptableObjects/TutorialTextList", order = 1)]
public class TutorialTextList : ScriptableObject
{
    public List<TutorialTextData> TextList;
    public List<TutorialTextData> ErrorTextList;
}
