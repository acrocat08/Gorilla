using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class PuzzleObjectReaction : ScriptableObject {

    public PuzzleObject targetA;
    public PuzzleObject targetB;
    public PuzzleObject subTarget;

    public virtual async UniTask Play(HashSet<PuzzleObjectView> targetViews, PuzzleObjectView subTargetView) {
        
    }

}
