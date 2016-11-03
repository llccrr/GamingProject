using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class ProceduralMapGenerator : MonoBehaviour {

	public Map[] maps;
	public int indexOfMap;

    public Transform tilePrefab;
	public Transform obstaclePrefab;
	public Transform naveMeshMap;
	public Transform navMeshMapLimiterPrefab;
	public Vector2 maxSizeOfMap;

	List<Coordinates> tilesCoordinates;
	Queue<Coordinates> mixedTilesCoordinates;

    [Range(0, 1)]
    public float tileIntersectionLine;

	public float sizeOfTile;

	Map currentMap;
     
    void Start()
    {
        GenerateMap();
    }
    public void GenerateMap()
    {
		currentMap = maps [indexOfMap];
		GetComponent<BoxCollider> ().size = new Vector3 (currentMap.sizeOfMap.x * sizeOfTile, .04f, currentMap.sizeOfMap.y * sizeOfTile);

		//Génération des coordonées
		tilesCoordinates = new List<Coordinates> ();

		for(int x = 0; x <currentMap.sizeOfMap.x; x++)
		{
			for (int y = 0; y < currentMap.sizeOfMap.y; y++)
			{
				tilesCoordinates.Add (new Coordinates (x, y));
			}
		}
		mixedTilesCoordinates = new Queue<Coordinates> (UtilityScript.MixArray (tilesCoordinates.ToArray (), currentMap.seed));


        //Créer l'object contenant la map et nomme le fichier qui contiendra chaque case.
        string name = "Procedural Map";
        if(transform.FindChild(name))
        {
            DestroyImmediate(transform.FindChild(name).gameObject);
        }

        Transform map = new GameObject(name).transform;
        map.parent = transform;

		//Génère les cases
		for(int x = 0; x <currentMap.sizeOfMap.x; x++)
        {
			for (int y = 0; y < currentMap.sizeOfMap.y; y++)
            {
				Vector3 tilePosition = CoordinatesOnMap(x, y);
                Transform newTile = (Transform)Instantiate(tilePrefab, tilePosition, Quaternion.Euler(Vector3.right * 90));
				newTile.localScale = Vector3.one * (1- tileIntersectionLine) * sizeOfTile;
                newTile.parent = map;
            }
        }

		//Génère les obstacles
		bool[,] allowMapObstacle = new bool[(int)currentMap.sizeOfMap.x, (int)currentMap.sizeOfMap.y];
		int numbObstacles = (int)(currentMap.obstacleCharge * currentMap.sizeOfMap.x * currentMap.sizeOfMap.y);
		int numbCurrentObstacles = 0;

		for (int i = 0; i < numbObstacles; i++) {
			numbCurrentObstacles += 1;
			Coordinates randomCoordinates = GetRandomCoordinates ();
			allowMapObstacle[randomCoordinates.x, randomCoordinates.y] = true ;

			if (randomCoordinates != currentMap.middleMap && IsMapAccessible (allowMapObstacle, numbCurrentObstacles)) {
				Vector3 obstaclePosition = CoordinatesOnMap (randomCoordinates.x, randomCoordinates.y);
				Transform genObstacle = Instantiate (obstaclePrefab, obstaclePosition + Vector3.up * 0.5f, Quaternion.identity) as Transform;
				genObstacle.parent = map;
				genObstacle.localScale = Vector3.one * (1- tileIntersectionLine) * sizeOfTile;

				Renderer rendererOfObstacle = genObstacle.GetComponent<Renderer> ();
				Material materialOfObstacle = new Material (rendererOfObstacle.sharedMaterial);
				float gradiant = randomCoordinates.y / (float)currentMap.sizeOfMap.y;
				materialOfObstacle.color = Color.Lerp (currentMap.foreGroundColor, currentMap.backGroundColor, gradiant);
				rendererOfObstacle.sharedMaterial = materialOfObstacle;
			
			} else {
					allowMapObstacle[randomCoordinates.x, randomCoordinates.y] = false ;
				numbCurrentObstacles -= 1;
			}
		}

		//Créer les limites du pathfinding de la map
		//Limiteur du NavMesh du coté gauche
		Transform mapLimiterLeft = Instantiate (navMeshMapLimiterPrefab, Vector3.left * (maxSizeOfMap.x + currentMap.sizeOfMap.x) / 4f * sizeOfTile, Quaternion.identity) as Transform;
		mapLimiterLeft.parent = map;
		mapLimiterLeft.localScale = new Vector3 ((maxSizeOfMap.x - currentMap.sizeOfMap.x) / 2f, 1, currentMap.sizeOfMap.y) * sizeOfTile;
		//Limiteur du NavMesh du coté droit
		Transform mapLimiterRight = Instantiate (navMeshMapLimiterPrefab, Vector3.right * (maxSizeOfMap.x + currentMap.sizeOfMap.x) / 4f * sizeOfTile, Quaternion.identity) as Transform;
		mapLimiterRight.parent = map;
		mapLimiterRight.localScale = new Vector3 ((maxSizeOfMap.x - currentMap.sizeOfMap.x) / 2f, 1, currentMap.sizeOfMap.y) * sizeOfTile;
		//Limiteur du NavMesh du coté haut
		Transform mapLimiterUp = Instantiate (navMeshMapLimiterPrefab, Vector3.forward * (maxSizeOfMap.y + currentMap.sizeOfMap.y) / 4f * sizeOfTile, Quaternion.identity) as Transform;
		mapLimiterUp.parent = map;
		mapLimiterUp.localScale = new Vector3 (maxSizeOfMap.x , 1, (maxSizeOfMap.y - currentMap.sizeOfMap.y) / 2f) * sizeOfTile;
		//Limiteur du NavMesh du coté bas
		Transform mapLimiterDown = Instantiate (navMeshMapLimiterPrefab, Vector3.back * (maxSizeOfMap.y + currentMap.sizeOfMap.y) / 4f * sizeOfTile, Quaternion.identity) as Transform;
		mapLimiterDown.parent = map;
		mapLimiterDown.localScale = new Vector3 (maxSizeOfMap.x , 1, (maxSizeOfMap.y - currentMap.sizeOfMap.y) / 2f) * sizeOfTile;

		naveMeshMap.localScale = new Vector3 (maxSizeOfMap.x, maxSizeOfMap.y * sizeOfTile);
    }

	bool IsMapAccessible(bool[,] allowMapObstacle, int numbCurrentObstacles){
		bool[,] mapChecked = new bool[allowMapObstacle.GetLength (0), allowMapObstacle.GetLength (1)];
		Queue<Coordinates> coordQueue = new Queue<Coordinates> ();
		coordQueue.Enqueue (currentMap.middleMap);
		mapChecked [currentMap.middleMap.x, currentMap.middleMap.y] = true;
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
			
		int wantedSafeTileNumber = (int)(currentMap.sizeOfMap.x * currentMap.sizeOfMap.y - numbCurrentObstacles);
		return wantedSafeTileNumber == safeTileNumber;
	}

	Vector3 CoordinatesOnMap(int x, int y){
		return new Vector3(-currentMap.sizeOfMap.x / 2f + 0.5f + x, 0, -currentMap.sizeOfMap.y / 2f + 0.5f + y)* sizeOfTile;
	}

	public Coordinates GetRandomCoordinates (){
		Coordinates randomCoordinates = mixedTilesCoordinates.Dequeue ();
		mixedTilesCoordinates.Enqueue (randomCoordinates);
		return randomCoordinates;
	}

	[System.Serializable]
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

	[System.Serializable]
	public class Map{

		public Coordinates sizeOfMap;
		[Range(0,1)]
		public float obstacleCharge;
		public int seed;
		public Color foreGroundColor;
		public Color backGroundColor;

		public Coordinates middleMap {
			get{
				return new Coordinates (sizeOfMap.x / 2, sizeOfMap.x / 2);
			}
		}
	}
}
