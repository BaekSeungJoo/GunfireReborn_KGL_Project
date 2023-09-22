using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingManager : MonoBehaviour
{
    public List<Sprite> loadingList = new List<Sprite>();
    public List<string> loadingTipList = new List<string>();
    public Image loadingImage;
    public TextMeshProUGUI loadingTipText;
    public Image loadingBar;
   
    private float[] fill = new float[5]{ 0.05f, 0.28f , 0.52f , 0.75f , 1f };

    private void Awake()
    {
        int randCount = UnityEngine.Random.Range(0, fill.Length);
        loadingBar.fillAmount = fill[randCount];

        loadingTipList.Add("무빙실력이 뛰어나면 대부분의 상황에 쉽게 대처를 할 수 있다.");
        loadingTipList.Add("박시연은 2002년생이다.");
        loadingTipList.Add("망고는 2022년 11월 08일에 데려왔습니다.");
        loadingTipList.Add("신성창 : 도와줘 승주에몽");
        loadingTipList.Add("백승주 : 부르지마세요");
        loadingTipList.Add("한만균 : 엥? 모르게따");

        int randImageNum = UnityEngine.Random.Range(0, loadingList.Count);
        int randTextNum = UnityEngine.Random.Range(0, loadingTipList.Count);

        loadingImage.sprite = loadingList[randImageNum];
        loadingTipText.text = loadingTipList[randTextNum];

        //StartCoroutine(LoadScene());
    }
     
    #region 비동기 
    //private IEnumerator LoadScene()
    //{
    //    AsyncOperation operation = SceneManager.LoadSceneAsync("Map_01_PSY");
    //    operation.allowSceneActivation = false;

    //    while ( !operation.isDone )
    //    {
    //        int randImageNum = UnityEngine.Random.Range(0, loadingList.Count);
    //        int randTextNum = UnityEngine.Random.Range(0, loadingTipList.Count);

    //        loadingImage.sprite = loadingList[randImageNum];
    //        loadingTipText.text = loadingTipList[randTextNum];

    //        yield return new WaitForSeconds(3f);

    //        operation.allowSceneActivation = true;
    //    }
    //}
    #endregion
}
