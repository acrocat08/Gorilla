using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using SoundMgr;
using Unity.VisualScripting;
using UnityEngine;

namespace Reaction {
    [CreateAssetMenu(menuName = "Reaction/MakeBucket")]
    public class MakeBucket : PuzzleObjectReaction {

        public override async UniTask Play(HashSet<PuzzleObjectView> targetViews, PuzzleObjectView subTargetView, PuzzleStageController controller) {
            PuzzleObjectView human = targetViews.First(x => x.GetPuzzleObject().objectName == "人間");
            PuzzleObjectView tree = targetViews.First(x => x.GetPuzzleObject().objectName == "木");

            await human.Goto(tree, 0.8f, 0.3f);
            SoundPlayer.I.Play("craft");
            await new List<UniTask> {
                human.Shake()
            };
            controller.RemoveView(tree);
            PuzzleObjectView bucket = controller.AddView("バケツ", tree);
            await new List<UniTask> {
                bucket.Show(),
                bucket.Jump(1f, 0.3f),
            };
            
        }
        

    }
}