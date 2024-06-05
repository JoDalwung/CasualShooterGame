#if !UNITY_WEBGL || UNITY_EDITOR
using System.IO;
#endif
using System.Collections.Generic;
using UnityEngine;

public class PlayerScore
{
    public List<int> ScoreDataList = new List<int>();
}

public class PlayerInfo : Singletion<PlayerInfo>
{   
    public bool PlzAnyKey = false;
    public List<int> ScoreList = new List<int>();

#if !UNITY_WEBGL || UNITY_EDITOR
    public void SaveScore()
    {
        PlayerScore data = new PlayerScore();
        data.ScoreDataList = ScoreList;
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(Application.streamingAssetsPath + "/PlayerScoreFile.json",json);
    }
    public void LoadScore()
    { 
        string json = File.ReadAllText(Application.streamingAssetsPath + "/PlayerScoreFile.json");
        PlayerScore data = JsonUtility.FromJson<PlayerScore>(json);
        ScoreList = data.ScoreDataList;
    }
#endif
}
