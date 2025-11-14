using AxGrid.Base;
using AxGrid.Model;
using System.Collections.Generic;
using UnityEngine;

//clearly needs additional work
//draw grid with custom wire lines
//check for view distance culling

public class AdvancedGridGizmoDrawer : MonoBehaviourExtBind
{
    [Header("Gizmo Settings")]
    private List<GameObject>[][] GameObjectGridList;
    public bool ShowGizmos = true;
    public GizmoDisplayMode DisplayMode = GizmoDisplayMode.Always;

    [Header("Visual Settings")]
    public float SphereRadius = 0.5f;
    public Color ObjectColor = Color.cyan;
    public Color ArrayPositionColor = Color.yellow;
    public Color ConnectionColor = Color.white;

    [Header("Array Grid Settings")]
    public Vector3 GridStartPosition = Vector3.zero;
    public float CellSize = 2f;
    public bool ShowGrid = true;

    [Header("Optimization Settings")]
    [Tooltip("Maximum number of objects to draw labels for (0 = unlimited)")]
    public int MaxLabelDrawCount = 50;
    [Tooltip("Only draw gizmos when camera is closer than this distance")]
    public float MaxDrawDistance = 100f;
    public bool UseDistanceCulling = true;
    [Tooltip("Times information updates, 1 is every frame, 2 is every second frame, etc")]
    public int timerOffest = 20;
    private float timer = 0f;
    public enum GizmoDisplayMode
    {
        Always,
        SelectedOnly,
        Never
    }

    // Cache frequently used values
    private Camera _currentCamera;
    private Transform _cameraTransform;
    private Vector3 _cameraPosition;
    private readonly Vector3 _labelOffset = new Vector3(0, 0.7f, 0);
    private readonly Vector3 _smallCubeSize = Vector3.one * 0.5f;
    private readonly Vector3 _gridCellSize = new Vector3(1f, 0.1f, 1f);

    // Pre-allocated collections to avoid GC
    private readonly List<Vector3> _gridCellCenters = new List<Vector3>(100);
    private readonly List<(Vector3 start, Vector3 end, int i, int j)> _connections = new List<(Vector3, Vector3, int, int)>(100);
    private readonly List<(Vector3 position, int i, int j)> _objectPositions = new List<(Vector3, int, int)>(100);

    

    [Bind("OnGameObjectGridChanged")]
    private void OnGameObjectGridChanged()
    {
        GameObjectGridList = Model.Get<GameObjectGrid>("GameObjectGrid").GetGrid();
    }

    private void OnDrawGizmos()
    {
        if (DisplayMode != GizmoDisplayMode.Always) return;
        DrawGizmos();
    }

    private void OnDrawGizmosSelected()
    {
        if (DisplayMode != GizmoDisplayMode.SelectedOnly) return;
        DrawGizmos();
    }

    private void DrawGizmos()
    {
        if (!ShowGizmos || GameObjectGridList == null) return;

        // Early exit if camera is too far
        if (UseDistanceCulling && !IsCameraInRange())
            return;


        timer += 1;
        if (timer % timerOffest == 0) 
        
        {
            // Clear cached data
            _gridCellCenters.Clear();
            _connections.Clear();
            _objectPositions.Clear();



            // First pass: collect all data
            CollectGridData();
        }
        // Second pass: batch draw everything
        BatchDrawGrid();
        BatchDrawConnectionsAndObjects();

        // Third pass: draw labels (most expensive, so do it last and with limits)
        DrawLabels();
    }

    private bool IsCameraInRange()
    {
        if (_currentCamera == null)
        {
            _currentCamera = Camera.current;
            if (_currentCamera != null)
                _cameraTransform = _currentCamera.transform;
        }

        if (_cameraTransform == null)
            return true; // No camera found, draw everything

        _cameraPosition = _cameraTransform.position;
        float distanceToGrid = Vector3.Distance(_cameraPosition, GridStartPosition);

        return distanceToGrid <= MaxDrawDistance;
    }

    private void CollectGridData()
    {
        for (int i = 0; i < GameObjectGridList.Length; i++)
        {
            if (GameObjectGridList[i] == null) continue;

            for (int j = 0; j < GameObjectGridList[i].Length; j++)
            {
                if (GameObjectGridList[i][j] == null) continue;

                Vector3 arrayWorldPos = GetArrayWorldPosition(i, j);
                _gridCellCenters.Add(arrayWorldPos);

                // Process objects in this array cell
                foreach (GameObject gameObject in GameObjectGridList[i][j])
                {
                    if (gameObject != null && gameObject.transform != null)
                    {
                        Vector3 objectPos = gameObject.transform.position;
                        _objectPositions.Add((objectPos, i, j));
                        _connections.Add((arrayWorldPos, objectPos, i, j));
                    }
                }
            }
        }
    }

    private void BatchDrawGrid()
    {
        if (!ShowGrid) return;

        // Batch draw all grid cells
        Gizmos.color = new Color(0.5f, 0.5f, 0.5f, 0.3f);
        foreach (Vector3 cellCenter in _gridCellCenters)
        {
            Gizmos.DrawWireCube(cellCenter, _gridCellSize * CellSize);
        }

        // Batch draw all array position indicators
        Gizmos.color = ArrayPositionColor;
        foreach (Vector3 cellCenter in _gridCellCenters)
        {
            Gizmos.DrawWireCube(cellCenter, _smallCubeSize);
        }
    }

    private void BatchDrawConnectionsAndObjects()
    {
        // Batch draw all connection lines
        Gizmos.color = ConnectionColor;
        foreach (var connection in _connections)
        {
            Gizmos.DrawLine(connection.start, connection.end);
        }

        // Batch draw all object positions
        Gizmos.color = ObjectColor;
        foreach (var obj in _objectPositions)
        {
            Gizmos.DrawSphere(obj.position, SphereRadius);
        }
    }

    private void DrawLabels()
    {
#if UNITY_EDITOR
        if (MaxLabelDrawCount == 0 || _objectPositions.Count <= MaxLabelDrawCount)
        {
            // Draw all labels if under limit
            foreach (var obj in _objectPositions)
            {
                DrawSingleLabel(obj.position, obj.i, obj.j);
            }
        }
        else
        {
            // Draw limited number of labels, prioritizing closest to camera
            DrawPriorityLabels();
        }
#endif
    }

    private void DrawPriorityLabels()
    {
#if UNITY_EDITOR
        // Sort objects by distance to camera (closest first)
        var sortedObjects = new List<(Vector3 position, int i, int j, float distance)>();

        foreach (var obj in _objectPositions)
        {
            float distance = Vector3.Distance(_cameraPosition, obj.position);
            sortedObjects.Add((obj.position, obj.i, obj.j, distance));
        }

        // Sort by distance (closest first)
        sortedObjects.Sort((a, b) => a.distance.CompareTo(b.distance));

        // Draw labels for closest objects only
        for (int i = 0; i < Mathf.Min(MaxLabelDrawCount, sortedObjects.Count); i++)
        {
            var obj = sortedObjects[i];
            DrawSingleLabel(obj.position, obj.i, obj.j);
        }
#endif
    }

    private void DrawSingleLabel(Vector3 position, int i, int j)
    {
#if UNITY_EDITOR
        string label = $"[{i},{j}]\n{position}";
        UnityEditor.Handles.Label(position + _labelOffset, label);
#endif
    }

    private Vector3 GetArrayWorldPosition(int i, int j)
    {
        return GridStartPosition + new Vector3(i * CellSize, j * CellSize, 0);
    }

    // Helper method to quickly check if we should draw
    private bool ShouldDraw()
    {
        if (!ShowGizmos || GameObjectGridList == null)
            return false;

        if (UseDistanceCulling && !IsCameraInRange())
            return false;

        return true;
    }
}