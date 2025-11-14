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

    public List<GameObject>[][] GetGrid ()
    {
        return GameObjectGridList;
    }

    public GameObjectGrid (float gridSideLength, float gridStep)
    {        
        if (gridSideLength < 0 )
        {
            Debug.LogError("[GameObjectGrid] Grid Side Length must be more than zero.");
        }
        _gridSideLength = gridSideLength;
        if (gridStep < 0)
        {
            Debug.LogError("[GameObjectGrid] Grid Step must be more than zero.");
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

    public void Track(GameObject go, Vector3 prevPosition)
    {        
         if (GameObjectGridList == null)
        {
            Debug.LogWarning("[GameObjectGrid] No GameObjectGrid presented!");
            return;
        }
        int prevPosX = GameObjectGridHelpers.WorldToGridIndices(prevPosition.x, _gridSideLength, _gridStep);
        int prevPosY = GameObjectGridHelpers.WorldToGridIndices(prevPosition.y, _gridSideLength, _gridStep);

        GameObjectGridList[prevPosX][prevPosY].Remove(go);

        int posX = GameObjectGridHelpers.WorldToGridIndices(go.transform.position.x, _gridSideLength, _gridStep);
        int posY = GameObjectGridHelpers.WorldToGridIndices(go.transform.position.y, _gridSideLength, _gridStep);

        GameObjectGridList[posX][posY].Add(go);
    }

    public void Add(GameObject go)
    {
        if (GameObjectGridList == null)
        {
            Debug.LogWarning("[GameObjectGrid] No GameObjectGrid presented!");
            return;
        }

        int posX = GameObjectGridHelpers.WorldToGridIndices(go.transform.position.x, _gridSideLength, _gridStep);
        int posY = GameObjectGridHelpers.WorldToGridIndices(go.transform.position.y, _gridSideLength, _gridStep);

        GameObjectGridList[posX][posY].Add(go);
    }

    public void Remove(GameObject go) 
    {
        if (GameObjectGridList == null)
        {
            Debug.LogWarning("[GameObjectGrid] No GameObjectGrid presented!");
            return;
        }
        

        int posX = GameObjectGridHelpers.WorldToGridIndices(go.transform.position.x, _gridSideLength, _gridStep);
        int posY = GameObjectGridHelpers.WorldToGridIndices(go.transform.position.y, _gridSideLength, _gridStep);

        for (int a = posX - _celldeviance; a < posX + _celldeviance; a++)
        {
            if (a < 0 || a >= GameObjectGridList.Length) continue;

            for (int b = posY - _celldeviance; b < posY + _celldeviance; b++)
            {
                if (b < 0 || b >= GameObjectGridList.Length) continue;

                for (int c = 0; c < GameObjectGridList[a][b].Count; c++)
                {                    
                    
                    if (GameObjectGridList[a][b][c].GetInstanceID() == go.GetInstanceID())
                    {
                        if (a != posX ||  b != posY)
                        Debug.Log("[GameObjectGrid] Missplace removed " + go.GetInstanceID() + " " + go.transform.position + " " + posX + " " + posY + " " + a + " " + b);
                        GameObjectGridList[a][b][c].SetActive(false);
                        GameObjectGridList[a][b].RemoveAt(c);
                    }
                    
                }
            }
        }        
    }

    public GameObject FindClosets(Vector3 pos) //returns 1st closest gameobject, checks for entire space
    {

        int posX = GameObjectGridHelpers.WorldToGridIndices(pos.x, _gridSideLength, _gridStep);
        int posY = GameObjectGridHelpers.WorldToGridIndices(pos.y, _gridSideLength, _gridStep);

        List<GameObject> found = GameObjectGridHelpers.FindNearestGameobject(GameObjectGridList, posX, posY);
        if (found != null)
        {            
            return found[0];
        }
        else
        {
            return null;
        }
    }
    
    public GameObject FindClosestByRadius(Vector3 pos, float radius)
    {
        int posX = GameObjectGridHelpers.WorldToGridIndices(pos.x, _gridSideLength, _gridStep);
        int posY = GameObjectGridHelpers.WorldToGridIndices(pos.y, _gridSideLength, _gridStep);

        int gridRadius = GameObjectGridHelpers.WorldToGridIndices(radius, _gridSideLength, _gridStep);

        List<GameObject> found = GameObjectGridHelpers.FindNearestInRadius(GameObjectGridList, posX, posY, gridRadius);
        if (found != null)
        {
            return found[0];
        }
        else
        {
            return null;
        }
    }

    public GameObject FindClosestByRadiusAndLayer(Vector3 pos, float radius, LayerMask layer)
    {
        int posX = GameObjectGridHelpers.WorldToGridIndices(pos.x, _gridSideLength, _gridStep);
        int posY = GameObjectGridHelpers.WorldToGridIndices(pos.y, _gridSideLength, _gridStep);

        int inGridRadius = Mathf.RoundToInt(radius / _gridStep);

        GameObject found = GameObjectGridHelpers.FindNearestInRadiusAndLayer(GameObjectGridList, posX, posY, inGridRadius, layer);
                
        if (found != null)
        {
            return found;
        }
        else
        {
            return null;
        }        
    }

    public List<GameObject> CheckProximity(float proximityRadius, float centerX, float centerY)
    {
        
        if (proximityRadius <= 0)
        {
            Debug.LogError("[GameObjectGrid] Proximity Radius should be greater than zero.");
            return null;
        }


        int indexHalfSize =Mathf.FloorToInt(proximityRadius / _gridStep);
        int indexCenterX = GameObjectGridHelpers.WorldToGridIndices(centerX, _gridSideLength, _gridStep);
        int indexCenterY = GameObjectGridHelpers.WorldToGridIndices(centerY, _gridSideLength, _gridStep);
                
        List<GameObject> coinsAround = new List<GameObject>();

        for (int a = indexCenterX - indexHalfSize; a < indexCenterX + indexHalfSize; a++)
        {            
            for (int b = indexCenterY - indexHalfSize; b < indexCenterY + indexHalfSize; b++)
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
