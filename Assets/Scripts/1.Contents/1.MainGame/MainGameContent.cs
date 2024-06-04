using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Tile_type
{
   Fire,
   Delay,
   none,
}
    
public class Tiles
{
    public Tile_type Right;
    public Tile_type Left;    
    
    public Tiles(Tile_type right, Tile_type left)
    {
        Right = right;
        Left  =  left;
    }
}

public class MainGameContent : IContent
{
    public IDialogLoader dialogLoader;

    bool _StartGame = false;   
    public List<Tiles> _TilesList = new List<Tiles>();


    protected override void _OnLoad()
    {
        dialogLoader.LoadDialog();
        _StartGame = false;
        //Set TileList
        _SetTileList();

    }
    #region Framework
    protected override void _OnLoadComplete()
    {
        dialogLoader.ShowIdxDialog(); 
    }

    protected override void _OnEnter()
    {
        _AddEvent();
    } 
    protected override void _OnExite()
    {
        dialogLoader.UnLoadDialog();
        _RemoveEvent();
    }

    void _AddEvent()
    {
        MainGameDialog.StartGame_act += MainGameDialog_StartGame_act;
    
    }

    void _RemoveEvent()
    {
        MainGameDialog.StartGame_act -= MainGameDialog_StartGame_act;
    }
    #endregion

    private void MainGameDialog_StartGame_act()
    {
        _StartGame = true;
    }

    void _SetTileList()
    {
        for (int i = 0; i < 6; i++)
        {
            int RandomNum = Random.Range(0, 2);
            Tile_type Right;
            Tile_type Left;

            Right = RandomNum == 0 ? Tile_type.Fire : Tile_type.Delay;
            Left = RandomNum == 1 ? Tile_type.Fire : Tile_type.Delay;
            _TilesList.Add(new Tiles(Right, Left));
            Debug.Log($"{i} : left = {Left} / right = {Right}");
        }

    }
}

