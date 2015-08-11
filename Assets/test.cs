using UnityEngine;
using System.Collections;

public class test : MonoBehaviour {

	void OnGUI () {
		GUI.Label (new Rect(Screen.width/2,0,100,200), "Jenkins Test Success");
	}
}
