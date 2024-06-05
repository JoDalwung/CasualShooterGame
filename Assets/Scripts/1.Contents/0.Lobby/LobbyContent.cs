using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyContent : IContent
{    
    public IDialogLoader dialogLoader;
    public static event System.Action PlzAnyKey_act;
    Transform _SunLight;
    Transform _CamPoint_1, _CamPoint_2;
    Camera _MainCam;

    #region Framework
    protected override void _OnLoad()
    {
        dialogLoader.LoadDialog();
        _Caching();

    }
    protected override void _OnLoadComplete()
    {
        dialogLoader.ShowIdxDialog();
        _Init();
    }
    protected override void _OnEnter()
    {
        _AddEvent();
        StartCoroutine(_cUpdate());
    } 
    protected override void _OnExite()
    {
        dialogLoader.UnLoadDialog();
        _RemoveEvent();
    }
    void _AddEvent(){}
    void _RemoveEvent(){}
    #endregion

    void _Caching()
    {
        _SunLight = transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Transform>();
        _MainCam = transform.GetChild(1).GetChild(0).GetComponent<Camera>();
        _CamPoint_1 = transform.GetChild(1).GetChild(1).GetChild(0).GetComponent<Transform>();
        _CamPoint_2 = transform.GetChild(1).GetChild(1).GetChild(1).GetComponent<Transform>();          
    }
    void _Init()
    {
        if (!PlayerInfo.Instance.PlzAnyKey)
        {
            _MainCam.transform.position = _CamPoint_1.position;
            _MainCam.transform.eulerAngles = _CamPoint_1.eulerAngles;
        }
        else
        {
            _MainCam.transform.position = _CamPoint_2.position;
            _MainCam.transform.eulerAngles = _CamPoint_2.eulerAngles;
        }
    }
    IEnumerator _cUpdate()
    {
        Vector3 _SunDirv3 = new Vector3(45.0f, 0, 0);
        while(true)
        {
            _SunLight.Rotate(_SunDirv3 * Time.deltaTime);
            if (Input.anyKeyDown)
            {
                if (!PlayerInfo.Instance.PlzAnyKey)
                {
                    PlayerInfo.Instance.PlzAnyKey = true;
                    StartCoroutine(_cfocus(true));                    
                }
            }
            yield return null;
        }
        
    }
    IEnumerator _cfocus(bool focus)
    {
        float time = 0;

        Transform StartTs, EndTs;

        StartTs = focus ? _CamPoint_1 : _CamPoint_2;
        EndTs = focus ? _CamPoint_2 : _CamPoint_1;

        while (time < 1.5f)
        {
            time += Time.deltaTime;
            _MainCam.transform.position = Vector3.Lerp(StartTs.position, EndTs.position, time / 1.5f);
            _MainCam.transform.eulerAngles = Vector3.Lerp(StartTs.eulerAngles, EndTs.eulerAngles, time / 1.5f);           
            yield return null;
        }
        PlzAnyKey_act?.Invoke();
    }

}
