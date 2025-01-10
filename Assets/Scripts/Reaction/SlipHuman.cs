using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using SoundMgr;
using Unity.VisualScripting;
using UnityEngine;

namespace Reaction {
    [CreateAssetMenu(menuName = "Reaction/SlipHuman")]
    public class SlipHuman : PuzzleObjectReaction {

        public override async UniTask Play(HashSet<PuzzleObjectView> targetViews, PuzzleObjectView subTargetView, PuzzleStageController controller) {
            PuzzleObjectView human = targetViews.First(x => x.GetPuzzleObject().objectName == "人間");
            PuzzleObjectView peel = targetViews.First(x => x.GetPuzzleObject().objectName == "バナナの皮");

            await human.Goto(peel, 1f, 0.5f);
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