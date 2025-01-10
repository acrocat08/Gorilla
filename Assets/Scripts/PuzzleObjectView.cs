using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class PuzzleObjectView : MonoBehaviour {

    private PuzzleObject _puzzleObject;
    private SpriteRenderer _sp;
    [SerializeField] private Material selectedMaterial;
    [SerializeField] private Material notSelectedMaterial;

    private float _screenWidth = 1920f;
    private float _screenHeight = 1080f;

    public void Init(PuzzleObject puzzleObject) {
        _sp = transform.Find("Image").GetComponent<SpriteRenderer>();
        _puzzleObject = puzzleObject;
        _sp.sprite = _puzzleObject.objectImage;
        transform.Find("Shadow").GetComponent<SpriteRenderer>().sprite = _puzzleObject.objectImage;
        transform.Find("Shadow").localPosition = Vector3.up * (-1 + _puzzleObject.shadowPos);
    }

    public PuzzleObject GetPuzzleObject() {
        return _puzzleObject;
    }

    public void Place(float x, float y) {
        transform.localPosition = new Vector3(x - _screenWidth / 2, -y + _screenHeight / 2) * 0.01f;
        _sp.sortingOrder = -(int)(transform.localPosition.y * 100);
    }

    public void OnSelected(bool isSelected) {
        _sp.material = isSelected ? selectedMaterial : notSelectedMaterial;
    }

    public async UniTask Goto(PuzzleObjectView opponent, float rate, float duration) {
        Vector3 localPosition = transform.localPosition;
        Vector3 toDir = opponent.transform.localPosition - localPosition;
        Vector2 toPos = localPosition + toDir * rate;
        transform.DOLocalMove(toPos, duration).SetEase(Ease.Linear);
        ChangeDir(toDir.x > 0);
        await UniTask.Delay(TimeSpan.FromSeconds(duration));
        _sp.sortingOrder = -(int)(localPosition.y * 100);
    }

    public async UniTask Move(float x, float y, float duration) {
        Vector3 localPosition = transform.localPosition;
        Vector3 toPos = localPosition + new Vector3(x , -y) * 0.01f;
        transform.DOLocalMove(toPos, duration).SetEase(Ease.Linear);
        ChangeDir((toPos - localPosition).x > 0);
        await UniTask.Delay(TimeSpan.FromSeconds(duration));
        _sp.sortingOrder = -(int)(localPosition.y * 100);
    }

    public async UniTask Jump(float length, float duration) {
        _sp.transform.DOLocalJump(Vector3.zero, length, 1, duration).SetEase(Ease.Linear);
        await UniTask.Delay(TimeSpan.FromSeconds(duration));
    }
    
    public async UniTask Drop(int dir) {
        float duration = 1f;
        float xOffset = 2f;
        transform.Find("Shadow").gameObject.SetActive(false);
        transform.DOLocalJump(
            transform.localPosition + Vector3.down * _screenHeight * 0.01f + Vector3.right * dir * xOffset, 10f, 1,
            duration);
        _sp.transform.DOLocalRotate(new Vector3(0f, 0f, Random.Range(-180, 180)), duration).SetEase(Ease.Linear);
        await UniTask.Delay(TimeSpan.FromSeconds(duration));
    }
    
    public async UniTask Shrink() {
        float duration = 0.5f;
        //_sp.sortingOrder = 10000;
        transform.DOScale(0f, duration).SetEase(Ease.Linear);
        _sp.transform.DOLocalRotate(new Vector3(0f, 0f, 270f), duration).SetEase(Ease.Linear);
        await UniTask.Delay(TimeSpan.FromSeconds(duration));
        _sp.sortingOrder = -(int)(transform.localPosition.y * 100);

    }
    public async UniTask Show() {
        float duration = 0.3f;
        //_sp.sortingOrder = 10000;
        transform.localScale = Vector3.zero;
        transform.DOScale(1f, duration).SetEase(Ease.Linear);
        await UniTask.Delay(TimeSpan.FromSeconds(duration));
        _sp.sortingOrder = -(int)(transform.localPosition.y * 100);

        
    }
    
    public async UniTask Shake() {
        float duration = 0.5f;
        _sp.transform.DOLocalRotate(new Vector3(0f, 0f, 20f), duration / 6f).SetLoops(6, LoopType.Yoyo);
        await UniTask.Delay(TimeSpan.FromSeconds(duration));
    }
    public async UniTask Vive() {
        float duration = 0.3f;
        _sp.transform.localPosition += Vector3.right * 0.2f;
        _sp.transform.DOLocalMoveX(_sp.transform.localPosition.x - 0.4f, duration / 6f).SetLoops(6, LoopType.Yoyo)
            .SetEase(Ease.Linear);
        await UniTask.Delay(TimeSpan.FromSeconds(duration));
        _sp.transform.localPosition -= Vector3.right * 0.2f;
    }

    private void ChangeDir(bool isRight) {
        if (!_puzzleObject.hasLife) return;
        transform.localScale = new Vector3(isRight ? -1 : 1, 1, 0);
    }

    public int GetDir() {
        return (int)transform.localScale.x;
    }

}
