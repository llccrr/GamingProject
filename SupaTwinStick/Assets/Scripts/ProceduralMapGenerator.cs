using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class ProceduralMapGenerator : MonoBehaviour {

    public Transform tilePrefab;
	public Transform obstaclePrefab;
    public Vector2 mapSize;

	List<Coordinates> tilesCoordinates;
	Queue<Coordinates> mixedTilesCoordinates;
	Coordinates middleMap;

    [Range(0, 1)]
    public float tileIntersectionLine;

	[Range(0, 1)]
	public float obstacleCharge;

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

		//We get the center of the map
		middleMap = new Coordinates((int)mapSize.x/2, (int)mapSize.y/2);

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

		bool[,] allowMapObstacle = new bool[(int)mapSize.x, (int)mapSize.y];

		int numbObstacles = (int)(obstacleCharge * mapSize.x * mapSize.y);
		int numbCurrentObstacles = 0;
		for (int i = 0; i < numbObstacles; i++) {
			numbCurrentObstacles += 1;
			Coordinates randomCoordinates = GetRandomCoordinates ();
			allowMapObstacle[randomCoordinates.x, randomCoordinates.y] = true ;
			if (randomCoordinates != middleMap && IsMapAccessible (allowMapObstacle, numbCurrentObstacles)) {
				Vector3 obstaclePosition = CoordinatesOnMap (randomCoordinates.x, randomCoordinates.y);
				Transform genObstacle = Instantiate (obstaclePrefab, obstaclePosition + Vector3.up * 0.5f, Quaternion.identity) as Transform;
				genObstacle.parent = map;
				} else {
					allowMapObstacle[randomCoordinates.x, randomCoordinates.y] = false ;
				numbCurrentObstacles -= 1;
				}
		}
    }

	bool IsMapAccessible(bool[,] allowMapObstacle, int numbCurrentObstacles){
		bool[,] mapChecked = new bool[allowMapObstacle.GetLength (0), allowMapObstacle.GetLength (1)];
		Queue<Coordinates> coordQueue = new Queue<Coordinates> ();
		coordQueue.Enqueue (middleMap);
		mapChecked [middleMap.x, middleMap.y] = true;
		//on a déjà une case vide puisque le milieu est vide
		int safeTileNumber = 1;

		while (coordQueue.Count > 0) {
			Coordinates currentTile = coordQueue.Dequeue ();

			//on regarde toute les cases qui entourent la case actuelle
			for(int x = -1; x <= 1; x++)
			{
				for(int y = -1; y <= 1; y++)
				{
					int nextToX = currentTile.x + x;
					int nextToY = currentTile.y + y;
					//si on est pas en diagonale
					if (x == 0 || y == 0) {
						//si on est pas sur la case centrale et dans la map d'obstacle
						if (nextToX >= 0 && nextToX < allowMapObstacle.GetLength (0) && nextToY >= 0 && nextToY < allowMapObstacle.GetLength (1)) {
							//si la case est sur un emplacement dont on s'est occupé et que ce n'est pas un obstacle
							if (!mapChecked [nextToX, nextToY] && !allowMapObstacle [nextToX, nextToY]) {
								mapChecked [nextToX, nextToY] = true;
								//on ajoute les cases adjacentes a la queue des cases
								coordQueue.Enqueue(new Coordinates(nextToX, nextToY));
								safeTileNumber++;
							}
						}
					}
				}
			}
		}
			
		int wantedSafeTileNumber = (int)(mapSize.x * mapSize.y - numbCurrentObstacles);
		return wantedSafeTileNumber == safeTileNumber;
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

		//on définit ce qu'est une égalité entre 2 coordonnées
		public static bool operator ==(Coordinates coordA, Coordinates coordB){
			return (coordA.x == coordB.x && coordA.y == coordB.y);
		}

		//on définit ce qu'est une inégalité entre 2 coordonnées
		public static bool operator !=(Coordinates coordA, Coordinates coordB){
			return !(coordA == coordB);
		}
	}
}
