using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyDialog : IDialog
{

    RectTransform _First_view_rect, _Second_view_rect;
    RectTransform _Popup_Noti_rect;
    RectTransform[] _Scores = new RectTransform[10];


    Button _Leaderboard_btn;
    Button _GameStart_btn;
    Button _GameExit_btn;
    Text _Popup_Noti_txt;


    public static event System.Action LobbyDialog_GameStartBtn_act;
    #region
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
        LobbyContent.PlzAnyKey_act += LobbyContent_PlzAnyKey_act;
    }
    void _RemoveEvent()
    {
        LobbyContent.PlzAnyKey_act -= LobbyContent_PlzAnyKey_act;
    }
    #endregion

    void _Caching()
    {
        _First_view_rect = transform.GetChild(0).GetComponent<RectTransform>();   
        _Second_view_rect = transform.GetChild(1).GetComponent<RectTransform>();

        _Leaderboard_btn = _Second_view_rect.GetChild(0).GetComponent<Button>();
        _Leaderboard_btn.onClick.AddListener(() =>
        {
            if (PlayerInfo.Instance.ScoreList.Count == 0)
                Show_PopUp_Noti("No Score");
        });

        _GameStart_btn = transform.GetChild(1).GetChild(1).GetComponent<Button>();
        _GameStart_btn.onClick.AddListener(() => { LobbyDialog_GameStartBtn_act?.Invoke();});

        _GameExit_btn = transform.GetChild(1).GetChild(3).GetComponent<Button>();
        _GameExit_btn.onClick.AddListener(Application.Quit);

        _Popup_Noti_rect = _Second_view_rect.GetChild(2).GetComponent<RectTransform>();
        _Popup_Noti_txt = _Popup_Noti_rect.GetChild(1).GetComponent<Text>();

        for (int i = 0; i < 10; i++)
            _Scores[i] = transform.GetChild(1).GetChild(0).GetChild(0).GetChild(0).GetChild(i).GetComponent<RectTransform>();

    }
    void _Init()
    {
        _First_view_rect.gameObject.SetActive(false);
        _Second_view_rect.gameObject.SetActive(false);
        _Popup_Noti_rect.gameObject.SetActive(false);

        if (!PlayerInfo.Instance.PlzAnyKey)
            _First_view_rect.gameObject.SetActive(true);
        else
        {
            _Second_view_rect.gameObject.SetActive(true);
            if (PlayerInfo.Instance.ScoreList.Count == 0)
            {
                Show_PopUp_Noti("No Score");
            }   
        }
        for (int i = 0; i < 10; i++)
            _Scores[i].gameObject.SetActive(false);
        for (int i = 0; i < PlayerInfo.Instance.ScoreList.Count; i++)
        {
            _Scores[i].GetChild(0).GetComponent<Text>().text = PlayerInfo.Instance.ScoreList[i].ToString();
            _Scores[i].gameObject.SetActive(true);
        }
    }
    void LobbyContent_PlzAnyKey_act()
    {
        _First_view_rect.gameObject.SetActive(false);
        _Second_view_rect.gameObject.SetActive(true);

        if (PlayerInfo.Instance.ScoreList.Count == 0)
        {
            Show_PopUp_Noti("No Score");
        }

    }
    void Show_PopUp_Noti(string mss)
    {
        _Popup_Noti_rect.gameObject.SetActive(true);
        _Popup_Noti_txt.text = mss;
    }

}
