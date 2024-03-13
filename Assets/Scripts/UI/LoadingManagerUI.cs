using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingManagerUI : MonoBehaviour
{    
    [SerializeField] private Slider _slider;
    [SerializeField] private TextMeshProUGUI _txtProgress;

    private void Start()
    {
        StartCoroutine(LoadingAsync(SceneEnum.GameScene.ToString()));
    }

    private IEnumerator LoadingAsync(string scene)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(scene);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);

            _slider.value = progress;
            _txtProgress.text = progress * 100f + "%";

            yield return null;
        }
    }
}
