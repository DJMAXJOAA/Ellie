using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Centers.Test;
using Assets.Scripts.Loading;
using Assets.Scripts.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Assets.Scripts.Centers.Test;
using UnityEditor.SearchService;

public class LoadingUI : MonoBehaviour
{
    public static readonly string ImagePath = "UI/Load/Loading";

    [SerializeField] private Slider loadingSlider;
    [SerializeField] private TextMeshProUGUI tipText;
    [SerializeField] private Image loadingImage;

    [SerializeField] private TipsParshingInfo data;

    const int loadingDataQuantity = 2;
    const float spareTimeToLoad = 1.0f;

    const float generalBarSpeed = 0.001f;
    const float fasterBarSpeed = 0.005f;

    private float barSpeed;

    const int imageQuantity = 4;
    const int tipQuantity = 3;

    private void Start()
    {
        UpdateImageTip();
        StartCoroutine(LoadLevelAsync(TestCenterWithScene.Instance.CurrentScene));
    }

    void UpdateImageTip()
    {
        string imagePath = ImagePath + Random.Range(0, imageQuantity).ToString();
        loadingImage.sprite = ResourceManager.Instance.LoadSprite(imagePath);
        string tip = data.datas[Random.Range(0, tipQuantity)].tip;
        tipText.text = tip;
    }

    private IEnumerator LoadLevelAsync(SceneName scene)
    {
        barSpeed = generalBarSpeed;
        float targetLoad = 0.8f / loadingDataQuantity;

        AsyncOperation loadOperation = SceneManager.LoadSceneAsync((int)scene, LoadSceneMode.Additive);


        //Load Map
        while (!loadOperation.isDone)
        {
            UpdateProgressBar(targetLoad);
            yield return null;
        }

        barSpeed = fasterBarSpeed;
        while (loadingSlider.value < targetLoad)
        {
            UpdateProgressBar(targetLoad);
            yield return null;
        }
        targetLoad += targetLoad;

        //Load Parsing
        yield return DataManager.Instance.CheckIsParseDone();

        barSpeed = fasterBarSpeed;
        while (loadingSlider.value < targetLoad)
        {
            UpdateProgressBar(targetLoad);
            yield return null;
        }
        barSpeed = generalBarSpeed;
        yield return new WaitForSeconds(spareTimeToLoad);
        targetLoad += targetLoad;

        // + Add Load
        // player save point

        // Finish Loading
        while (loadingSlider.value < 1.0f)
        {
            UpdateProgressBar(1.0f);
            yield return null;
        }
        yield return new WaitForSeconds(spareTimeToLoad);

        loadingSlider.value = 0.0f;
        TestCenterWithScene.Instance.FinishLoading();

        SceneManager.UnloadSceneAsync((int)SceneName.LoadingScene);
    }

    private void UpdateProgressBar(float max)
    {
        if (loadingSlider.value < max)
            loadingSlider.value += barSpeed;
    }
}
