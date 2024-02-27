using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScene : Scene
{

    Transform DirectionalLight;

    public override void _OnEnter()
    {
        Debug.Log("Main_onEnter");
        DirectionalLight = GameObject.Find("0.Directional Light").GetComponent<Transform>();     
        // 
    }

    public override void _OnExite()
    {
        Debug.Log("Main_onExit");
    }

    public void ChangeScene(SceneName Scenename) => ActiveScene(Scenename);




    private void Update()
    {
        DirectionalLight.Rotate(new Vector3(45, 0, 0) * Time.deltaTime);

    }

}
