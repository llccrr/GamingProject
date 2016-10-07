using UnityEngine;
using System.Collections;

public class GunController : MonoBehaviour {

	public Transform gunHold;
	public Gun startGun;
	Gun equippedGun;

	void Start() {
		if (startGun != null) {
			EquipGun(startGun);
		}
	}

	//Se munir l'arme
	public void EquipGun(Gun gunToEquip){
		if (equippedGun != null) {
			Destroy (equippedGun.gameObject);
		}
		equippedGun = Instantiate (gunToEquip, gunHold.position, gunHold.rotation) as Gun;
		equippedGun.transform.parent = gunHold;
	}

	public void Shoot(){
		if (equippedGun != null) {
			equippedGun.Fire ();
		}
	}
}
