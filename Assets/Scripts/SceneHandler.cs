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
    protected static SceneHandler instance;
    private Coroutine reloadRoutine;

    private void Start()
    {
        Application.targetFrameRate = 60;
        if (instance != null)
        {
            Debug.LogError("Have another instance");
            Destroy(gameObject);
        }
        instance = this;
        StartCoroutine(LoadAsync());
    }

    public static void Reload()
    {
        instance.ReloadScene();
    }

    public void LoadScene()
    {
        StartCoroutine(LoadAsync());
    }

    private IEnumerator LoadAsync()
    {
        PlayLoadingAnimation();
        AsyncOperation operation = SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
        yield return new WaitUntil(() => operation.isDone);
        yield return new WaitForSeconds(0.2f);
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(1));
        CompleteLoading();
    }

    public void ReloadScene()
    {
        if (reloadRoutine == null)
            reloadRoutine = StartCoroutine(ReloadAsync());
    }

    private IEnumerator ReloadAsync()
    {
        cg.gameObject.SetActive(true);
        cg.DOFade(1, 0.2f);
        PlayLoadingAnimation();
        yield return new WaitForSeconds(0.2f);
        var operation = SceneManager.UnloadSceneAsync(1);
        yield return new WaitUntil(() => operation != null && operation.isDone);
        operation = SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
        yield return new WaitUntil(() => operation != null && operation.isDone);
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(1));
        reloadRoutine = null;
        CompleteLoading();
    }
    private void CompleteLoading()
    {
        cg.DOFade(0, 0.2f).OnComplete(() => cg.gameObject.SetActive(false));
        loadingImage.DOKill();
    }

    private void PlayLoadingAnimation()
    {
        loadingImage.DOLocalRotate(Vector3.forward * -120, 1f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Incremental);
    }
}
