using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class IScene : MonoBehaviour
{
    public enum SceneName
    { 
        LobbyScene,       
    }

    IEnumerator _cActiveScene(string SceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(SceneName);
        while (!asyncLoad.isDone)
        {
           // Debug.Log(asyncLoad.progress);
            yield return null;
        }        
    }

    protected void ActiveScene(SceneName sceneName)
    {
        StartCoroutine(_cActiveScene(sceneName.ToString()));
    }

    private void OnEnable() => _OnEnter();
    private void OnDisable() => _OnExite();

    protected virtual void _OnEnter()
    { }
    protected virtual void _OnExite()
    { }

}
