using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : IScene
{

    // ������� ���� ����
    public List<IScene.SceneName> SceneList;    
    //

    public IScene.SceneName EntryScene;


    protected override void _OnLoad()
    {
        base._OnLoad();
        DontDestroyOnLoad(gameObject);
    }

    protected override void _OnLoadComplete()
    {
        base._OnLoadComplete();
        ActiveScene(EntryScene , false);
    }

    protected override void _OnEnter() => _AddEvent();
    protected override void _OnExite() => _RemoveEvent();

    // �ظ��ϸ� �� �̵� �̺�Ʈ�� Dialog event�� ���ƿðŰ���.
    void _AddEvent()
    {
        LobbyDialog.LobbyDialog_GameStartBtn_act += LobbyDialog_LobbyDialog_GameStartBtn_act;

    }

    void _RemoveEvent()
    {
        LobbyDialog.LobbyDialog_GameStartBtn_act -= LobbyDialog_LobbyDialog_GameStartBtn_act;
    }

    private void LobbyDialog_LobbyDialog_GameStartBtn_act() => ActiveScene(SceneName.MainGameScene, true);


}
