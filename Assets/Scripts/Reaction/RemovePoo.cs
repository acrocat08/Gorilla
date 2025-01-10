using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using SoundMgr;
using Unity.VisualScripting;
using UnityEngine;

namespace Reaction {
    [CreateAssetMenu(menuName = "Reaction/RemovePoo")]
    public class RemovePoo : PuzzleObjectReaction {

        public override async UniTask Play(HashSet<PuzzleObjectView> targetViews, PuzzleObjectView subTargetView, PuzzleStageController controller) {
            PuzzleObjectView poo = targetViews.First(x => x.GetPuzzleObject().objectName == "うんち");
            PuzzleObjectView bucket = targetViews.First(x => x.GetPuzzleObject().objectName == "バケツ");

            await new List<UniTask> {
                bucket.Jump(1f, 0.5f),
                bucket.Goto(poo, 1f, 0.5f)
            };
            SoundPlayer.I.Play("toilet");
            await new List<UniTask> {
                poo.Shrink(),
                bucket.Shrink()
            };
            controller.RemoveView(poo);
            controller.RemoveView(bucket);

            
        }
        

    }
}