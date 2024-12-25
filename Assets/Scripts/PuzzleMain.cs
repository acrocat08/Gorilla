using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class PuzzleMain : MonoBehaviour {

    public PuzzleObjectSelector selector;
    public PuzzleStageController stageController;
    
    
    public PuzzleStage stage;

    [SerializeField] List<PuzzleObjectReaction> reactions;


    private Dictionary<HashSet<PuzzleObject>, PuzzleObjectReaction> _reactionDict;
    
    
    async void Start() {
        stageController.Create(stage);
        MakeDict();
        await MainRoutine();
    }
    
    async UniTask MainRoutine() {
        while (true) {
            HashSet<PuzzleObjectView> selected = await SelectPhase();
            if (selected == null) continue;
            bool success = await ReactionPhase(selected);
            if (success) break;
        }
    }
    

    void MakeDict() {
        _reactionDict = new Dictionary<HashSet<PuzzleObject>, PuzzleObjectReaction>();
        foreach (PuzzleObjectReaction reaction in reactions) {
            HashSet<PuzzleObject> set = new HashSet<PuzzleObject> {
                reaction.targetA, reaction.targetB
            };
            _reactionDict[set] = reaction;
        }
    }


    async UniTask<HashSet<PuzzleObjectView>> SelectPhase() {
        PuzzleObjectView first = await selector.SelectObject();
        first.OnSelected(true);
        PuzzleObjectView second = await selector.SelectObject();
        first.OnSelected(false);
        if (first == second) return null;
        return new HashSet<PuzzleObjectView> {
            first, second
        };
    }

    async UniTask<bool> ReactionPhase(HashSet<PuzzleObjectView> selected) {
        HashSet<PuzzleObject> set = selected.Select(x => x.GetPuzzleObject()).ToHashSet();
        HashSet<PuzzleObject> key = _reactionDict.Keys.FirstOrDefault(x => x.SetEquals(set));
        if (key == null) return false;
        PuzzleObjectReaction reaction = _reactionDict[key];
        PuzzleObjectView subTarget = null;
        if (reaction.subTarget != null) {
            subTarget = stageController
                .GetViews()
                .Where(x => !selected.Contains(x))
                .FirstOrDefault(x => x.GetPuzzleObject() == reaction.subTarget);
            if (subTarget == null) return false;            
        }
        await reaction.Play(selected, subTarget);
        return true;
    }
    
    
    
}