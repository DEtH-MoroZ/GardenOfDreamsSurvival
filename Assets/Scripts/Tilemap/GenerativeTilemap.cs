using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GenerativeTilemap : MonoBehaviour
{
    public Transform tilePrefab;

    private int tileMapSize = 200;

    private void Start()
    {
       TileSpawn();
    }

    async UniTaskVoid TileSpawn()
    {
        for (int x = -tileMapSize/2; x < tileMapSize/2; x++)
        {
            for (int y = -tileMapSize / 2; y < tileMapSize/2; y++)
            {
                Instantiate(tilePrefab, new Vector3(x, y, 0f), Quaternion.identity, transform);
                if (x % 100 == 0)
                {
                    await UniTask.WaitForEndOfFrame(this);
                }
            }
        }
    }
}
