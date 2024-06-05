using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Tile_type
{
    Fire,
    Delay,
    Bonus,
    none,
}

public class Tiles
{
    public Tile_type Right;
    public Tile_type Left;

    public Tiles(Tile_type right, Tile_type left)
    {
        Right = right;
        Left = left;
    }
}

public class MainGameContent : IContent
{

    public static event System.Action Fire_act;
    public static event System.Action UpdateTileList_act;
    public static event System.Action<float> AddBonusValue_act;
    public static event System.Action<int> AddScore_act;
    public static event System.Action<float> AddPenalty_act;
    public static event System.Action LeaveMainGameScene_act;


    public IDialogLoader dialogLoader;
    public List<Tiles> _TilesList = new List<Tiles>();
    public float GameTime = 60.0f;
    public float BonusTimer = 10.0f;
    public int NomalScorePoint = 10;
    public int BonusScorePoint = 50;


    bool _StartGame = false;
    bool _BonusTime = false;
    int _CurrentScore = 0;
    
    Camera _MainCam;
    Transform _CamPoint_1, _CamPoint_2;
    
    #region Framework
    protected override void _OnLoad()
    {
        dialogLoader.LoadDialog();
        _StartGame = false;
        _InitTileList();       
        _Caching();
    }
    protected override void _OnLoadComplete()
    {
        StartCoroutine(_cSetUpMainCamPos());
    }
    protected override void _OnEnter()
    {
        StartCoroutine(_cUpdate());
        _AddEvent();
    }
    protected override void _OnExite()
    {
        dialogLoader.UnLoadDialog();
        _RemoveEvent();
    }
    private void _AddEvent()
    {
        MainGameDialog.StartBonusTime_act += MainGameDialog_StartBonusTime_act;
        MainGameDialog.StartGame_act += MainGameDialog_StartGame_act;
        MainGameDialog.ButtonTouch_act += MainGameDialog_ButtonTouch_act;

    }



    private void _RemoveEvent()
    {
        MainGameDialog.StartBonusTime_act -= MainGameDialog_StartBonusTime_act; 
        MainGameDialog.StartGame_act -= MainGameDialog_StartGame_act;
        MainGameDialog.ButtonTouch_act -= MainGameDialog_ButtonTouch_act;
        
    }
    #endregion

    void _InitTileList()
    {
        _CurrentScore = 0;
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
    private void _Caching()
    {
        _MainCam = transform.GetChild(1).GetChild(0).GetComponent<Camera>();
        _CamPoint_1 = transform.GetChild(1).GetChild(1).GetComponent<Transform>();
        _CamPoint_2 = transform.GetChild(1).GetChild(2).GetComponent<Transform>();
    }  
    IEnumerator _cSetUpMainCamPos()
    {
        float time = 0;
        while (time < 1.5f)
        {
            time += Time.deltaTime;
            _MainCam.transform.position = Vector3.Lerp(_CamPoint_1.position, _CamPoint_2.position, time / 1.5f);
            _MainCam.transform.eulerAngles = Vector3.Lerp(_CamPoint_1.eulerAngles, _CamPoint_2.eulerAngles, time / 1.5f);
            yield return null;
        }

        dialogLoader.ShowIdxDialog();
    }
    private void MainGameDialog_StartGame_act(bool obj) => _StartGame = obj;
    private bool _KeyDownCheck = true;
    IEnumerator _cUpdate()
    {
        while (true)
        {
            if (_StartGame && _KeyDownCheck) 
            {
                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {                   
                    _LeftKeyDownEvent();
                }
                else if (Input.GetKeyDown(KeyCode.RightArrow))
                {                   
                    _RightKeyDownEvent();
                }
            }
            yield return null;
        }

    }
    IEnumerator _WaitforKeyDown(float time)
    {
        yield return new WaitForSecondsRealtime(time);
        _KeyDownCheck = true;
    }
    private void _RightKeyDownEvent()
    {
        _KeyDownCheck = false;
        if (_TilesList[0].Right == Tile_type.Fire || _TilesList[0].Right == Tile_type.Bonus)
        {
            StartCoroutine(_WaitforKeyDown(0.1f));
            _AddScore();
        }
        else
        {
            StartCoroutine(_WaitforKeyDown(1.0f));
            _AddPenalty();
        }

        if (_BonusTime)
            _UpdateBonusTileList();
        else
            _UpdateTileList();
    }
    private void _LeftKeyDownEvent()
    {
        _KeyDownCheck = false;
        if (_TilesList[0].Left == Tile_type.Fire || _TilesList[0].Left == Tile_type.Bonus)
        {
            StartCoroutine(_WaitforKeyDown(0.1f));
            _AddScore();
        }
        else
        {
            StartCoroutine(_WaitforKeyDown(1.0f));
            _AddPenalty();
        }

        if (_BonusTime)
            _UpdateBonusTileList();
        else
            _UpdateTileList();
    }
    private void _UpdateTileList()
    {
        _TilesList.RemoveAt(0);
        int RandomNum = Random.Range(0, 2);
        Tile_type Right;
        Tile_type Left;
        Right = RandomNum == 0 ? Tile_type.Fire : Tile_type.Delay;
        Left = RandomNum == 1 ? Tile_type.Fire : Tile_type.Delay;
        _TilesList.Add(new Tiles(Right, Left));
        UpdateTileList_act?.Invoke();
    }
    private void _UpdateBonusTileList()
    {
        for (int i = 0; i < _TilesList.Count; i++)
        {
            _TilesList[i].Right = Tile_type.Bonus;
            _TilesList[i].Left = Tile_type.Bonus;
        }
        UpdateTileList_act?.Invoke();
    }
    private void _AddScore()
    {
        Fire_act?.Invoke();

        if (_TilesList[0].Left == Tile_type.Fire || _TilesList[0].Right == Tile_type.Fire)
        {
            _CurrentScore += NomalScorePoint;
            // 여기서 다이얼로그 슽라이더 벨류를 올려주는 이벤트 발생
            AddBonusValue_act?.Invoke(0.025f);
        }
        else
            _CurrentScore += BonusScorePoint;

        AddScore_act?.Invoke(_CurrentScore);
        
        Debug.Log("_AddScore");
    }
    private void MainGameDialog_StartBonusTime_act()
    {
        StartCoroutine(_cBonusTime());
    }
    IEnumerator _cBonusTime()
    {
        _BonusTime = true;
        yield return new WaitForSeconds(BonusTimer);
        _BonusTime = false;
    }
    private void _AddPenalty()
    {
        AddPenalty_act?.Invoke(1.0f);
        AddBonusValue_act?.Invoke(-0.1f);
        Debug.Log("_AddPenalty");
    }
    public void ReStartGame()
    {
        StopAllCoroutines();
        StartCoroutine(_cUpdate());
        _CurrentScore = 0;
        _BonusTime = false;
        for (int i = 0; i < _TilesList.Count; i++)
        {
            int RandomNum = Random.Range(0, 2);
            Tile_type Right;
            Tile_type Left;

            Right = RandomNum == 0 ? Tile_type.Fire : Tile_type.Delay;
            Left = RandomNum == 1 ? Tile_type.Fire : Tile_type.Delay;
            _TilesList[i] = new Tiles(Right, Left);
            Debug.Log($"{i} : left = {Left} / right = {Right}");
        }
    }
    public string GetCurrentScore()
    {
        return _CurrentScore.ToString();
    }
    private void MainGameDialog_ButtonTouch_act(bool obj)
    {
        if (_StartGame && _KeyDownCheck)
        {
            if (obj)
            {
                //left
                _LeftKeyDownEvent();
            }
            else
            {
                //right
                _RightKeyDownEvent();
            }

        }
    }
}

