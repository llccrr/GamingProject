using UnityEngine;
using System.Collections;

public class Killable : MonoBehaviour, ITakeDamage {

	public float beginningLifePoints;
	public float lifePoints{get; protected set;}
	protected string status;
	protected bool killed;

    public event System.Action OnKilled;

	protected virtual void Start(){
		lifePoints = beginningLifePoints;
	}

	public virtual void TakeShell (float damageShell, Vector3 shellPoint, Vector3 shellDirection){
        TakeDamage(damageShell);
	}

    public virtual void TakeDamage(float damageShell)
    {
        lifePoints -= damageShell;

        if (!killed && (lifePoints <= 0))
        {
            Die();
        }
    }

    [ContextMenu("Self Destruct")]
    public virtual void Die()
    {
        killed = true;
        if (OnKilled != null)
        {
            OnKilled();
        }
        GameObject.Destroy(gameObject);
    }
}
