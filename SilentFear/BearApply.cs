using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class BearApply : MonoBehaviour
{
    //  public GameObject controlPanel;
    // Start is called before the first frame update
    [DllImport("__Internal")]
    private static extern void OpenNewTab(string url);

    public void openIt(string url)
    {
#if !UNITY_EDITOR && UNITY_WEBGL
        OpenNewTab(url);
#endif
    }


    public void ApplyBear()
    {
        Progress.Instance.PlayerInfo.bearS = 1;
        Progress.Instance.Save();
        // This is function only for YANDEX GAMES BUT I NOT LOVE THIS   ShowAdv();
    }

    /* public void ApplyAndroidControl()
     {
         PlayerPrefs.SetInt("Control", 1);
         controlPanel.SetActive(false);
     }

     public void ApplyPCControl()
     {
         PlayerPrefs.SetInt("Control", 2);
         controlPanel.SetActive(false);
     } */
}
