using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using SoundMgr;
using Unity.VisualScripting;
using UnityEngine;

namespace Reaction {
    [CreateAssetMenu(menuName = "Reaction/SlipGorilla")]
    public class SlipGorilla : PuzzleObjectReaction {

        public override async UniTask Play(HashSet<PuzzleObjectView> targetViews, PuzzleObjectView subTargetView, PuzzleStageController controller) {
            PuzzleObjectView gorilla = targetViews.First(x => x.GetPuzzleObject().objectName == "ゴリラ");
            PuzzleObjectView peel = targetViews.First(x => x.GetPuzzleObject().objectName == "バナナの皮");

            await gorilla.Goto(peel, 1f, 0.5f);
            int dir = gorilla.GetDir();
            SoundPlayer.I.Play("slip");
            await new List<UniTask> {
                peel.Drop(dir),
                gorilla.Drop(-dir)
            };
            controller.RemoveView(gorilla);
            controller.RemoveView(peel);

            
        }
        

    }
}