using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using SoundMgr;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PuzzleMain : MonoBehaviour {

    public PuzzleObjectSelector selector;
    public PuzzleStageController stageController;
    
    
    public List<PuzzleStage> stages;

    [SerializeField] List<PuzzleObjectReaction> reactions;


    private Dictionary<HashSet<PuzzleObject>, PuzzleObjectReaction> _reactionDict;

    [SerializeField] private SceneTransition transition;

    public static int NowStageIndex = 0;
    [SerializeField] private int debugStageIndex;

    [SerializeField] private EventTrigger retryButton;
    [SerializeField] private TextMeshProUGUI titleText;
    
    
    async void Start() {
        SoundMgr.SoundPlayer.I.Play("bgm");
        if (NowStageIndex == 0) NowStageIndex = debugStageIndex;
        stageController.Create(stages[NowStageIndex]);
        MakeDict();
        foreach (PuzzleObjectView view in stageController.GetViews()) {
            view.transform.localScale = Vector3.zero;
        }

        foreach (PuzzleObjectView view in stageController.GetViews()) {
            view.Show();
            await UniTask.Delay(TimeSpan.FromSeconds(0.1f));
            SoundPlayer.I.Play("appear");

        }

        titleText.text = $"Stage {NowStageIndex + 1}";
        await MainRoutine();
    }
    
    async UniTask MainRoutine() {
        await transition.SceneStart();
        
        while (true) {
            HashSet<PuzzleObjectView> selected = await SelectPhase();
            if (selected == null) continue;
            bool success = await ReactionPhase(selected);
            if (success) {
                bool empty = await CountPhase();
                if (empty) break;
            };
        }
        SoundPlayer.I.Play("empty");
        await transition.SceneEnd();
        NowStageIndex++;
        SceneManager.LoadScene("Scenes/Puzzle");
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
        retryButton.enabled = true;
        PuzzleObjectView first = await selector.SelectObject();
        retryButton.enabled = false;

        SoundPlayer.I.Play("select");
        first.OnSelected(true);
        PuzzleObjectView second = await selector.SelectObject();
        first.OnSelected(false);
        if (first == second) {
            return null;
        }
        return new HashSet<PuzzleObjectView> {
            first, second
        };
    }

    async UniTask<bool> ReactionPhase(HashSet<PuzzleObjectView> selected) {
        HashSet<PuzzleObject> set = selected.Select(x => x.GetPuzzleObject()).ToHashSet();
        HashSet<PuzzleObject> key = _reactionDict.Keys.FirstOrDefault(x => x.SetEquals(set));
        if (key == null) {
            SoundPlayer.I.Play("nomatch");
            foreach (PuzzleObjectView view in selected) {
                view.Vive();
            }
            return false;
        }
        PuzzleObjectReaction reaction = _reactionDict[key];
        PuzzleObjectView subTarget = null;
        if (reaction.subTarget != null) {
            subTarget = stageController
                .GetViews()
                .Where(x => !selected.Contains(x))
                .FirstOrDefault(x => x.GetPuzzleObject() == reaction.subTarget);
            if (subTarget == null) return false;            
        }
        await reaction.Play(selected, subTarget, stageController);
            return true;
    }

    async UniTask<bool> CountPhase() {
        List<PuzzleObjectView> views = stageController.GetViews();
        if (views.Count == 0) return true;
        return false;
    }


    public async void Retry() {
        SoundPlayer.I.Play("retry");
        retryButton.enabled = false;
        retryButton.transform.localScale = Vector3.one * 1.5f;
        retryButton.transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.InBack);
        stageController.Clear();
        stageController.Create(stages[NowStageIndex]);
        await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
        retryButton.enabled = true;

    }
    
    
}