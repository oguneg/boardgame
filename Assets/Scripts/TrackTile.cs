using UnityEngine;

public class TrackTile : MonoBehaviour
{
    public TileType tileType;
    public Vector2Int gridPosition;
    public MeshRenderer tile;

    public void SetMaterial(Material tileMat)
    {
        tile.sharedMaterial = tileMat;
    }
}