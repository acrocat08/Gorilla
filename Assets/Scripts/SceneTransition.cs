using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class SceneTransition : MonoBehaviour {

    [SerializeField] private RectTransform header;
    [SerializeField] private RectTransform empty;

    public async UniTask SceneStart() {
        float duration = 0.5f;
        header.localPosition = new Vector3(0, 650f, 0);
        header.DOLocalMoveY(450f, duration).SetEase(Ease.OutQuart);
        await UniTask.Delay(TimeSpan.FromSeconds(duration));
    }
    
    public async UniTask SceneEnd() {
        float duration = 0.5f;
        header.localPosition = new Vector3(0, 450f, 0);
        header.DOLocalMoveY(650f, duration).SetEase(Ease.OutQuart);
        
        TextMeshProUGUI tmp = empty.GetComponent<TextMeshProUGUI>();
        tmp.DOFade(0, 0);
        DOTweenTMPAnimator animator = new DOTweenTMPAnimator(tmp);
        for (int i = 0; i < animator.textInfo.characterCount; ++i) {
            animator.DOScaleChar(i, 0.7f, 0);
            Vector3 currCharOffset = animator.GetCharOffset(i);
            DOTween.Sequence()
                .Append(animator.DOOffsetChar(i, currCharOffset + new Vector3(0, 30, 0), 0.4f).SetEase(Ease.OutFlash, 2))
                .Join(animator.DOFadeChar(i, 1, 0.4f))
                .Join(animator.DOScaleChar(i, 1, 0.4f).SetEase(Ease.OutBack))
                .SetDelay(0.07f * i);
        }
        await UniTask.Delay(TimeSpan.FromSeconds(2f));
        for (int i = 0; i < animator.textInfo.characterCount; ++i) {
            animator.DOFadeChar(i, 0, 0.3f);
        }
        await UniTask.Delay(TimeSpan.FromSeconds(1f));

    }
    
    

}
