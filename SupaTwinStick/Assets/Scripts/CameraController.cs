using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public Player player;

	Vector3 gap;

	// Use this for initialization
	void Start () {
		gap = transform.position - player.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = player.transform.position + gap;
	}
}
