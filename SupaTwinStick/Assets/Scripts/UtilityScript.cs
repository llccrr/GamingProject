using System.Collections;

public static class UtilityScript {

	//this algorithme is used to shuffle the member of an array
	public static T[] MixArray<T>(T[] array, int seed){
		
		System.Random rand = new System.Random (seed);

		for (int i = 0; i < array.Length - 1; i++) {
			int rIndex = rand.Next (i, array.Length);
			T storeItem = array [rIndex];
			array [rIndex] = array [i];
			array [i] = storeItem;
		}

		return array;
	}
}
