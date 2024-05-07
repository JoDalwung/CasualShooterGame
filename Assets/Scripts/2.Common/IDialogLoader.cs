using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IDialogLoader : MonoBehaviour
{    
    public List<IDialog.DialogName> Dialog_names = new List<IDialog.DialogName>();
    [HideInInspector] public List<RectTransform> DialogLayers = new List<RectTransform>();
    // 1:1 =  Loader : Dialog 
    // 하나의 컨탠츠에서 Dialog를 여러개 사용하는 경우가 있을수 있다.
    // 복잡한 게임일수록 가능성이 있으니 열어두자 
    // DialogLayers가 Dialog_names 순서를 기준으로 레이어 캐싱
    // Content에서 수동으로 ui 로드를 하자


    public void LoadDialog()
    {
        if (Dialog_names.Count > 0)
        {
            for (int i = 0; i < Dialog_names.Count; i++)
            {
                DialogLayers.Add(Instantiate(Resources.Load<RectTransform>($"Dialog/{Dialog_names[i].ToString()}")));
                DialogLayers[i].transform.SetParent(GameObject.Find("GameManager").transform.GetChild(0).GetChild(0),false);
            }
            AllHideDialog();
        }        
    }

    public void UnLoadDialog()
    {
       for (int i = 0; i < DialogLayers.Count; i++)
         Destroy(DialogLayers[i].gameObject);
        DialogLayers.Clear();
    }


    public void ShowIdxDialog(int idx = 0)
    {
        if(DialogLayers.Count > 0)
            DialogLayers[idx].gameObject.SetActive(true);
    }

    public void HideIdxDialog(int idx)
    {
        if (DialogLayers.Count > 0)
            DialogLayers[idx].gameObject.SetActive(false);
    }

    public void AllHideDialog()
    {
        for (int i=0; i < DialogLayers.Count; i++)
        {
            DialogLayers[i].gameObject.SetActive(false);
        }    
    } 


}
