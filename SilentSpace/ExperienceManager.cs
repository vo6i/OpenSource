using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Runtime.InteropServices;

public class ExperienceManager : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void SetToLeaderboard(int value);

    [Header("Experience")]
    [SerializeField] AnimationCurve experienceCurve;

    int currentLevel, totalExperience;
    int previousLevelsExperience, nextLevelsExperience;

    [Header("Interface")]
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] TextMeshProUGUI experienceText;
    [SerializeField] Image experienceFill;
    
    

    void Start()
    {
        if (PlayerPrefs.HasKey("Exp") && PlayerPrefs.HasKey("Lvl"))
        {
            totalExperience = PlayerPrefs.GetInt("Exp");
            currentLevel = PlayerPrefs.GetInt("Lvl");
            // bestExpitience = PlayerPrefs.GetInt("BstExp"));
        }
        else
        {
            totalExperience = 0;
            currentLevel = 0;
        }
       // currentLevel = Progress.Instance.PlayerInfo.currentLevel;
       // totalExperience = Progress.Instance.PlayerInfo.totalExperience;
        UpdateLevel();
    }

   

    public void AddExperience(int amount)
    {
        totalExperience += amount;
        PlayerPrefs.SetInt("Exp", totalExperience);
      //  Progress.Instance.PlayerInfo.totalExperience += amount;
        CheckForLevelUp();
        UpdateInterface();
    }

    void CheckForLevelUp()
    {
        if(totalExperience >= nextLevelsExperience)
        {
            currentLevel++;
            PlayerPrefs.SetInt("Lvl", currentLevel);
            // Progress.Instance.PlayerInfo.currentLevel++;
            UpdateLevel();

            // Start level up sequence... Possibly vfx?
        }
    }

    void UpdateLevel()
    {
        previousLevelsExperience = (int)experienceCurve.Evaluate(currentLevel);
        nextLevelsExperience = (int)experienceCurve.Evaluate(currentLevel + 1);
        UpdateInterface();
    }

    void UpdateInterface()
    {
        int start = totalExperience - previousLevelsExperience;
        int end = nextLevelsExperience - previousLevelsExperience; 

        levelText.text = currentLevel.ToString();
        experienceText.text = start + " exp / " + end + " exp";
        experienceFill.fillAmount = (float)start / (float)end;
    }
}
