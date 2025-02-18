using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PistolGun : MonoBehaviour
{
    public int ammo = 8;
    public int fullAmmo = 8;
    public int damageAmount = 100;
    public float impactForce = 150;

    public float range = 20;

    public Transform fpsCam;
    public ParticleSystem muzzleFlash;

    public float strttime = 0f;
    //  public GameObject otherGun;
    [SerializeField] private Animator animator;
    [SerializeField] private AudioSource sawSound;
    [SerializeField] private Collider sawcCllider;
    public Image[] ammoImages;
    public GameObject ammoPanel;
    public GunSwitcher gunSwitcher;

    private int pistolATrigger = Animator.StringToHash("Shooting");
    
    public FixedTouchField fixedTouchField;
    //public Camera camera;
    // public GameObject fireLight;
    // Start is called before the first frame update
    void Start()
    {
        ammo = 8;
        //animator = GetComponent<Animator>();
        ammoPanel.SetActive(true);
        strttime += Time.deltaTime;
    }



    // Update is called once per frame
    void Update()
    {
        ammoPanel.SetActive(true);

       // strttime += Time.deltaTime;

        for (int i = 0; i < ammoImages.Length; i++)
        {
            if (i < ammo)
            {
                ammoImages[i].enabled = true;
            }
            else
            {
                ammoImages[i].enabled = false;
            }
        }

        if (fixedTouchField.Pressed && strttime > 1 && ammo != 0)
        {
            strttime = 0;
            // fireLight.SetActive(true);
            animator.SetBool(pistolATrigger, true);
            muzzleFlash.Play();
            RaycastHit hit;
            ammo--;
            if (Physics.Raycast(fpsCam.position + fpsCam.forward, fpsCam.forward, out hit, range))
            {
                ZombieInto e = hit.transform.GetComponent<ZombieInto>();
                if (e != null)
                {
                    e.TakeDamage(damageAmount);

                    return;
                }
            }

        }
        else
        {

            animator.SetBool(pistolATrigger, false);
            strttime += Time.deltaTime;
            //    camera.fieldOfView = 50;
            // fireLight.SetActive(false);
        }

        if (ammo == 0)
        {

            //Progress.Instance.PlayerInfo.gunNumber = 0;

            ammoPanel.SetActive(false);
            gunSwitcher.haveGun = false;
            //animator.SetBool("Shooting", true);
            ammo = fullAmmo;
            //  this.gameObject.SetActive(false);

        }
    }

    public void PlaySawSound()
    {
        sawcCllider.enabled = true;
        sawSound.Play();
    }

    public void StopSawSound()
    {
        sawcCllider.enabled = false;
        sawSound.Stop();
    }
}
