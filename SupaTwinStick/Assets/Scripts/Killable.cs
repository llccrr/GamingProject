using UnityEngine;
using System.Collections;

public class Killable : MonoBehaviour, ITakeDamage {

	public float beginningLifePoints;
	protected float lifePoints;
	protected string status;
	protected bool killed;

    public event System.Action OnKilled;

	protected virtual void Start(){
		lifePoints = beginningLifePoints;
	}

	public void TakeShell (float damageShell, RaycastHit collision){
        TakeDamage(damageShell);
	}

    public void TakeDamage(float damageShell)
    {
        lifePoints -= damageShell;

        if (!killed && (lifePoints <= 0))
        {
            Die();
        }
    }
	public void Die (){
		killed = true;
        if(OnKilled != null)
        {
            OnKilled();
        }
		GameObject.Destroy (gameObject);
	}
}
