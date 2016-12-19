using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour {

	public Transform barrelEnd;
	public Shell shell;
	public float fireRate = 100000;// en milliseconde
	public float barrelEndVelocity = 35;
    float fireRateLimiter;
    public Transform bullet;
    public Transform bulletEjection;
    FlashPowa flashPowa;

    [Header("Effects")]
    public AudioClip fireAudio;
    //public AudioClip reloadAudio;

	
    void Start()
    {
        flashPowa = GetComponent<FlashPowa>();
    }
	public void Fire(){
		// on empeche de tirer au de la du firerate inscrit
		if (Time.time > fireRateLimiter) {
			fireRateLimiter = Time.time + fireRate / 1000;
			Shell newShell = Instantiate (shell, barrelEnd.position, barrelEnd.rotation) as Shell;
			newShell.ResetSpeed (barrelEndVelocity);

            Instantiate(bullet, bulletEjection.position, bulletEjection.rotation);
            flashPowa.Activate();

            AudioManager.instance.PlaySound(fireAudio, transform.position);
		}
	}


	
	// Update is called once per frame
	void Update () {
	
	}


}
