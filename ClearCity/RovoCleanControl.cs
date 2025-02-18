using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class RovoCleanControl : MonoBehaviour
{
    NavMeshAgent agent;
    public Camera cameraMotion;
    //public Transform[] cameraPoses;
    private bool OnHommie = false;
    private int energy = 100;
    private int money = 0;
    private float strttm = 0;
    private float energyReload = 0;
    public GameObject powerBank;

    public Slider energyCount;
    public Text moneyCount;
    public ParticleSystem roboVaco;
    public ParticleSystem roboVacoTri;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && energy > 0)
        {
            RaycastHit hit;

            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100))
            {
                agent.destination = hit.point;
            }
        }

        if(OnHommie == false)
        {
            cameraMotion.fieldOfView = 60f;
        }
        else
        {
            cameraMotion.fieldOfView = 20f;
        }

        if(strttm > 70)
        {
            energy -= 20;
            strttm = 0;
        }
        else
        {
            strttm += Time.deltaTime;
        }

        if(energy == 0 && energyReload > 360)
        {
            energy = 100;
            energyReload = 0;   
        }
        else
        {
            energyReload += Time.deltaTime;
        }

        energyCount.value = energy;
        moneyCount.text = "Money " + money.ToString();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("CityTrash"))
        {
            roboVaco.Play();
            roboVacoTri.Play();
            ControlRovoJob.countCollected += 15;
            energy -= 2;
            money += 1;
            Destroy(other.gameObject);
        }

        if(other.CompareTag("Homies"))
        {
            OnHommie = true;
        }

        if (other.CompareTag("PowerStation"))
        {
            if(money > 13)
            {
                money -= 13;
                energy = 100;
                powerBank.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Homies"))
        {
            OnHommie = false;
        }

        if (other.CompareTag("PowerStation"))
        {
            powerBank.SetActive(false);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Homies"))
        {
            OnHommie = true;
        }
    }
}
