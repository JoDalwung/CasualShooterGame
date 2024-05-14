using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyDialog : IDialog
{

    RectTransform _First_view_rect, _Second_view_rect;

    Button _Leaderboard_btn;
    

    RectTransform _Popup_Noti_rect;
    Text _Popup_Noti_txt;

    protected override void _OnLoad()
    {
        _Caching();
    }
    protected override void _OnLoadComplete()
    {
        _Init();
    }


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

        _Popup_Noti_rect = _Second_view_rect.GetChild(2).GetComponent<RectTransform>();
        _Popup_Noti_txt = _Popup_Noti_rect.GetChild(1).GetComponent<Text>();

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


    private void LobbyContent_PlzAnyKey_act()
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
