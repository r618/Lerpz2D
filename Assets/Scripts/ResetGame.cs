using UnityEngine;
using System.Collections;

public class ResetGame : MonoBehaviour {

	void Start () {
        GameData.Instance.reset();
	}

}
