using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleObjectFactory : MonoBehaviour {
    [SerializeField] private GameObject puzzleObjectPrefab;
    
    
    public PuzzleObjectView Create(PuzzleObject puzzleObject) {
        GameObject obj = Instantiate(puzzleObjectPrefab);
        
        PuzzleObjectView view = obj.GetComponent<PuzzleObjectView>();
        view.Init(puzzleObject);
        return view;
    }
}
