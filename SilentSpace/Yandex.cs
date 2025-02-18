using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Yandex : MonoBehaviour
{

    [DllImport("__Internal")]
    private static extern void Hello();

    [DllImport("__Internal")]
    private static extern void GiveMePlayerData();

    [DllImport("__Internal")]
    private static extern void RateGame();

    [SerializeField] TextMeshProUGUI _nameText;
    [SerializeField] RawImage _photo;
    [SerializeField] GameObject connectMenu;
    [SerializeField] GameObject gameMenu;
    [SerializeField] GameObject looseMenu;
    public PlayerController playerController;

   

    public void RateGameButton()
    {
        RateGame();
    }

    public void HelloButton()
    {
        
        GiveMePlayerData();
        Progress.Instance.PlayerInfo.connectedAccount = 3;
        Progress.Instance.Save();
        gameMenu.SetActive(true);

    }

    public void HellodButton()
    {
        Progress.Instance.PlayerInfo.connectedAccount = 2;
        gameMenu.SetActive(true);
        Progress.Instance.Save();
      //  gameMenu.SetActive(true);
      //  GiveMePlayerData();

    }

    public void LoadGame()
    {
        SceneManager.LoadScene(1);
    }

    public void LoadLevel(int level)
    {
        SceneManager.LoadScene(level);
    }

    public void ExitGame()
    {
        SceneManager.LoadScene(0);
    }

 /*   public void CloseGameButton()
    {
        Progress.Instance.PlayerInfo.gameStats = 2;
        looseMenu.SetActive(false);
        gameMenu.SetActive(true);
        Progress.Instance.Save();
        //  GiveMePlayerData();

    }

    public void StartGameButton()
    {
        Progress.Instance.PlayerInfo.gameStats = 3;
        gameMenu.SetActive(false);
        playerController.health = 100;
        Progress.Instance.Save();
      //  gameMenu.SetActive(false);
        //  GiveMePlayerData();

    } */

    public void SetName(string name)
    {
        _nameText.text = name;
    }

    public void SetPhoto(string url)
    {
        StartCoroutine(DownloadImage(url));
    }

    IEnumerator DownloadImage(string mediaUrl)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(mediaUrl);
        yield return request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            Debug.Log(request.error);
        else
            _photo.texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
    }

}
