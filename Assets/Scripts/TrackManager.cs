using UnityEngine;
using System.Collections.Generic;

public class TrackManager : MonoBehaviour
{
    public List<Track> tracks;
    public Material startTileMat, defaultTileMat, minigameTileMat;

    public List<TrackTileData> trackTiles = new List<TrackTileData>();
    private Track loadedTrack;
    [SerializeField] private PlayerController player;

    public void LoadTrack(int index)
    {
        loadedTrack = Instantiate(tracks[index % tracks.Count], transform);
        loadedTrack.AssignMaterials(defaultTileMat, startTileMat, minigameTileMat);
        player.Initialize(loadedTrack);
    }
}

[System.Serializable]
public class TrackTileData
{
    public Vector3 position;
    public TileType tileType;
    public int index;
}

public enum TileType
{
    Empty,
    Minigame,
    Start
}
