using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "StaticData/PuzzleObject")]
public class PuzzleObject : ScriptableObject {

    public string objectName;
    public Sprite objectImage;
}


