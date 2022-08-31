using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEditor;
using System;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField]
    private GameObject mainMenuScreen;
    [SerializeField]
    private GameObject scoreBoardScreen;
    [SerializeField]
    private Transform scoreBoardContent;

    [SerializeField]
    private GameObject scoreBoardItemTemplate;

    public void SetPlayerName(string _playerName)
    {
        ScoresManager.Instance.playerName = _playerName;
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void OpenScoreBoard()
    {
        scoreBoardScreen.SetActive(true);
        mainMenuScreen.SetActive(false);

        if (scoreBoardContent.childCount > 1)
            return;

        StartCoroutine(FillScoreBoard());
    }

    public void CloseScoreBoard()
    {
        mainMenuScreen.SetActive(true);
        scoreBoardScreen.SetActive(false);
    }

    public void ExitGame()
    {
        //ScoresManager.Instance.SaveData();
        //ScoreManager.Instance.SaveColor();
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }

    IEnumerator FillScoreBoard()
    {
        List<ScoreData> scoresDatas = ScoresManager.Instance.scoreDataList;
        for (int i = 0; i < scoresDatas.Count; i++)
        {
            CreateScoreBoardItem(scoresDatas[i], scoreBoardContent, i);
            yield return new WaitForSeconds(.05f);
        }
    }

    private void CreateScoreBoardItem(ScoreData scoreData, Transform container, int index)
    {
        Transform itemTransform = Instantiate(scoreBoardItemTemplate, container).transform;
        RectTransform containerRectTransform = container.GetComponent<RectTransform>();
        //itemRectTransform.anchoredPosition = new Vector2(0, -(index * 100));
        containerRectTransform.sizeDelta = new Vector2(containerRectTransform.sizeDelta.x, (index+1) * 100);
        itemTransform.position = new Vector3(itemTransform.position.x, containerRectTransform.Find("ContentTopBorder").transform.position.y - (index * 100), itemTransform.position.z);
        
        Debug.Log(containerRectTransform.Find("ContentTopBorder").transform.position.y);
        Debug.Log("#" + (index + 1) + " index: " + index + "position " + itemTransform.position);
        
        itemTransform.Find("RankText").GetComponent<TextMeshProUGUI>().text = "#" + (index + 1);
        itemTransform.Find("PlayerNameText").GetComponent<TextMeshProUGUI>().text = scoreData.playerName;
        itemTransform.Find("ScoreText").GetComponent<TextMeshProUGUI>().text = scoreData.score.ToString();

        TimeSpan time = TimeSpan.FromSeconds(scoreData.time);

        itemTransform.Find("TimeText").GetComponent<TextMeshProUGUI>().text = time.ToString("hh':'mm':'ss");
        itemTransform.Find("DateText").GetComponent<TextMeshProUGUI>().text = scoreData.date;
    }


}
