using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ScoresManager : MonoBehaviour
{
    public static ScoresManager Instance { get; private set; }

    public string playerName;
    public int score;
    public float time;

    public List<ScoreData> scoreDataList { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        LoadData();
        
        DontDestroyOnLoad(gameObject);
    }

    public void SetPlayerName(string playerName_)
    {
        this.playerName = playerName_;
    }

    public void SortScores()
    {
        for (int i = 0; i < scoreDataList.Count; i++)
        {
            for (int j = i; j < scoreDataList.Count; j++)
            {
                if (scoreDataList[j].score > scoreDataList[i].score)
                {
                    ScoreData tmp = scoreDataList[i];
                    scoreDataList[i] = scoreDataList[j];
                    scoreDataList[j] = tmp;

                }
            }
        }
    }

    public void SaveNewGameData()
    {
        ScoreData newGameData = new ScoreData();
        newGameData.time = time;
        newGameData.date = System.DateTime.Now.ToString("dd/MM/yyyy");
        newGameData.playerName = playerName;
        newGameData.score = score;

        scoreDataList.Add(newGameData);
        SortScores();
        SaveData();
    }

    public void SaveData()
    {
        ScoresData scores = new ScoresData();
        scores.ScoreDataList = scoreDataList;
        string json = JsonUtility.ToJson(scores);

        File.WriteAllText(Application.persistentDataPath + "scoresData.json", json);
        //Debug.Log(File.ReadAllText(Application.persistentDataPath + "scoresData.json"));
    }

    public void LoadData()
    {
        string path = Application.persistentDataPath + "scoresData.json";
        Debug.Log(Application.persistentDataPath + "scoresData.json");
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            ScoresData data = JsonUtility.FromJson<ScoresData>(json);
            scoreDataList = data.ScoreDataList;

            Debug.Log(scoreDataList);

            
            //SortScores();
        }
        else
        {
            scoreDataList = new List<ScoreData>();
        }
    }
}
