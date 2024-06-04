using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IDialog : MonoBehaviour
{
    public enum DialogName
    { 
        LobbyDialog,
        MainGameDialog,
    }

    private void Awake() => _OnLoad();
    private void Start() => _OnLoadComplete();
    private void OnEnable() => _OnEnter();
    private void OnDisable() => _OnExite();

    protected virtual void _OnLoad() { }
    protected virtual void _OnLoadComplete() { }
    protected virtual void _OnEnter(){ }
    protected virtual void _OnExite(){ }

}
