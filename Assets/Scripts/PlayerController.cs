using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class PlayerController : MonoBehaviour
{
    private Track track;
    private TrackTile currentTile;
    [SerializeField] private Transform pin;
    public void Initialize(Track track)
    {
        this.track = track;
        transform.position = track.startTile.transform.position;
        currentTile = track.startTile;
        StartCoroutine(Move());
    }

    IEnumerator Move()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.25f);
            transform.DOLookAt(track.GetNextTile(currentTile).transform.position, 0.1f).SetEase(Ease.InOutSine);
            currentTile = track.GetNextTile(currentTile);
            transform.DOMove(currentTile.transform.position, 0.15f).SetEase(Ease.InOutSine);
            currentTile.transform.DOShakeScale(0.6f, 0.2f, 10, 0).SetDelay(0.15f);
            pin.DOLocalMoveY(0.5f, 0.075f).SetEase(Ease.InOutSine).SetRelative().SetLoops(2, LoopType.Yoyo);
            pin.DOScaleY(1.5f, 0.075f).SetEase(Ease.InOutBack).SetLoops(2, LoopType.Yoyo);
        }
    }
}
