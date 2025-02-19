using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class SceneHandler : MonoBehaviour
{
    [SerializeField] private CanvasGroup cg;
    [SerializeField] private Transform loadingImage;
    private void Start()
    {
        loadingImage.DOLocalRotate(Vector3.forward * -120, 1f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Incremental);
        StartCoroutine(LoadGameScene());
    }

    private IEnumerator LoadGameScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
        yield return new WaitUntil(() => asyncLoad.isDone);
        yield return new WaitForSeconds(1f);
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(1));
        CompleteLoading();
    }

    private void CompleteLoading()
    {
        cg.DOFade(0, 0.2f).OnComplete(() => cg.gameObject.SetActive(false));
        loadingImage.DOKill();
    }
}
