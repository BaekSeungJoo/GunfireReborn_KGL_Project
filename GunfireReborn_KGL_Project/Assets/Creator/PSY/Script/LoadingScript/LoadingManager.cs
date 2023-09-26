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

    private float[] fill = new float[3] { 0.06f, 0.52f, 1f };

    private void Awake()
    {
        int randCount = UnityEngine.Random.Range(0, fill.Length);
        loadingBar.fillAmount = fill[randCount];

        #region 로딩 팁 List
        loadingTipList.Add("무빙실력이 뛰어나면 대부분의 상황에 쉽게 대처를 할 수 있다.");
        loadingTipList.Add("박시연은 2002년생이다.");
        loadingTipList.Add("망고는 2022년 11월 08일에 데려왔습니다.");
        loadingTipList.Add("신성창 : 도와줘 승주에몽");
        loadingTipList.Add("백승주 : 부르지마세요");
        loadingTipList.Add("한만균 : 엥? 모르게따");
        #endregion

        // 랜덤으로 로딩 이미지 및 텍스트를 출력한다. {
        int randImageNum = UnityEngine.Random.Range(0, loadingList.Count);
        int randTextNum = UnityEngine.Random.Range(0, loadingTipList.Count);

        loadingImage.sprite = loadingList[randImageNum];
        loadingTipText.text = loadingTipList[randTextNum];
        // } 랜덤으로 로딩 이미지 및 텍스트를 출력한다.
    }

    private void Start()
    {
        Debug.Log("실행");
        if(GameManager.instance?.nowStage >= 1 && GameManager.instance?.nowStage <= 3)
        {
            GameManager.instance.nowStage++;
            Debug.Log("들어옴" + GameManager.instance.nowStage);
            StartCoroutine(LoadSceneMap("Main_Map_0" + GameManager.instance.nowStage));
        }
    }

    #region 비동기 로딩
    /// <summary>
    /// 비동기 로딩 함수 ( Map 01 )
    /// </summary>
    public IEnumerator LoadSceneMap( string sceneName )
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;  // 씬을 로드하는데 준비가 안됨.

        while (!operation.isDone && operation.allowSceneActivation == false)  // 씬의 로드가 끝날 때까지 반복
        {
            yield return new WaitForSeconds(3f);    // 실제 시간 3초 딜레이를 준다.

            operation.allowSceneActivation = true;  // 씬 로드의 준비를 끝낸다.
        }
    }
    #endregion
}
