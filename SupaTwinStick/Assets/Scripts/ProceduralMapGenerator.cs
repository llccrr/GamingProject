using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class ProceduralMapGenerator : MonoBehaviour {

    public Transform tilePrefab;
	public Transform obstaclePrefab;
    public Vector2 mapSize;

	List<Coordinates> tilesCoordinates;
	Queue<Coordinates> mixedTilesCoordinates;

    [Range(0, 1)]
    public float tileIntersectionLine;

	public int seed = 10;
     
    void Start()
    {
        GenerateMap();
    }
    public void GenerateMap()
    {
		tilesCoordinates = new List<Coordinates> ();

		for(int x = 0; x <mapSize.x; x++)
		{
			for (int y = 0; y < mapSize.y; y++)
			{
				tilesCoordinates.Add (new Coordinates (x, y));
			}
		}
		mixedTilesCoordinates = new Queue<Coordinates> (UtilityScript.MixArray (tilesCoordinates.ToArray (), seed));

        //Name of the folder which contain every generated tile.
        string name = "Procedural Map";
        if(transform.FindChild(name))
        {
            DestroyImmediate(transform.FindChild(name).gameObject);
        }

        Transform map = new GameObject(name).transform;
        map.parent = transform;

        for(int x = 0; x <mapSize.x; x++)
        {
            for (int y = 0; y < mapSize.y; y++)
            {
				Vector3 tilePosition = CoordinatesOnMap(x, y);
                Transform newTile = (Transform)Instantiate(tilePrefab, tilePosition, Quaternion.Euler(Vector3.right * 90));
                newTile.localScale = Vector3.one * (1- tileIntersectionLine);
                newTile.parent = map;
            }
        }

		int numbObstacles = 10;
		for (int i = 0; i < numbObstacles; i++) {
			Coordinates randomCoordinates = GetRandomCoordinates ();
			Vector3 obstaclePosition = CoordinatesOnMap(randomCoordinates.x, randomCoordinates.y);
			Transform genObstacle = Instantiate (obstaclePrefab, obstaclePosition + Vector3.up * 0.5f, Quaternion.identity) as Transform;
			genObstacle.parent = map;
		}
    }

	Vector3 CoordinatesOnMap(int x, int y){
		return new Vector3(-mapSize.x / 2 + 0.5f + x, 0, -mapSize.y/2 + 0.5f + y);
	}

	public Coordinates GetRandomCoordinates (){
		Coordinates randomCoordinates = mixedTilesCoordinates.Dequeue ();
		mixedTilesCoordinates.Enqueue (randomCoordinates);
		return randomCoordinates;
	}

	public struct Coordinates {
		public int x;
		public int y;

		public Coordinates(int _x, int _y) {
			x = _x;
			y = _y;
		}

	}
}
