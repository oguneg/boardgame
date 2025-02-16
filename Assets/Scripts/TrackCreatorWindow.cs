using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class TrackCreatorWindow : EditorWindow
{
    private GridCellData[,] grid;
    private Vector2Int gridSize = new Vector2Int(10, 10);
    private Vector2 scrollPosition;
    private float cellSize = 30f;
    private string trackName = "New Track";
    private GameObject tilePrefab;

    // Painting states
    private bool isDragging = false;
    private bool isErasing = false;
    private bool paintStart = false;
    private bool paintMinigame = false;
    private Vector2Int? startPosition = null;

    [MenuItem("Tools/Track Creator")]
    public static void ShowWindow()
    {
        GetWindow<TrackCreatorWindow>("Track Creator");
    }

    private void OnGUI()
    {
        DrawToolbar();
        DrawPaintingTools();
        DrawGrid();
        DrawSettings();
    }

    private void DrawToolbar()
    {
        EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);

        if (GUILayout.Button("New", EditorStyles.toolbarButton))
        {
            CreateNewGrid();
        }

        if (GUILayout.Button("Save", EditorStyles.toolbarButton))
        {
            SaveTrack();
        }

        if (GUILayout.Button("Load", EditorStyles.toolbarButton))
        {
            LoadTrack();
        }

        if (GUILayout.Button("Generate Prefab", EditorStyles.toolbarButton))
        {
            GenerateTrackPrefab();
        }

        EditorGUILayout.EndHorizontal();
    }


    private void DrawSettings()
    {
        EditorGUILayout.Space();
        EditorGUILayout.BeginVertical("box");

        trackName = EditorGUILayout.TextField("Track Name", trackName);
        tilePrefab = (GameObject)EditorGUILayout.ObjectField(
            "Tile Prefab",
            tilePrefab,
            typeof(GameObject),
            false
        );

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Grid Size");
        gridSize.x = EditorGUILayout.IntField(gridSize.x);
        gridSize.y = EditorGUILayout.IntField(gridSize.y);
        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("Resize Grid"))
        {
            CreateNewGrid();
        }

        EditorGUILayout.EndVertical();
    }


    private void DrawPaintingTools()
    {
        EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);

        // Toggle buttons for different painting modes
        GUI.backgroundColor = paintStart ? Color.green : Color.white;
        paintStart = GUILayout.Toggle(paintStart, "Set Start", EditorStyles.toolbarButton);

        GUI.backgroundColor = paintMinigame ? Color.yellow : Color.white;
        paintMinigame = GUILayout.Toggle(paintMinigame, "Minigame Tile", EditorStyles.toolbarButton);

        GUI.backgroundColor = Color.white;

        EditorGUILayout.EndHorizontal();
    }

    private void DrawGrid()
    {
        if (grid == null) CreateNewGrid();

        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

        Rect gridRect = GUILayoutUtility.GetRect(
            gridSize.x * cellSize,
            gridSize.y * cellSize
        );

        // Handle mouse input for the entire grid area
        HandleGridInput(gridRect);

        // Draw grid cells
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                Rect cellRect = new Rect(
                    gridRect.x + x * cellSize,
                    gridRect.y + y * cellSize,
                    cellSize,
                    cellSize
                );

                DrawCell(cellRect, x, y);
            }
        }

        EditorGUILayout.EndScrollView();
    }

    private void HandleGridInput(Rect gridRect)
    {
        Event e = Event.current;

        if (!gridRect.Contains(e.mousePosition))
        {
            return;
        }

        Vector2Int gridPosition = GetGridPosition(e.mousePosition, gridRect);

        switch (e.type)
        {
            case EventType.MouseDown:
                if (e.button == 0)
                {
                    isDragging = true;
                    isErasing = !grid[gridPosition.x, gridPosition.y].isTrackTile;
                    PaintCell(gridPosition);
                    e.Use();
                }
                break;

            case EventType.MouseDrag:
                if (isDragging && e.button == 0)
                {
                    PaintCell(gridPosition);
                    e.Use();
                }
                break;

            case EventType.MouseUp:
                if (e.button == 0)
                {
                    isDragging = false;
                    e.Use();
                }
                break;
        }
    }

    private Vector2Int GetGridPosition(Vector2 mousePosition, Rect gridRect)
    {
        Vector2 localPosition = mousePosition - new Vector2(gridRect.x, gridRect.y);
        return new Vector2Int(
            Mathf.Clamp(Mathf.FloorToInt(localPosition.x / cellSize), 0, gridSize.x - 1),
            Mathf.Clamp(Mathf.FloorToInt(localPosition.y / cellSize), 0, gridSize.y - 1)
        );
    }

    private void PaintCell(Vector2Int pos)
    {
        if (paintStart)
        {
            // Clear previous start position
            if (startPosition.HasValue)
            {
                var oldPos = startPosition.Value;
                grid[oldPos.x, oldPos.y].isStart = false;
            }

            grid[pos.x, pos.y].isTrackTile = true;
            grid[pos.x, pos.y].isStart = true;
            grid[pos.x, pos.y].isMinigame = false;
            startPosition = pos;
        }
        else if (paintMinigame)
        {
            grid[pos.x, pos.y].isTrackTile = true;
            grid[pos.x, pos.y].isMinigame = true;
            grid[pos.x, pos.y].isStart = false;
        }
        else
        {
            // Normal painting/erasing
            grid[pos.x, pos.y].isTrackTile = !isErasing;
            if (isErasing)
            {
                grid[pos.x, pos.y].isMinigame = false;
                grid[pos.x, pos.y].isStart = false;
                if (startPosition.HasValue && startPosition.Value == pos)
                {
                    startPosition = null;
                }
            }
        }

        Repaint();
    }

    private void DrawCell(Rect cellRect, int x, int y)
    {
        var cell = grid[x, y];

        // Draw base cell color
        Color cellColor = Color.white;
        if (cell.isTrackTile)
        {
            cellColor = cell.isStart ? Color.green :
                       cell.isMinigame ? Color.yellow :
                       Color.black;
        }

        EditorGUI.DrawRect(cellRect, cellColor);

        // Draw cell border
        EditorGUI.DrawRect(cellRect, new Color(0, 0, 0, 0.1f));
    }

    private void CreateNewGrid()
    {
        grid = new GridCellData[gridSize.x, gridSize.y];
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                grid[x, y] = new GridCellData { position = new Vector2Int(x, y) };
            }
        }
        startPosition = null;
        Repaint();
    }

    private void SaveTrack()
    {
        var data = new TrackCreatorData
        {
            dimensions = gridSize,
            trackName = trackName,
            cells = new List<GridCellData>()
        };

        // Save only track tiles
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                if (grid[x, y].isTrackTile)
                {
                    data.cells.Add(new GridCellData
                    {
                        position = new Vector2Int(x, y),
                        isTrackTile = true,
                        isStart = grid[x, y].isStart,
                        isMinigame = grid[x, y].isMinigame
                    });
                }
            }
        }

        string json = JsonUtility.ToJson(data, true);
        string path = EditorUtility.SaveFilePanel(
            "Save Track Layout",
            "Assets",
            $"{trackName}.json",
            "json"
        );

        if (!string.IsNullOrEmpty(path))
        {
            System.IO.File.WriteAllText(path, json);
            AssetDatabase.Refresh();
        }
    }

    private void LoadTrack()
    {
        string path = EditorUtility.OpenFilePanel(
            "Load Track Layout",
            "Assets",
            "json"
        );

        if (!string.IsNullOrEmpty(path))
        {
            string json = System.IO.File.ReadAllText(path);
            var data = JsonUtility.FromJson<TrackCreatorData>(json);

            gridSize = data.dimensions;
            trackName = data.trackName;
            CreateNewGrid(); // Reset grid

            // Apply loaded data
            foreach (var cell in data.cells)
            {
                grid[cell.position.x, cell.position.y] = cell;
                if (cell.isStart)
                {
                    startPosition = cell.position;
                }
            }

            Repaint();
        }
    }

    private void GenerateTrackPrefab()
    {
        if (tilePrefab == null)
        {
            EditorUtility.DisplayDialog("Error", "Please assign a tile prefab!", "OK");
            return;
        }

        // Create parent object
        GameObject trackObject = new GameObject(trackName);
        Track track = trackObject.AddComponent<Track>();

        List<TrackTile> tiles = new List<TrackTile>();
        TrackTile firstTile = null;

        // Generate tiles based on grid
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                if (grid[x, y].isTrackTile)
                {
                    GameObject tileObj = PrefabUtility.InstantiatePrefab(tilePrefab) as GameObject;
                    tileObj.transform.parent = trackObject.transform;
                    tileObj.transform.localPosition = new Vector3(x, 0, -y);

                    TrackTile tile = tileObj.GetComponent<TrackTile>();
                    if (tile == null) tile = tileObj.AddComponent<TrackTile>();
                    if (grid[x, y].isMinigame)
                    {
                        tile.tileType = TileType.Minigame;
                    }
                    else if (grid[x, y].isStart)
                    {
                        tile.tileType = TileType.Start;
                        firstTile = tile;
                    }
                    tile.gridPosition = new Vector2Int(x, -y);
                    tiles.Add(tile);
                }
            }
        }

        // Set track properties
        track.startTile = firstTile;
        track.startTile.tileType = TileType.Start;
        track.orderedTiles = OrderTilesInSequence(tiles, track.startTile);

        // Save as prefab
        string prefabPath = $"Assets/Tracks/{trackName}.prefab";
        if (!AssetDatabase.IsValidFolder("Assets/Tracks"))
        {
            AssetDatabase.CreateFolder("Assets", "Tracks");
        }

        GameObject prefab = PrefabUtility.SaveAsPrefabAsset(trackObject, prefabPath);
        DestroyImmediate(trackObject);

        EditorUtility.DisplayDialog("Success",
            $"Track prefab created at {prefabPath}", "OK");
    }
    private List<TrackTile> OrderTilesInSequence(List<TrackTile> tiles, TrackTile startTile)
    {
        if (tiles == null || tiles.Count == 0 || startTile == null)
            return new List<TrackTile>();

        List<TrackTile> orderedTiles = new List<TrackTile>();
        HashSet<TrackTile> visitedTiles = new HashSet<TrackTile>();

        // Add start tile
        orderedTiles.Add(startTile);
        visitedTiles.Add(startTile);

        TrackTile currentTile = startTile;
        Vector2Int? lastDirection = null;

        while (orderedTiles.Count < tiles.Count)
        {
            var nextTile = FindNextClockwiseTile(currentTile, lastDirection, tiles, visitedTiles);
            if (nextTile == null)
                break;

            lastDirection = nextTile.gridPosition - currentTile.gridPosition;
            orderedTiles.Add(nextTile);
            visitedTiles.Add(nextTile);
            currentTile = nextTile;
        }

        if (orderedTiles.Count != tiles.Count)
        {
            Debug.LogWarning("Not all tiles could be connected in sequence. Track might have gaps.");
        }

        return orderedTiles;
    }

    private TrackTile FindNextClockwiseTile(TrackTile currentTile, Vector2Int? lastDirection, List<TrackTile> allTiles, HashSet<TrackTile> visitedTiles)
    {
        // Define directions in clockwise order
        Vector2Int[] clockwiseDirections = new Vector2Int[]
        {
        new Vector2Int(1, 0),   // right
        new Vector2Int(0, 1),   // down
        new Vector2Int(-1, 0),  // left
        new Vector2Int(0, -1)   // up
        };

        // If this is the first move (no lastDirection), prefer moving right if possible
        if (!lastDirection.HasValue)
        {
            foreach (var direction in clockwiseDirections)
            {
                Vector2Int neighborPos = currentTile.gridPosition + direction;
                var neighborTile = allTiles.FirstOrDefault(t =>
                    t.gridPosition == neighborPos && !visitedTiles.Contains(t));

                if (neighborTile != null)
                {
                    return neighborTile;
                }
            }
            return null;
        }

        // Find the index of the last direction in our clockwise array
        int lastDirIndex = -1;
        for (int i = 0; i < clockwiseDirections.Length; i++)
        {
            if (clockwiseDirections[i] == lastDirection.Value)
            {
                lastDirIndex = i;
                break;
            }
        }

        if (lastDirIndex == -1)
        {
            Debug.LogError("Invalid last direction");
            return null;
        }

        // Check each direction in clockwise order, starting from 90 degrees right of our last direction
        for (int offset = 1; offset <= 4; offset++)
        {
            int dirIndex = (lastDirIndex + offset) % 4;
            Vector2Int nextDirection = clockwiseDirections[dirIndex];
            Vector2Int neighborPos = currentTile.gridPosition + nextDirection;

            var neighborTile = allTiles.FirstOrDefault(t =>
                t.gridPosition == neighborPos && !visitedTiles.Contains(t));

            if (neighborTile != null)
            {
                return neighborTile;
            }
        }

        return null;
    }
}

[System.Serializable]
public class GridCellData
{
    public bool isTrackTile;
    public bool isStart;
    public bool isMinigame;
    public Vector2Int position;
}

[System.Serializable]
public class TrackCreatorData
{
    public List<GridCellData> cells = new List<GridCellData>();
    public Vector2Int dimensions;
    public string trackName;
}