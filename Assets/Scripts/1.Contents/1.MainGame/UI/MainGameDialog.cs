using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainGameDialog : IDialog
{
    public static event System.Action StartBonusTime_act;
    public static event System.Action<bool> StartGame_act;
    public static event System.Action LeaveScene_act;
    public static event System.Action<bool> ButtonTouch_act;
    public static event System.Action ReStart_act;
    public Sprite[] Tile_sp; 
    MainGameContent _MainGameContent;
    Text _Start_txt , _Score_txt , _Timer_txt;
    RectTransform _Tile;
    List<RectTransform> _TileList = new List<RectTransform>();
    RectTransform _DelayBar_Slider;
    RectTransform _DelayBar_Slider_Fill;
    Slider _BonusTime_Slider;
    Button _Sound_btn, _Pause_btn;
    Button _Right_btn, _Left_btn;
    //popup
    RectTransform _TimeOver_PopUp_rect, _Pause_PopUp_rect;
    Button _TimeOver_PopUp_Home_btn, _TimeOver_PopUp_Restart_btn, _TimeOver_PopUp_Rank_btn;
    Text _TimeOver_PopUp_Score_txt;
    Button _Pause_PopUp_Home_btn, _Pause_PopUp_Play_btn, _Pause_PopUp_Rank_btn;
    Text _Pause_PopUp_Score_txt;
    #region Framework
    protected override void _OnLoad()
    {
        _Caching();
    }
    protected override void _OnLoadComplete()
    {
        _Init();
    }
    protected override void _OnEnter()
    {
        _AddEvent();      
    }
    protected override void _OnExite()
    {
        _RemoveEvent();
    }
    void _AddEvent()
    {
        MainGameContent.UpdateTileList_act += MainGameContent_UpdateTileList_act;
        MainGameContent.AddPenalty_act += MainGameContent_AddPenalty_act;
        MainGameContent.AddBonusValue_act += MainGameContent_AddBonusValue_act;
        MainGameContent.AddScore_act += MainGameContent_AddScore_act;
    }
    void _RemoveEvent()
    {
        MainGameContent.UpdateTileList_act -= MainGameContent_UpdateTileList_act;
        MainGameContent.AddPenalty_act -= MainGameContent_AddPenalty_act;
        MainGameContent.AddBonusValue_act -= MainGameContent_AddBonusValue_act;
        MainGameContent.AddScore_act -= MainGameContent_AddScore_act;
    }
    #endregion
    void _Caching()
    {
        _MainGameContent = GameObject.Find("MainGameContent").GetComponent<MainGameContent>();       
        _Left_btn = transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<Button>();
        _Left_btn.onClick.AddListener(() => ButtonTouch_act?.Invoke(true));

        _Right_btn = transform.GetChild(1).GetChild(0).GetChild(1).GetComponent<Button>();
        _Right_btn.onClick.AddListener(() => ButtonTouch_act?.Invoke(false));
        //_Left_btn;

        _Start_txt = transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>();
        _Score_txt = transform.GetChild(1).GetChild(1).GetChild(0).GetChild(2).GetChild(1).GetComponent<Text>();
        _Timer_txt = transform.GetChild(1).GetChild(1).GetChild(0).GetChild(2).GetChild(2).GetComponent<Text>();
        _Tile = transform.GetChild(2).GetChild(0).GetChild(0).GetComponent<RectTransform>();
        _DelayBar_Slider = transform.GetChild(1).GetChild(2).GetChild(0).GetComponent<RectTransform>();
        _DelayBar_Slider_Fill = _DelayBar_Slider.GetChild(1).GetComponent<RectTransform>();
        _BonusTime_Slider = transform.GetChild(1).GetChild(3).GetChild(0).GetComponent<Slider>();
        for (int i = 0; i < 6; i++)
        {
            _TileList.Add(_Tile.GetChild(i).GetComponent<RectTransform>());
        }

        _TimeOver_PopUp_rect = transform.GetChild(3).GetChild(0).GetComponent<RectTransform>();

        _TimeOver_PopUp_Home_btn = _TimeOver_PopUp_rect.transform.GetChild(0).GetChild(1).GetComponent<Button>();
        _TimeOver_PopUp_Home_btn.onClick.AddListener(() => _LeaveScene_evt(false));

        _TimeOver_PopUp_Restart_btn = _TimeOver_PopUp_rect.transform.GetChild(0).GetChild(2).GetComponent<Button>();
        _TimeOver_PopUp_Restart_btn.onClick.AddListener(_TimeOver_PopUp_Restart_btn_evt);
        _TimeOver_PopUp_Rank_btn = _TimeOver_PopUp_rect.transform.GetChild(0).GetChild(3).GetComponent<Button>();
        _TimeOver_PopUp_Rank_btn.onClick.AddListener(() => _LeaveScene_evt(true));

        _TimeOver_PopUp_Score_txt = _TimeOver_PopUp_rect.GetChild(0).GetChild(0).GetChild(1).GetComponent<Text>();

        _Pause_PopUp_rect = transform.GetChild(3).GetChild(1).GetComponent<RectTransform>();

        _Pause_PopUp_Home_btn = _Pause_PopUp_rect.transform.GetChild(0).GetChild(1).GetComponent<Button>();
        _Pause_PopUp_Home_btn.onClick.AddListener(() => { Time.timeScale = 1; _LeaveScene_evt(false); });

        _Pause_PopUp_Play_btn = _Pause_PopUp_rect.transform.GetChild(0).GetChild(2).GetComponent<Button>();
        _Pause_PopUp_Play_btn.onClick.AddListener(() =>
        {
            Time.timeScale = 1;
            StartGame_act?.Invoke(true);
            _Pause_PopUp_rect.gameObject.SetActive(false);
        });

        _Pause_PopUp_Rank_btn = _Pause_PopUp_rect.transform.GetChild(0).GetChild(3).GetComponent<Button>();
        _Pause_PopUp_Rank_btn.onClick.AddListener(() => { Time.timeScale = 1; _LeaveScene_evt(true); });

        _Pause_PopUp_Score_txt = _Pause_PopUp_rect.GetChild(0).GetChild(0).GetChild(1).GetComponent<Text>();

        _Sound_btn = transform.GetChild(1).GetChild(1).GetChild(0).GetChild(1).GetChild(0).GetComponent<Button>();
        _Pause_btn = transform.GetChild(1).GetChild(1).GetChild(0).GetChild(1).GetChild(1).GetComponent<Button>();
        _Pause_btn.onClick.AddListener(() =>
        {
            Time.timeScale = 0;
            _Pause_PopUp_Score_txt.text = "Score : " + _MainGameContent.GetCurrentScore();
            StartGame_act?.Invoke(false);
            _Pause_PopUp_rect.gameObject.SetActive(true);

        });


    }
    void _Init()
    {
        Debug.Log("MainGameDialog _OnLoadComplete");
        _Start_txt.gameObject.SetActive(false);
        _Score_txt.text = "0";
        _Tile.gameObject.SetActive(false);
        _DelayBar_Slider.gameObject.SetActive(false);
        _BonusTime_Slider.value = 0.0f;
        _TimeOver_PopUp_rect.gameObject.SetActive(false);
        _Pause_PopUp_rect.gameObject.SetActive(false);
        _Timer_txt.text = string.Format("{0:00} : {1:00}", Mathf.FloorToInt(_MainGameContent.GameTime / 60), Mathf.FloorToInt(_MainGameContent.GameTime % 60));
        for (int i = 0; i < _TileList.Count; i++)
        {
            _TileList[i].GetChild(0).GetChild(0).GetComponent<Image>().sprite = Tile_sp[(int)_MainGameContent._TilesList[i].Right];
            _TileList[i].GetChild(1).GetChild(0).GetComponent<Image>().sprite = Tile_sp[(int)_MainGameContent._TilesList[i].Left];
        }
        StartCoroutine(_cStartGame());
    }
    IEnumerator _cStartGame()
    {
        _Start_txt.gameObject.SetActive(true);
        _Start_txt.text = $"3";
        yield return new WaitForSeconds(1.0f);
        _Start_txt.text = $"2";
        yield return new WaitForSeconds(1.0f);
        _Start_txt.text = $"1";
        yield return new WaitForSeconds(1.0f);
        _Start_txt.text = $"Start!";
        yield return new WaitForSeconds(0.25f);
        _Start_txt.gameObject.SetActive(false);
        _Tile.gameObject.SetActive(true);
        StartGame_act?.Invoke(true);
        StartCoroutine(_cStartGameTimer());

    }
    IEnumerator _cStartGameTimer()
    {
        float time = _MainGameContent.GameTime;
        while (time > 0)
        {
            time -= Time.deltaTime;
            _Timer_txt.text = string.Format("{0:00} : {1:00}" , Mathf.FloorToInt(time/60), Mathf.FloorToInt(time%60));
            yield return null;
        }
        //EndGame
        StartGame_act?.Invoke(false);
        _TimeOver_PopUp_rect.gameObject.SetActive(true);
        // Save Score
        _SaveScore();
        _TimeOver_PopUp_Score_txt.text = "Score : " + _MainGameContent.GetCurrentScore();
    }
    IEnumerator _cMoveTiles(float duration)
    {

        _TileList[0].gameObject.SetActive(false);
        
        _TileList[0].anchoredPosition = new Vector2(0, 1450);
        _TileList[0].GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector2(395, -246);
        _TileList[0].GetChild(1).GetComponent<RectTransform>().anchoredPosition = new Vector2(-395, -246);
        _TileList[0].gameObject.SetActive(true);

        for (int i = 0; i < 5; i++)
        { 
            var Tmpe = _TileList[i];
            _TileList[i] = _TileList[i + 1];
            _TileList[i + 1] = Tmpe;
        }
        float time = 0;        
        while (time < duration)
        {
            time += Time.deltaTime;

            for (int i = 0; i < 6; i++)
            {
                _TileList[i].anchoredPosition = Vector2.Lerp(new Vector2(0, i * 250 + 250), new Vector2(0, (i-1) * 250 + 250) , time/ duration);
                if (i == 0)
                {
                    _TileList[i].GetChild(0).GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(new Vector2(395, -246) , new Vector2(335, -246) , time/ duration);
                    _TileList[i].GetChild(1).GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(new Vector2(-395, -246), new Vector2(-335, -246), time / duration);
                }            
            }
            yield return null;
        }

        yield return new WaitForEndOfFrame();

        for (int i = 0; i < _TileList.Count; i++)
        {
            _TileList[i].GetChild(0).GetChild(0).GetComponent<Image>().sprite = Tile_sp[(int)_MainGameContent._TilesList[i].Right];
            _TileList[i].GetChild(1).GetChild(0).GetComponent<Image>().sprite = Tile_sp[(int)_MainGameContent._TilesList[i].Left];
        }

        yield return null;

    }
    private void MainGameContent_UpdateTileList_act()
    {
        StartCoroutine(_cMoveTiles(0.2f));
    }
    private void MainGameContent_AddPenalty_act(float time)
    {
        StartCoroutine(_cPenaltyDelay(time));
    }
    IEnumerator _cPenaltyDelay(float t)
    {
        _DelayBar_Slider.gameObject.SetActive(true);
        float time = 0;
        while (time < t)
        {
            time += Time.deltaTime;

            _DelayBar_Slider_Fill.sizeDelta = Vector2.Lerp(new Vector2(0, 40), new Vector2(280, 40), time / t);

            yield return null;
        }
        _DelayBar_Slider.gameObject.SetActive(false);
    }
    private void MainGameContent_AddBonusValue_act(float obj)
    {
        _BonusTime_Slider.value += obj;
        if (_BonusTime_Slider.value >= 1)
        {
            StartBonusTime_act?.Invoke();
            StartCoroutine(_cStartBonusTimer());        
        }


    }
    IEnumerator _cStartBonusTimer()
    {
        float time = 0;
        while (time < _MainGameContent.BonusTimer)
        {
            time += Time.deltaTime;
            _BonusTime_Slider.value = Mathf.Lerp(1, 0, time / _MainGameContent.BonusTimer);
            yield return null;
        }
        _BonusTime_Slider.value = 0;
    }
    private void MainGameContent_AddScore_act(int obj)
    {
        _Score_txt.text = obj.ToString();
    }
    void _LeaveScene_evt(bool PlzAnyKey)
    {      
        PlayerInfo.Instance.PlzAnyKey = PlzAnyKey;
        for (int i = 0; i < transform.childCount; i++)
            transform.GetChild(i).gameObject.SetActive(false);

        LeaveScene_act?.Invoke();          
    }
    void _TimeOver_PopUp_Restart_btn_evt()
    {
        StopAllCoroutines();
        ReStart_act?.Invoke();
        // Ui init
        _Start_txt.gameObject.SetActive(false);
        _Score_txt.text = "0";
        _Tile.gameObject.SetActive(false);
        _DelayBar_Slider.gameObject.SetActive(false);
        _BonusTime_Slider.value = 0.0f;
        _TimeOver_PopUp_rect.gameObject.SetActive(false);
        _Pause_PopUp_rect.gameObject.SetActive(false);
        _Timer_txt.text = string.Format("{0:00} : {1:00}", Mathf.FloorToInt(_MainGameContent.GameTime / 60), Mathf.FloorToInt(_MainGameContent.GameTime % 60));
        for (int i = 0; i < _TileList.Count; i++)
        {
            _TileList[i].GetChild(0).GetChild(0).GetComponent<Image>().sprite = Tile_sp[(int)_MainGameContent._TilesList[i].Right];
            _TileList[i].GetChild(1).GetChild(0).GetComponent<Image>().sprite = Tile_sp[(int)_MainGameContent._TilesList[i].Left];
        }
        StartCoroutine(_cStartGame());
    }
    
    
    void _SaveScore()
    {
        if (PlayerInfo.Instance.ScoreList.Count < 10)
        {
            PlayerInfo.Instance.ScoreList.Add(int.Parse(_MainGameContent.GetCurrentScore()));
        }
        else
        {
            PlayerInfo.Instance.ScoreList.Sort();
            PlayerInfo.Instance.ScoreList.Reverse();
            if (int.Parse(_MainGameContent.GetCurrentScore()) > PlayerInfo.Instance.ScoreList[9])
                PlayerInfo.Instance.ScoreList[9] = int.Parse(_MainGameContent.GetCurrentScore());
        }
        PlayerInfo.Instance.ScoreList.Sort();
        PlayerInfo.Instance.ScoreList.Reverse();
#if !UNITY_WEBGL || UNITY_EDITOR
        PlayerInfo.Instance.SaveScore();
#endif
    }
}

