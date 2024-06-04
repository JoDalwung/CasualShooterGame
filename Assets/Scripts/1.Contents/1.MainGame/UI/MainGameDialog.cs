using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainGameDialog : IDialog
{
    public static event System.Action StartGame_act;
    public Sprite[] Tile_sp;
    
    MainGameContent _MainGameContent;
    Text Start_txt;
    RectTransform Tile;
    RectTransform[] Tiles = new RectTransform[6];


    #region Framework
    protected override void _OnLoad()
    {
        // ui caching
        Debug.Log("MainGameDialog _OnLoad");
        _MainGameContent = GameObject.Find("MainGameContent").GetComponent<MainGameContent>();
        Start_txt = transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>();

        Tile = transform.GetChild(2).GetChild(0).GetChild(1).GetComponent<RectTransform>();

        for (int i = 0; i < Tiles.Length; i++)
        {
            Tiles[i] = Tile.GetChild(i).GetComponent<RectTransform>();
        }
        
    }
    protected override void _OnLoadComplete()
    {
        // ui caching Complete
        Debug.Log("MainGameDialog _OnLoadComplete");
        Start_txt.gameObject.SetActive(false);
        Tile.gameObject.SetActive(false);
        for (int i = 0; i < Tiles.Length; i++)
        {
            Tiles[i].GetChild(0).GetChild(0).GetComponent<Image>().sprite = Tile_sp[(int)_MainGameContent._TilesList[i].Right];
            Tiles[i].GetChild(1).GetChild(0).GetComponent<Image>().sprite = Tile_sp[(int)_MainGameContent._TilesList[i].Left];
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

    }
    void _RemoveEvent()
    {

    }
    #endregion

    IEnumerator _cStartGameTimer()
    {
        Start_txt.gameObject.SetActive(true);
        Start_txt.text = $"3";
        yield return new WaitForSeconds(1.0f);
        Start_txt.text = $"2";
        yield return new WaitForSeconds(1.0f);
        Start_txt.text = $"1";
        yield return new WaitForSeconds(1.0f);
        Start_txt.text = $"Start!";
        yield return new WaitForSeconds(0.25f);
        Start_txt.gameObject.SetActive(false);
        Tile.gameObject.SetActive(true);
        StartGame_act?.Invoke();
    }



}

