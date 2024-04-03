using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScene : Scene
{
    // obj
    Transform _DirectionalLight;
    Transform _CamPoint_1, _CamPoint_2;
    Camera _MainCam;

    // ui
    RectTransform _First_view , _Second_view;

    // 베이스 프레임원크 개선점
    // 컨탠츠 스크립트 / ui 스크립트의 분리의 필요성
    // 추가로 사운드 메니저의 필요성
    // Game
    bool _StartGame = false;

    private void Start()
    {
        //caching
        // obj
        _DirectionalLight = transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<Transform>();
        _MainCam = transform.GetChild(2).GetChild(0).GetComponent<Camera>();
        _CamPoint_1 = transform.GetChild(2).GetChild(2).GetChild(0).GetComponent<Transform>();
        _CamPoint_2 = transform.GetChild(2).GetChild(2).GetChild(1).GetComponent<Transform>();

        // ui
        _First_view = transform.GetChild(0).GetChild(0).GetComponent<RectTransform>();
        _Second_view = transform.GetChild(0).GetChild(1).GetComponent<RectTransform>();
        _First_view.gameObject.SetActive(true);
        _Second_view.gameObject.SetActive(false);



    }



    public override void _OnEnter()
    {
        Debug.Log("Main_onEnter");          
    }

    public override void _OnExite()
    {
        Debug.Log("Main_onExit");
    }

    void ChangeScene(SceneName Scenename) => ActiveScene(Scenename);


    IEnumerator _cfocus()
    {
        float time = 0;
        while (time < 1.5f)
        {
            time += Time.deltaTime;
            _MainCam.transform.position = Vector3.Lerp(_CamPoint_1.position, _CamPoint_2.position, time/ 1.5f);
            _MainCam.transform.eulerAngles = Vector3.Lerp(_CamPoint_1.eulerAngles, _CamPoint_2.eulerAngles, time/ 1.5f);
            yield return null;
        }

        _First_view.gameObject.SetActive(false);
        _Second_view.gameObject.SetActive(true);

    }


    private void Update()
    {
        _DirectionalLight.Rotate(new Vector3(45, 0, 0) * Time.deltaTime);

        if (Input.anyKey && !_StartGame)
        {
            _StartGame = true;
            StartCoroutine(_cfocus());
        }


    }

}


