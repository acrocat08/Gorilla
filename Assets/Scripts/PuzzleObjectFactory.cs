using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PuzzleObjectFactory : MonoBehaviour {
    [SerializeField] private GameObject puzzleObjectPrefab;
    [SerializeField] private List<PuzzleObject> puzzleObjects;
    public Transform container;
    
    public PuzzleObjectView Create(string objectName, PuzzleObjectView creator = null) {
        GameObject obj = Instantiate(puzzleObjectPrefab);
        
        PuzzleObjectView view = obj.GetComponent<PuzzleObjectView>();
        view.Init(puzzleObjects.First(x => x.objectName == objectName));
        if (creator != null) view.transform.localPosition = creator.transform.localPosition;
        view.transform.SetParent(container);
        return view;
    }
}
