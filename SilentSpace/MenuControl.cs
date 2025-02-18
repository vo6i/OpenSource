using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuControl : MonoBehaviour
{
    public GameObject controlMenu;
    public GameObject volumeMenu;
    public GameObject levelMenu;
    public GameObject charMenu;

    public Button[] levels;
    public Button[] ships;
    // Start is called before the first frame update
    private void Start()
    {
        int currentLevel = PlayerPrefs.GetInt("Lvl");
        if(currentLevel > 5)
        {
            levels[1].interactable = true;
            ships[1].interactable = true;
        }
        else
        {
            levels[1].interactable = false;
            levels[2].interactable = false;
            ships[1].interactable = false;
            ships[2].interactable = false;

        }

        if (currentLevel > 6)
        {
            levels[2].interactable = true;
            ships[2].interactable = true;
        }
    }

    public void OpenControlMenu()
    {
        controlMenu.SetActive(true);
    }

    public void CloseControlMenu()
    {
        controlMenu.SetActive(false);
    }

    public void OpenVolume()
    {
        volumeMenu.SetActive(true);
    }

    public void CloseVolume()
    {
        volumeMenu.SetActive(false);
    }

    public void Selectlevel ()
    {
        levelMenu.SetActive(true);
    }

    public void CloseSelectLevel ()
    {
        levelMenu.SetActive(false);
    }

    public void SelectChar()
    {
        charMenu.SetActive(true);
    }

    public void CloseSelectChar()
    {
        charMenu.SetActive(false);
    }
}
