using DG.Tweening;
using UnityEngine;

public class TrackTile : MonoBehaviour
{
    public TileType tileType;
    public Vector2Int gridPosition;

    [SerializeField] private MeshRenderer tile;
    [SerializeField] private ParticleSystem visitParticle, landParticle;
 
    public void SetMaterial(Material tileMat)
    {
        tile.sharedMaterial = tileMat;
    }

    public void VisitTile()
    {
        transform.DOShakeScale(0.6f, 0.2f, 10, 0);
        visitParticle.Play();
    }

    public void LandTile()
    {
        transform.DOShakeScale(2f, 0.2f, 10, 0);
        landParticle.Play();
    }
}