using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Scene : MonoBehaviour
{
    public enum SceneName
    { 
        // 여기이름이랑 해당 씬스크립트 이름이랑 동일시할 예정 
        MainScene,
        Game_1_Scene,
        Game_2_Scene,
        Game_3_Scene,
    }

    IEnumerator _cActiveScene(string SceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(SceneName);
        while (!asyncLoad.isDone)
        {
            Debug.Log(asyncLoad.progress);
            //Loading Dialog 추가 예정
            yield return null;
        }        
    }

    protected void ActiveScene(SceneName sceneName)
    {
        StartCoroutine(_cActiveScene(sceneName.ToString()));
    }

    private void OnEnable() => _OnEnter();
    private void OnDisable() => _OnExite();

    public virtual void _OnEnter()
    { }
    public virtual void _OnExite()
    { }

}
