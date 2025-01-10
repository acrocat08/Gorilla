using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using SoundMgr;
using Unity.VisualScripting;
using UnityEngine;

namespace Reaction {
    [CreateAssetMenu(menuName = "Reaction/EatBanana")]
    public class EatBanana : PuzzleObjectReaction {

        public override async UniTask Play(HashSet<PuzzleObjectView> targetViews, PuzzleObjectView subTargetView, PuzzleStageController controller) {
            PuzzleObjectView gorilla = targetViews.First(x => x.GetPuzzleObject().objectName == "ゴリラ");
            PuzzleObjectView banana = targetViews.First(x => x.GetPuzzleObject().objectName == "バナナ");

            await gorilla.Goto(banana, 0.7f, 0.3f);
            SoundPlayer.I.Play("eat");
            await new List<UniTask> {
                banana.Shrink(),
                gorilla.Shake()
            };
            controller.RemoveView(banana);
            PuzzleObjectView peel = controller.AddView("バナナの皮", banana);
            PuzzleObjectView poo1 = controller.AddView("うんち", gorilla);
            PuzzleObjectView poo2 = controller.AddView("うんち", gorilla);
            SoundPlayer.I.Play("poo");
            await new List<UniTask> {
                peel.Show(),
                poo1.Show(),
                poo2.Show(),
                poo1.Move(200f * gorilla.GetDir(), 50f, 0.3f),
                poo2.Move(220f * gorilla.GetDir(), -40f, 0.3f),
                poo1.Jump(1f, 0.3f),
                poo2.Jump(1f, 0.3f),
            };
            
        }
        

    }
}