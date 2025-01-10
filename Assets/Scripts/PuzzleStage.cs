using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "StaticData/PuzzleStage")]
public class PuzzleStage : ScriptableObject {
    public List<StagePiece> pieces;
}


[Serializable]
public class StagePiece {
    public string objectName;
    public Vector2 position;
}
