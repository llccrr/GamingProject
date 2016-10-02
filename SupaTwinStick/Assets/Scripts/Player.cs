using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PlayerController))]
public class Player : MonoBehaviour {

    public float moveSpeed = 5;

    PlayerController myPlayerController;
    Camera viewCamera;

    // Use this for initialization    
    void Start () {
        myPlayerController = GetComponent<PlayerController>();
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
    }
}
