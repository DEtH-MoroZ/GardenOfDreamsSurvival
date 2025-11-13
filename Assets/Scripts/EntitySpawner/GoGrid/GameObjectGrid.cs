using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class GameObjectGrid //class to hold information about objects. nessesarry for optimizations, that makes it all possible
{
    float _gridSideLength = 10f;
    float _gridStep = 0.2f; //size of a cell
    readonly int _celldeviance = 2; //ammount of cells, that would be checked around target

    int _gridSideCellCount;

    private List<GameObject>[][] GameObjectGridList;
    public GameObjectGrid (float gridSideLength, float gridStep)
    {        
        if (gridSideLength < 0 )
        {
            Debug.LogError("[CoinGrid] Grid Side Length must be more than zero.");
        }
        _gridSideLength = gridSideLength;
        if (gridStep < 0)
        {
            Debug.LogError("[CoinGrid] Grid Step must be more than zero.");
        }
        _gridStep = gridStep;

        _gridSideCellCount = Mathf.FloorToInt(_gridSideLength / _gridStep);

        GameObjectGridList = new List<GameObject>[_gridSideCellCount][];

        for (int a = 0; a < _gridSideCellCount; a++)
        {
            GameObjectGridList[a] = new List<GameObject>[_gridSideCellCount];
            for (int b = 0; b < _gridSideCellCount; b++)
            {
                GameObjectGridList[a][b] = new List<GameObject>();
            }
        }
    }




    public void Add(GameObject go)
    {
        if (GameObjectGridList == null)
        {
            Debug.LogWarning("[CoinGrid] No coin grid presented!");
            return;
        }

        int posX = GameObjectGridHelpers.WorldToGridIndices(go.transform.position.x, _gridSideLength, _gridStep);
        int posZ = GameObjectGridHelpers.WorldToGridIndices(go.transform.position.z, _gridSideLength, _gridStep);

        GameObjectGridList[posX][posZ].Add(go);
    }

    public void Remove(GameObject go) 
    {
        if (GameObjectGridList == null)
        {
            Debug.LogWarning("[CoinGrid] No coin grid presented!");
            return;
        }
        

        int posX = GameObjectGridHelpers.WorldToGridIndices(go.transform.position.x, _gridSideLength, _gridStep);
        int posZ = GameObjectGridHelpers.WorldToGridIndices(go.transform.position.z, _gridSideLength, _gridStep);

        for (int a = posX - _celldeviance; a < posX + _celldeviance; a++)
        {
            if (a < 0 || a >= GameObjectGridList.Length) continue;

            for (int b = posZ - _celldeviance; b < posZ + _celldeviance; b++)
            {
                if (b < 0 || b >= GameObjectGridList.Length) continue;

                for (int c = 0; c < GameObjectGridList[a][b].Count; c++)
                {                    
                    
                    if (GameObjectGridList[a][b][c].GetInstanceID() == go.GetInstanceID())
                    {
                        if (a != posX ||  b != posZ)
                        Debug.Log("[CoinGrid] Missplace removed " + go.GetInstanceID() + " " + go.transform.position + " " + posX + " " + posZ + " " + a + " " + b);
                        GameObjectGridList[a][b][c].SetActive(false);
                        GameObjectGridList[a][b].RemoveAt(c);
                    }
                    
                }
            }
        }        
    }
    public GameObject FindCloses(Vector3 pos)
    {

        int posX = GameObjectGridHelpers.WorldToGridIndices(pos.x, _gridSideLength, _gridStep);
        int posZ = GameObjectGridHelpers.WorldToGridIndices(pos.z, _gridSideLength, _gridStep);

        List<GameObject> found = GameObjectGridHelpers.FindNearestGameobjects(GameObjectGridList, posX, posZ);
        if (found != null)
        {            
            return found[0];
        }
        else
        {
            return null;
        }
    }


    public List<GameObject> CheckProximity(float proximityRadius, float centerX, float centerZ)
    {
        
        if (proximityRadius <= 0)
        {
            Debug.LogError("[CoinGrid] Proximity Radius should be greater than zero.");
            return null;
        }


        int indexHalfSize =Mathf.FloorToInt(proximityRadius / _gridStep);
        int indexCenterX = GameObjectGridHelpers.WorldToGridIndices(centerX, _gridSideLength, _gridStep);
        int indexCenterZ = GameObjectGridHelpers.WorldToGridIndices(centerZ, _gridSideLength, _gridStep);
                
        List<GameObject> coinsAround = new List<GameObject>();

        for (int a = indexCenterX - indexHalfSize; a < indexCenterX + indexHalfSize; a++)
        {            
            for (int b = indexCenterZ - indexHalfSize; b < indexCenterZ + indexHalfSize; b++)
            {
                if (a < 0 ||  b < 0 || a >= GameObjectGridList.Length || b >= GameObjectGridList[a].Length)
                {
                    continue;
                }
                for (int c = 0; c < GameObjectGridList[a][b].Count; c++)
                {
                    if (GameObjectGridList[a][b][c])
                    {
                        coinsAround.Add(GameObjectGridList[a][b][c] );
                    }
                }
            }
        }
        return coinsAround;
    }
}
