using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneAsyncAfterVideo : MonoBehaviour
{
    private bool videoIsFished = false;
    [SerializeField]
    private float videoDuration;
    void Start()
    {
        StartCoroutine(LoadYourAsyncScene());  
        StartCoroutine(WaitForVideo());  
    }

    IEnumerator LoadYourAsyncScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(2);
        asyncLoad.allowSceneActivation = false;

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            if (asyncLoad.progress >= 0.9f && videoIsFished)
            {
                asyncLoad.allowSceneActivation = true;
            }
            yield return null;
        }
    }

    IEnumerator WaitForVideo()
    {
        yield return new WaitForSeconds(videoDuration);
        videoIsFished = true;
    }
}