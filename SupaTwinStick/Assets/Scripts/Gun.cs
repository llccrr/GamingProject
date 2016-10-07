using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour {

	public Transform barrelEnd;
	public Shell shell;
	public float fireRate = 100;// en milliseconde
	public float barrelEndVelocity = 35;

	float fireRateLimiter;

	public void Fire(){
		// on empeche de tirer au de la du firerate inscrit
		if (Time.time > fireRateLimiter) {
			fireRateLimiter = Time.time + fireRateLimiter / 1000;
			Shell newShell = Instantiate (shell, barrelEnd.position, barrelEnd.rotation) as Shell;
			newShell.ResetSpeed (barrelEndVelocity);
		}
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


}
