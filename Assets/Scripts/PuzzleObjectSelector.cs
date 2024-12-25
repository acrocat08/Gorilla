using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class PuzzleObjectSelector : MonoBehaviour {

    public async UniTask<PuzzleObjectView> SelectObject() {
        RaycastHit2D hit2d;
        while (true) {
            if (Input.GetMouseButtonUp(0)) {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                hit2d = Physics2D.Raycast(ray.origin, ray.direction);
                if (hit2d) break;
            }
            await UniTask.Yield();
        }
        PuzzleObjectView view = hit2d.transform.gameObject.GetComponent<PuzzleObjectView>();
        await UniTask.Yield();
        return view;
    }
    

}
