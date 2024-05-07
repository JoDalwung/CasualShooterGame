using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyDialog : IDialog
{

    protected override void _OnLoad()
    {
        // ui caching
    }
    protected override void _OnLoadComplete()
    {
        // ui caching Complete
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

}
