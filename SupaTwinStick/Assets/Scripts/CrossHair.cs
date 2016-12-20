using UnityEngine;
using System.Collections;

public class CrossHair : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Cursor.visible = false;
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate (Vector3.forward * 50 * Time.deltaTime);
	}
}
