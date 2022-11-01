using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExcelAsset]
public class StageDataTable : ScriptableObject
{
	public List<StageNumDBEntity> StageNum; // Replace 'EntityType' to an actual type that is serializable.
	public List<StageCoordinateDBEntity> StageCoordinate; // Replace 'EntityType' to an actual type that is serializable.
	//public List<EntityType> 범례; // Replace 'EntityType' to an actual type that is serializable.
}
