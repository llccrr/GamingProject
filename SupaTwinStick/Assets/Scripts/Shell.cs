using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ITakeDamage))]
public class Shell : MonoBehaviour {

	float damageShell = 1;
	float currentSpeed = 10;
	public LayerMask collisionMask;

	public void ResetSpeed(float newSpeed){
		currentSpeed = newSpeed;
	}

	// Update is called once per frame
	void Update () {
		float movePath = currentSpeed * Time.deltaTime;
		CollisionDetection (movePath);
		transform.Translate (Vector3.forward * Time.deltaTime * currentSpeed);
	}

	void CollisionDetection(float movePath) {
		Ray collisionRay = new Ray (transform.position, transform.forward);
		RaycastHit collision;

		if(Physics.Raycast(collisionRay, out collision, movePath, collisionMask, QueryTriggerInteraction.Collide)){
			OnCollision (collision);
		}
	}

	void OnCollision ( RaycastHit collision){
		ITakeDamage damagedObject = collision.collider.GetComponent<ITakeDamage> ();
		if (damagedObject != null) {
			damagedObject.TakeShell (damageShell, collision);
		}
		GameObject.Destroy (gameObject);
	}
}
