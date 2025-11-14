using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor.U2D;
using UnityEngine;


public class GenerativeTilemap : MonoBehaviour
{
    public Transform tilePrefab;

    public Sprite[] spritesCollection;
    

    private int tileMapSize = 50;

    private void Start()
    {
       TileSpawn().Forget();

    }

    async UniTaskVoid TileSpawn()
    {
        
        for (int x = 0; x < tileMapSize; x++)
        {
            for (int y = 0 ; y < tileMapSize; y++)
            {
                
                tilePrefab.GetComponent<SpriteRenderer>().sprite = spritesCollection[GetImprovedTileValue(x,y)];
                

                Instantiate(tilePrefab, new Vector3(x - tileMapSize/2, y - tileMapSize/2, 0f), Quaternion.identity, transform);
                if (x % 100 == 0)
                {
                    await UniTask.WaitForEndOfFrame(this);
                }
            }
        }
    }

    public int GetImprovedTileValue(float x, float y)
    {
        // Parameters to tweak
        float scale = 0.1f;
        int octaves = 3;
        float persistence = 0.5f;
        float lacunarity = 2.0f;
        float power = 1.5f;

        // Multi-octave noise
        float value = 0;
        float amplitude = 1;
        float frequency = scale;
        float maxValue = 0;

        for (int i = 0; i < octaves; i++)
        {
            value += Mathf.PerlinNoise(x * frequency, y * frequency) * amplitude;
            maxValue += amplitude;
            amplitude *= persistence;
            frequency *= lacunarity;
        }

        value /= maxValue;

        // Increase contrast
        value = Mathf.Pow(value, power);

        // Apply thresholds for better distribution
        if (value < 0.15f) return 0;
        if (value < 0.35f) return 1;
        if (value < 0.65f) return 2;
        if (value < 0.85f) return 3;
        return 4;
    }
}
