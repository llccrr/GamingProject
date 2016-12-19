using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

    public Rigidbody myRigidbody;
    public float forceMin;
    public float forceMax;

    float lifeTime = 4;
    float fadeTime = 2;
	// Use this for initialization
	void Start () {
        float force = Random.Range(forceMin, forceMax);
        myRigidbody.AddForce(transform.right * force);
        myRigidbody.AddTorque(Random.insideUnitSphere * force);

        StartCoroutine(Fade());
	}
	
    IEnumerator Fade()
    {
        yield return new WaitForSeconds(lifeTime);
        float percent = 0;
        float fadeSpeed = 1 / fadeTime;
        Material mat = GetComponent<Renderer>().material;
        Color initialColor = mat.color;

        while(percent <1)
        {
            percent += Time.deltaTime * fadeSpeed;
            mat.color = Color.Lerp(initialColor, Color.clear, percent);
            yield return null;
        }
        Destroy(gameObject);
    }
	// Update is called once per frame
	void Update () {
	
	}
}
