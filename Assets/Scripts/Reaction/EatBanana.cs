using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Reaction {
    [CreateAssetMenu(menuName = "Reaction/EatBanana")]
    public class EatBanana : PuzzleObjectReaction {

        public override async UniTask Play(HashSet<PuzzleObjectView> targetViews, PuzzleObjectView subTargetView) {
            PuzzleObjectView gorilla = targetViews.First(x => x.GetPuzzleObject().objectName == "ゴリラ");
            PuzzleObjectView banana = targetViews.First(x => x.GetPuzzleObject().objectName == "バナナ");

            await gorilla.Goto(banana, 1, 0.3f);
        }

    }
}