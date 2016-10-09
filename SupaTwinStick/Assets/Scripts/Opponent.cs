using UnityEngine;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
public class Opponent : Killable {

	NavMeshAgent navSys;
	Transform destination;

	// Use this for initialization
	protected override void Start () {
		base.Start ();
		navSys = GetComponent<NavMeshAgent> ();
		destination = GameObject.FindGameObjectWithTag("Player").transform;
		StartCoroutine(UpdatePathfinding());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	IEnumerator UpdatePathfinding() {
		float timingRefresh = 0.3f;

		while (destination != null){
			Vector3 destinationPosition = new Vector3(destination.position.x, 0, destination.position.z);
			if (!killed) {
				navSys.SetDestination (destinationPosition);
			}
			yield return new WaitForSeconds (timingRefresh);
		}
	}
}
