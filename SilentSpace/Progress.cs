using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;

[System.Serializable]
public class PlayerInfo
{
    public int Coins;
    public int Width;
    public int Height;
    public int Level;
    public int currentLevel;
    public int totalExperience;
    public int connectedAccount;
    public float soundVolume;
}

public class Progress : MonoBehaviour
{
    private int expirience = 0;
    private int bestExpitience = 0;
    public PlayerInfo PlayerInfo;
    [DllImport("__Internal")]
    private static extern void SaveExtern(string date);
    [DllImport("__Internal")]
    private static extern void LoadExtern();
    [SerializeField] TextMeshProUGUI _playerInfoText;
    public static Progress Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            transform.parent = null;
            DontDestroyOnLoad(gameObject);
            Instance = this;
           // LoadExtern();
        }
        else
        {
            Destroy(gameObject);
        }
/*
        if (PlayerPrefs.HasKey("Exp"))
        {
            expirience = PlayerPrefs.GetInt("Exp"));
            bestExpitience = PlayerPrefs.GetInt("BstExp"));
        }
        else
        {
            expirience = 0;
            bestExpitience = 0;
        }
            
      */  
    }

  /*  private void Update()
    {
        if (bestExpitience < expirience)
        {
            PlayerPrefs.SetInt("Exp", expirience);
            PlayerPrefs.SetInt("BstExp", expirience);

        }
        else
        {
            PlayerPrefs.SetInt("BstExp", bestExpitience);

        }
    }

    private void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            PlayerInfo = new PlayerInfo();
            Save();
        }
    } */

    public void Save()
    {
        string jsonString = JsonUtility.ToJson(PlayerInfo);
/*#if UNITY_WEBGL
        SaveExtern(jsonString);
#endif */
    }

    public void SetPlayerInfo(string value)
    {
        PlayerInfo = JsonUtility.FromJson<PlayerInfo>(value);
        if (_playerInfoText)
        {
            _playerInfoText.text = PlayerInfo.Coins + "\n" + PlayerInfo.Width + "\n" + PlayerInfo.Height + "\n" + PlayerInfo.Level + "\n" + PlayerInfo.currentLevel + "\n" + PlayerInfo.totalExperience;
        }
    }

}
