using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using SoundMgr;
using Unity.VisualScripting;
using UnityEngine;

namespace Reaction {
    [CreateAssetMenu(menuName = "Reaction/ThrowPoo")]
    public class ThrowPoo : PuzzleObjectReaction {

        public override async UniTask Play(HashSet<PuzzleObjectView> targetViews, PuzzleObjectView subTargetView, PuzzleStageController controller) {
            PuzzleObjectView gorilla = targetViews.First(x => x.GetPuzzleObject().objectName == "ゴリラ");
            PuzzleObjectView poo = targetViews.First(x => x.GetPuzzleObject().objectName == "うんち");
            PuzzleObjectView human = subTargetView;

            await gorilla.Goto(poo, 0.8f, 0.5f);
            int dir = human.GetDir();
            SoundPlayer.I.Play("slip");
            await new List<UniTask> {
                peel.Drop(dir),
                human.Drop(-dir)
            };
            controller.RemoveView(human);
            controller.RemoveView(peel);

            
        }
        

    }
}