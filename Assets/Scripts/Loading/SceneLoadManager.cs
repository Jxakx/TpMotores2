using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneLoadManager : MonoBehaviour
{

    [SerializeField] private Slider loadBar;
    [SerializeField] private GameObject loadPanel;

    public void SceneLoad(int sceneIndex)
    {
        loadPanel.SetActive(true);
        StartCoroutine(LoadAsync(sceneIndex));
    }

    IEnumerator LoadAsync(int sceneIndex)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneIndex);

        while (!asyncOperation.isDone)
        {
            Debug.Log(asyncOperation.progress);
            loadBar.value = asyncOperation.progress / 0.9f;
            yield return null;
        }
    }

}
