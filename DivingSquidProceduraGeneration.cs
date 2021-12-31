using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DivingSquidProceduraGeneration : MonoBehaviour{

    [SerializeField] int width, height;
    [SerializeField] float smoothness;
    [SerializeField] float seed;
    [SerializeField] TileBase groundTile;
    [SerializeField] Tilemap groundTilemap;
    int[,] map;
    
    void Start() {
        Generation();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            Generation();
        }
    }

    void Generation() {
        groundTilemap.ClearAllTiles();
        map = GenerateArray(width, height, true);
        map = TerrainGeneration(map);

        RenderMap(map, groundTilemap, groundTile);
    }

    public int[,] GenerateArray(int width, int height, bool empty) {
        int[,] map = new int[width, height];
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                map[x, y] = (empty) ? 0 : 1;
            }
        }
        return map;
    }

    public int[,] TerrainGeneration(int[,] map) {
        int perlinHeight;
        for (int x = 0; x < width; x++) {
            perlinHeight = Mathf.RoundToInt(Mathf.PerlinNoise(x / smoothness, seed) * height / 2);
            perlinHeight += height / 2;
            
            for (int y = 0; y < perlinHeight; y++) {
                map[x, y] = 1;
            }
        }
        return map;
    }

    public void RenderMap(int[,] map, Tilemap groundTilemap, TileBase groundTileBase) {
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                if (map[x, y] == 1) {
                    groundTilemap.SetTile(new Vector3Int(x,y,0), groundTileBase);
                }
            }
        }
        
    }

}
