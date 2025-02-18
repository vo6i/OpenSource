using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunSwitcher : MonoBehaviour
{
  //  [SerializeField] private Transform player;
    public Text scoreText;
    public int score;
    public int runScore;

    [SerializeField] private GameObject gun1;
    [SerializeField] private GameObject gun2;
    [SerializeField] private GameObject gun3;

    
    // [SerializeField] private GameObject fireTouch;

    [SerializeField] private PistolGun[] pistolGuns;

    public bool haveGun = false;
    private int gunNumber = 0;
    private int maxAmmo = 8;
    private int scoreIndex = 80;

   // private string playerAm = "Ammo";
  //  private string playerAam = "Ammo1";
    // float scoreTimer = 0f;
    //Start is called before the first frame update
    void Start()
    {
        haveGun = false;
        gunNumber = 0;
        // scoreTimer += Time.deltaTime;
        score = 0;

    }

    void Update()
    {
        if (haveGun == false)
        {
            gunNumber = 0;
        }

        if (gunNumber == 0)
        {
            gun1.SetActive(true);
            gun2.SetActive(false);
            gun3.SetActive(false);
            //  fireTouch.SetActive(false);
            if (scoreText != null) 
                scoreText.text = score.ToString(); //((int)(player.position.z)).ToString();
        }

        if (gunNumber == 1)
        {
            gun1.SetActive(false);
            gun2.SetActive(true);
            gun3.SetActive(false);
            // fireTouch.SetActive(true);
        }

        if (gunNumber == 2)
        {
            gun1.SetActive(false);
            gun2.SetActive(false);
            gun3.SetActive(true);
            // fireTouch.SetActive(true);
        }

        if (Progress.Instance.PlayerInfo.gameOver == 0 && GameLogic.spawntime >= GameLogic.zombieIndex)
        {
            score += scoreIndex;
           // scoreTimer = 0f;
        }
        //new lines 10.09.24
        else
        {
            runScore = score;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(GameLogic.amWp))
        {
            haveGun = true;
            gunNumber = 1;
            for (int i = 0; i < pistolGuns.Length; i++)
                pistolGuns[i].ammo = maxAmmo;
            // Destroy(other.gameObject);
        }

        if (other.CompareTag(GameLogic.amVp))
        {
            haveGun = true;
            gunNumber = 2;
            for (int i = 0; i < pistolGuns.Length; i++)
                pistolGuns[i].ammo = maxAmmo;
            // Destroy(other.gameObject);
        }
    }

    
}
