using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    private Track track;
    private TrackTile currentTile;

    public UnityAction<TileType> OnMovementEnded;
    public UnityAction<int> OnMoved;
    [SerializeField] private Transform pin;
    [SerializeField] private Animator animator;
    private int JumpAnimTrigger = Animator.StringToHash("Jump");
    public void Initialize(Track track)
    {
        this.track = track;
        transform.position = track.startTile.transform.position;
        currentTile = track.startTile;
        FaceNextTile();
    }

    public void Move(int stepCount)
    {
        StartCoroutine(MoveRoutine(stepCount));
    }

    IEnumerator MoveRoutine(int stepCount)
    {
        for (int i = 0; i < stepCount; i++)
        {
            yield return new WaitForSeconds(0.25f);
            MoveToNextTile(i < stepCount - 1);
            OnMoved?.Invoke(stepCount - i - 1);
        }
    }

    private void MoveToNextTile(bool isVisiting)
    {
        currentTile = track.GetNextTile(currentTile);
        transform.DOMove(currentTile.transform.position, 0.15f).SetEase(Ease.InOutSine).OnComplete(() =>
        {
            FaceNextTile();
            if (isVisiting)
            {
                currentTile.VisitTile();
            }
            else
            {
                currentTile.LandTile();
                EndMovement();
            }
        });

        pin.DOLocalMoveY(0.5f, 0.075f).SetEase(Ease.InOutSine).SetRelative().SetLoops(2, LoopType.Yoyo);
        animator.SetTrigger(JumpAnimTrigger);
    }

    private void EndMovement()
    {
        OnMovementEnded?.Invoke(currentTile.tileType);
    }

    private void FaceNextTile()
    {
        transform.DOLookAt(track.GetNextTile(currentTile).transform.position, 0.1f).SetEase(Ease.InOutSine);
    }
}
