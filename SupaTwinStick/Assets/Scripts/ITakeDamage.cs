using UnityEngine;

public interface ITakeDamage {

	void TakeShell (float damageShell, Vector3 shellPoint, Vector3 shellDirection);

    void TakeDamage(float damageShell);

}
