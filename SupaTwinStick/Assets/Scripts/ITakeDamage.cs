using UnityEngine;

public interface ITakeDamage {

	void TakeShell (float damageShell, RaycastHit collision);

    void TakeDamage(float damageShell);

}
