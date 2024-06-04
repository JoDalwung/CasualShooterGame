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
    List<RectTransform> Tiles = new List<RectTransform>();


    #region Framework
    protected override void _OnLoad()
    {
        // ui caching
        Debug.Log("MainGameDialog _OnLoad");
        _MainGameContent = GameObject.Find("MainGameContent").GetComponent<MainGameContent>();
        Start_txt = transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>();

        Tile = transform.GetChild(2).GetChild(0).GetChild(1).GetComponent<RectTransform>();

        for (int i = 0; i < 6; i++)
        {
            Tiles.Add(Tile.GetChild(i).GetComponent<RectTransform>());
        }
        
    }
    protected override void _OnLoadComplete()
    {
        // ui caching Complete
        Debug.Log("MainGameDialog _OnLoadComplete");
        Start_txt.gameObject.SetActive(false);
        Tile.gameObject.SetActive(false);
        for (int i = 0; i < Tiles.Count; i++)
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
        MainGameContent.UpdateTileList_act += MainGameContent_UpdateTileList_act;
    }

    void _RemoveEvent()
    {
        MainGameContent.UpdateTileList_act -= MainGameContent_UpdateTileList_act;
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



    private void MainGameContent_UpdateTileList_act()
    {
        //StopAllCoroutines();
        StartCoroutine(_cMoveTiles(0.2f));
    }

    int SetNum = 0;
    
    IEnumerator _cMoveTiles(float duration)
    {

        Tiles[0].gameObject.SetActive(false);
        
        Tiles[0].anchoredPosition = new Vector2(0, 1450);
        Tiles[0].GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector2(395, -246);
        Tiles[0].GetChild(1).GetComponent<RectTransform>().anchoredPosition = new Vector2(-395, -246);
        Tiles[0].gameObject.SetActive(true);

        for (int i = 0; i < 5; i++)
        { 
            var Tmpe = Tiles[i];
            Tiles[i] = Tiles[i + 1];
            Tiles[i + 1] = Tmpe;
        }
        float time = 0;        
        while (time < duration)
        {
            time += Time.deltaTime;

            for (int i = 0; i < 6; i++)
            {
                Tiles[i].anchoredPosition = Vector2.Lerp(new Vector2(0, i * 250 + 250), new Vector2(0, (i-1) * 250 + 250) , time/ duration);
                if (i == 0)
                {
                    Tiles[i].GetChild(0).GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(new Vector2(395, -246) , new Vector2(335, -246) , time/ duration);
                    Tiles[i].GetChild(1).GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(new Vector2(-395, -246), new Vector2(-335, -246), time / duration);
                }            
            }
            yield return null;
        }

        yield return new WaitForEndOfFrame();

        for (int i = 0; i < Tiles.Count; i++)
        {
            Tiles[i].GetChild(0).GetChild(0).GetComponent<Image>().sprite = Tile_sp[(int)_MainGameContent._TilesList[i].Right];
            Tiles[i].GetChild(1).GetChild(0).GetComponent<Image>().sprite = Tile_sp[(int)_MainGameContent._TilesList[i].Left];
        }

        yield return null;

    }



}

