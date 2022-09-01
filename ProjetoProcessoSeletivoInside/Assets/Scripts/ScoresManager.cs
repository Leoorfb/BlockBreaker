using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class IntEvent : UnityEvent<int> { }

public class ScoresManager : MonoBehaviour
{
    // Evento ativado quando a Pontuação é alterada
    public IntEvent ScoreChangeEvent;

    // Singleton
    public static ScoresManager Instance { get; private set; }

    // Atributos relacinados a Pontuação da partida atual (talvez mover eles)
    public PlayerController player;
    public string playerName;
    public int score;
    public float time;

    // Lista de Pontuações
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

    // Adiciona pontos ao player atual
    public void AddScore(int _score)
    {
        score += _score;
        ScoreChangeEvent.Invoke(score);
    }

    // Organiza as Pontuações da maior para a menor
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

    // Salva a Pontuação do Jogo Atual
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

    // Salva a lista de Pontuações
    public void SaveData()
    {
        ScoresData scores = new ScoresData();
        scores.ScoreDataList = scoreDataList;
        string json = JsonUtility.ToJson(scores);

        File.WriteAllText(Application.persistentDataPath + "scoresData.json", json);
    }

    // Carrega a lista de Pontuações
    public void LoadData()
    {
        string path = Application.persistentDataPath + "scoresData.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            ScoresData data = JsonUtility.FromJson<ScoresData>(json);
            scoreDataList = data.ScoreDataList;
        }
        else
        {
            scoreDataList = new List<ScoreData>();
        }
    }
}
