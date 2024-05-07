using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : IScene
{

    // 어떤식으로 쓸지 미정
    public List<IScene.SceneName> SceneList;    
    //

    public IScene.SceneName EntryScene;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        PlayerInfo.Instance.Name = "Player";     
    }

    private void Start() => ActiveScene(EntryScene);

    protected override void _OnEnter() => _AddEvent();
    protected override void _OnExite() => _RemoveEvent();

    void _AddEvent()
    {


    }



    void _RemoveEvent()
    {
    

    }





}
