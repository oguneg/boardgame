using UnityEngine;
using System.Collections.Generic;

public class Track : MonoBehaviour
{
    public TrackTile startTile;
    public List<TrackTile> orderedTiles = new List<TrackTile>();

    public TrackTile GetNextTile(TrackTile currentTile)
    {
        int index = orderedTiles.IndexOf(currentTile);
        return orderedTiles[(index + 1) % orderedTiles.Count];
    }

    public void AssignMaterials(Material defaultMat, Material startMat, Material minigameMat)
    {
        foreach (var element in orderedTiles)
        {
            switch (element.tileType)
            {
                case TileType.Empty:
                    element.SetMaterial(defaultMat);
                    break;
                case TileType.Minigame:
                    element.SetMaterial(minigameMat);
                    break;
                case TileType.Start:
                    element.SetMaterial(startMat);
                    break;
            }
        }
    }
}