using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(GunController))]
public class Player : Killable {

    public float moveSpeed = 5;

    PlayerController myPlayerController;
    Camera viewCamera;
	GunController gunController;

    // Use this for initialization    
	protected override void Start () {
		base.Start ();
        myPlayerController = GetComponent<PlayerController>();
		gunController = GetComponent<GunController>();
        viewCamera = Camera.main;
    }
	
	// Update is called once per frame
	void Update () {

        /*Manage the movement of the player */
        Vector3 moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        Vector3 moveVelocity = moveInput.normalized * moveSpeed;
        myPlayerController.Move(moveVelocity);

        /* Manage the camera to look at the mouse cursor */
        Ray ray = viewCamera.ScreenPointToRay(Input.mousePosition);
        Plane myPlane = new Plane(Vector3.up, Vector3.zero);
        float rayDistance;

        if(myPlane.Raycast(ray, out rayDistance))
        {
            Vector3 point = ray.GetPoint(rayDistance);
            myPlayerController.LookAt(point);
        }

		/* Manage the weaponing and shooting system*/
		if (Input.GetMouseButton (0)) {
			gunController.Shoot ();
		}
    }

    public override void Die()
    {
        AudioManager.instance.PlaySound("Player Death", transform.position);
        base.Die();
    }
}
