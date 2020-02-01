using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileGeneration : MonoBehaviour
{
    public Tilemap tilemap;
    public Tile[] tiles;
    private System.Random rand;
    public int X, Y;

    // Start is called before the first frame update
    void Start()
    {
        rand = new System.Random();
        for(int x=-X; x<X+1; ++x)
        {
            for(int y=-Y; y<Y+1; ++y)
            {
                tilemap.SetTile(new Vector3Int(x, y, 0), tiles[rand.Next(tiles.Length)]);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
