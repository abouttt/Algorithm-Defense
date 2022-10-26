using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Wave", menuName = "ScriptableObject/Wave", order = 1)]
public class WaveSystem : ScriptableObject
{
    [field: SerializeField]
    public GameObject[] monsterPrefab { get; private set; }
    [field: SerializeField]
    public float TimeBeforeThisWave { get; private set; }
    [field: SerializeField]
    public float NumberToSpawn { get; private set; }
}
