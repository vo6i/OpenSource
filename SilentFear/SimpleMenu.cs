using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class SimpleMenu : MonoBehaviour
{
    public TMP_Text tMP_Text;
    private int score;
    public TMP_Text rankText;
    public TMP_Text missionTxt;

    //new lines 10.09.24
    private void Start()
    {
        int score = PlayerPrefs.GetInt("Score");
        tMP_Text.text = score.ToString();

        if(score < 500)
        {
            rankText.text = "Amauter";
            missionTxt.text = "500";
        }

        if (score > 500 && score < 750)
        {
            rankText.text = "EZ";
            missionTxt.text = "750";
        }

        if (score > 750 && score < 950)
        {
            rankText.text = "SkUF";
            missionTxt.text = "950";
        }

        if (score > 950 && score < 1250)
        {
            rankText.text = "Pahom";
            missionTxt.text = "1250";
        }

        if (score > 1250 && score < 1500)
        {
            rankText.text = "Fufel";
            missionTxt.text = "1500";
        }

        if(score > 1500)
        {
            rankText.text = "Undefined";
            missionTxt.text = "Err0r4O4";
        }
        // StartCoroutine(ScoreDeath());
    }

    //new lines 10.09.24
    private void Update()
    {

        int score = PlayerPrefs.GetInt("Score");
        tMP_Text.text = score.ToString();
        if (score < 500)
        {
            rankText.text = "Amauter";
            missionTxt.text = "500";
        }

        if (score > 500 && score < 750)
        {
            rankText.text = "EZ";
            missionTxt.text = "750";
        }

        if (score > 750 && score < 950)
        {
            rankText.text = "SkUF";
            missionTxt.text = "950";
        }

        if (score > 950 && score < 1250)
        {
            rankText.text = "Pahom";
            missionTxt.text = "1250";
        }

        if (score > 1250 && score < 1500)
        {
            rankText.text = "Fufel";
            missionTxt.text = "1500";
        }

        if (score > 1500)
        {
            rankText.text = "Undefined";
            missionTxt.text = "Err0r4O4";
        }

    }
   
    public void LoadScene()
    {
        SceneManager.LoadScene(1);
       // Progress.Instance.PlayerInfo.gameOver = 0;
    }

    public void MenuScene()
    {
        Time.timeScale = 1f;
        AudioListener.volume = 1f;
        SceneManager.LoadScene(0);
      //  tMP_Text.text = Progress.Instance.PlayerInfo.bestScore.ToString();
    }

    //new lines 10.09.24
   /* private IEnumerator ScoreDeath()
    {
        yield return new WaitForSeconds(2.5f);
        // enemyPool.Release(this);
        score = Progress.Instance.PlayerInfo.bestScore;
        tMP_Text.text = score.ToString();
        StopCoroutine(ScoreDeath());
    } */
}
