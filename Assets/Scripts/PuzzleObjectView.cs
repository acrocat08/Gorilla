using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class PuzzleObjectView : MonoBehaviour {

    private PuzzleObject _puzzleObject;
    private SpriteRenderer _sp;
    [SerializeField] private Material selectedMaterial;
    [SerializeField] private Material notSelectedMaterial;

    public void Init(PuzzleObject puzzleObject) {
        _sp = transform.Find("Image").GetComponent<SpriteRenderer>();
        _puzzleObject = puzzleObject;
        _sp.sprite = _puzzleObject.objectImage;
    }

    public PuzzleObject GetPuzzleObject() {
        return _puzzleObject;
    }

    public void Place(float x, float y) {
        float screenWidth = 1920f;
        float screenHeight = 1080f;
        transform.localPosition = new Vector3(x - screenWidth / 2, -y + screenHeight / 2) * 0.01f;
        _sp.sortingOrder = (int)y;
    }

    public void OnSelected(bool isSelected) {
        _sp.material = isSelected ? selectedMaterial : notSelectedMaterial;
    }

    public async UniTask Goto(PuzzleObjectView opponent, float rate, float duration) {
        Vector3 localPosition = transform.localPosition;
        Vector2 toPos = localPosition + (opponent.transform.localPosition - localPosition) * rate;
        transform.DOLocalMove(toPos, duration).SetEase(Ease.Linear);
        await UniTask.Delay(TimeSpan.FromSeconds(duration));
        _sp.sortingOrder = (int)localPosition.y;
    }

}
