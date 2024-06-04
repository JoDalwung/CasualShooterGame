using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainGameDialog : IDialog
{
    public static event System.Action StartGame_act;
    public Sprite[] Tile_sp;
    
    MainGameContent _MainGameContent;
    Text _Start_txt , _Score_txt;
    RectTransform _Tile;
    List<RectTransform> _TileList = new List<RectTransform>();

    RectTransform _DelayBar_Slider;
    RectTransform _DelayBar_Slider_Fill;

    Slider _BonusTime_Slider;

    #region Framework
    protected override void _OnLoad()
    {
        // ui caching
        Debug.Log("MainGameDialog _OnLoad");
        _MainGameContent = GameObject.Find("MainGameContent").GetComponent<MainGameContent>();
        _Start_txt = transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>();
        _Score_txt = transform.GetChild(1).GetChild(1).GetChild(0).GetChild(2).GetChild(1).GetComponent<Text>();
        _Tile = transform.GetChild(2).GetChild(0).GetChild(0).GetComponent<RectTransform>();

        _DelayBar_Slider = transform.GetChild(1).GetChild(2).GetChild(0).GetComponent<RectTransform>();
        _DelayBar_Slider_Fill = _DelayBar_Slider.GetChild(1).GetComponent<RectTransform>();

        _BonusTime_Slider = transform.GetChild(1).GetChild(3).GetChild(0).GetComponent<Slider>();



        for (int i = 0; i < 6; i++)
        {
            _TileList.Add(_Tile.GetChild(i).GetComponent<RectTransform>());
        }
        
    }
    protected override void _OnLoadComplete()
    {
        // ui caching Complete
        Debug.Log("MainGameDialog _OnLoadComplete");
        _Start_txt.gameObject.SetActive(false);
        _Tile.gameObject.SetActive(false);
        _DelayBar_Slider.gameObject.SetActive(false);
        _BonusTime_Slider.value = 0.0f;

        for (int i = 0; i < _TileList.Count; i++)
        {
            _TileList[i].GetChild(0).GetChild(0).GetComponent<Image>().sprite = Tile_sp[(int)_MainGameContent._TilesList[i].Right];
            _TileList[i].GetChild(1).GetChild(0).GetComponent<Image>().sprite = Tile_sp[(int)_MainGameContent._TilesList[i].Left];
        }

        StartCoroutine(_cStartGameTimer());

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

    IEnumerator _cStartGameTimer()
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
        StartGame_act?.Invoke();
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

    public static event System.Action StartBonusTime_act;

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
    }



    private void MainGameContent_AddScore_act(int obj)
    {
        _Score_txt.text = obj.ToString();
    }


}

