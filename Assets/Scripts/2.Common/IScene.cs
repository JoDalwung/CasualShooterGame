using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class IScene : MonoBehaviour
{
    public enum SceneName
    { 
        LobbyScene,
        MainGameScene,
    }

    RectTransform Loading_rect;
    UnityEngine.UI.Text LoadProgress_txt;

    IEnumerator _cActiveScene(string SceneName, bool Loading = true)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(SceneName);

        asyncLoad.allowSceneActivation = !Loading;
        float progress = 0;
        if (Loading)
            Loading_rect.gameObject.SetActive(true);

        while (!asyncLoad.isDone)
        {
            if (Loading)
            {
                progress = Mathf.MoveTowards(progress, asyncLoad.progress, Time.deltaTime);
                LoadProgress_txt.text = (progress * 100).ToString("F0")+"%";
                if (progress >= 0.9f)
                {
                    // 로딩창 디자인에따라 커스텀
                    LoadProgress_txt.text = "100%";
                    //
                    asyncLoad.allowSceneActivation = true;
                    yield return new WaitForEndOfFrame();
                    Loading_rect.gameObject.SetActive(false);
                }
                
                yield return null;
            }
            else
                yield return null;
        }        
    }
    protected void ActiveScene(SceneName sceneName, bool Loading = true)
    {
        StartCoroutine(_cActiveScene(sceneName.ToString(), Loading ));
    }

    private void Awake() => _OnLoad();
    private void Start() => _OnLoadComplete();
    private void OnEnable() => _OnEnter();
    private void OnDisable() => _OnExite();


    protected virtual void _OnLoad()
    {
        Loading_rect = transform.GetChild(0).GetChild(1).GetComponent<RectTransform>();
        LoadProgress_txt = Loading_rect.GetChild(1).GetComponent<UnityEngine.UI.Text>();
        Loading_rect.gameObject.SetActive(false);
    }
    protected virtual void _OnLoadComplete()
    { }
    protected virtual void _OnEnter()
    { }
    protected virtual void _OnExite()
    { }

}
