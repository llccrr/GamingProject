using UnityEngine;
using System.Collections;

public class ProceduralMapGenerator : MonoBehaviour {

    public Transform tilePrefab;
    public Vector2 mapSize;

    [Range(0, 1)]
    public float tileIntersectionLine;
     
    void Start()
    {
        GenerateMap();
    }
    public void GenerateMap()
    {
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
                Vector3 tilePosition = new Vector3(-mapSize.x / 2 + 0.5f + x, 0, -mapSize.y/2 + 0.5f + y);
                Transform newTile = (Transform)Instantiate(tilePrefab, tilePosition, Quaternion.Euler(Vector3.right * 90));
                newTile.localScale = Vector3.one * (1- tileIntersectionLine);
                newTile.parent = map;
            }
        }
    }
}
