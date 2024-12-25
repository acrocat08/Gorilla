using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleStageController : MonoBehaviour {

    public PuzzleObjectFactory factory;
    public Transform container;

    private List<PuzzleObjectView> _views;

    void Start() {
        _views = new List<PuzzleObjectView>();
    }
    
    public void Create(PuzzleStage stage) {
        foreach (StagePiece piece in stage.pieces) {
            PuzzleObjectView view = factory.Create(piece.puzzleObject);
            view.transform.SetParent(container);
            view.Place(piece.position.x, piece.position.y);
            _views.Add(view);
        }
    }

    public void Clear() {
        for (int i = 0; i < _views.Count; i++) {
            Destroy(_views[i].gameObject);
        }
        _views = new List<PuzzleObjectView>();
    }

    public List<PuzzleObjectView> GetViews() {
        return _views;
    }

}
