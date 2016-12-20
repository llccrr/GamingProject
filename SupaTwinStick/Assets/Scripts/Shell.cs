using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ITakeDamage))]
public class Shell : MonoBehaviour {

	float damageShell = 1;
	float currentSpeed = 10;
	public LayerMask collisionMask;
    public Color trailColor;
    float autoDestruction = 4;
    float safetyWidth = .1f;

    void Start()
    {
        Destroy(gameObject, autoDestruction);
        Collider[] initCollisions = Physics.OverlapSphere(transform.position, .1f, collisionMask);
        if (initCollisions.Length > 0)
        {
            OnCollision(initCollisions[0], transform.position);
        }

        GetComponent<TrailRenderer>().material.SetColor("_TintColor", trailColor);
    }
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

		if(Physics.Raycast(collisionRay, out collision, movePath + safetyWidth, collisionMask, QueryTriggerInteraction.Collide)){
			OnCollision (collision.collider, collision.point);
		}
	}


    void OnCollision(Collider c, Vector3 shellPoint)
    {
        ITakeDamage damagedObject = c.GetComponent<ITakeDamage>();
        if (damagedObject != null)
        {
            damagedObject.TakeShell(damageShell, shellPoint, transform.forward);
        }
		GameObject.Destroy(gameObject);
    }
}
