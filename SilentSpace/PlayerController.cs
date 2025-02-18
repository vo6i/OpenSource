using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.InteropServices;

public class PlayerController : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void ShowAdv();
    // public Joystick input;
    public float moveSpeed = 10f;
    public float maxRotation = 25f;

    public GameObject[] GunsUnlocked;

    private Rigidbody rb;
    private float minX, maxX, minY, maxY;
    public GameObject particleSystem;
    public GameObject particleSystem1;

    public Transform fpsCam;

    public Transform bulletPos;
    public Transform bulletPos1;
    public GameObject bullet;

    public float range = 20;
   // public float strttime = 0f;
    public int health;
    public Slider slider;

    public GameObject explosion;
    public GameObject loosMenu;
    public GameObject gameMenu;

    private bool Damage = false;
    public GameObject damegeScreen;
    private float strttime = 0f;
    // public bool Die = false;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        SetUpBoundries();
        health = 100;
        if (Progress.Instance.PlayerInfo.soundVolume > 0.1f)
            AudioListener.volume = Progress.Instance.PlayerInfo.soundVolume;
        else
            AudioListener.volume = 0.5f;
        Damage = false;

        if (PlayerPrefs.GetInt("Lvl") > 5)
        {
            GunsUnlocked[0].SetActive(true);
        }

        if (PlayerPrefs.GetInt("Lvl") > 6)
        {
            GunsUnlocked[1].SetActive(true);
        }

        if (PlayerPrefs.GetInt("Lvl") > 7)
        {
            GunsUnlocked[2].SetActive(true);
        }

        if (PlayerPrefs.GetInt("Lvl") > 8)
        {
            GunsUnlocked[3].SetActive(true);
        }
    }
    
    public void Clearness()
    {
        PlayerPrefs.DeleteAll();
    }
    // Update is called once per frame
    void Update()
    {
        
        slider.value = health;
        if (SimpleInput.GetAxisRaw("Horizontal") != 0f || SimpleInput.GetAxisRaw("Vertical") != 0f)
        {
            particleSystem.SetActive(true);
            particleSystem1.SetActive(true);
        }
        else
        {
            particleSystem.SetActive(false);
            particleSystem1.SetActive(false);
        }
        MovePlayer();
        RotatePlayer();

        CalculateBoundries();
        if(Damage == true)
        {
            damegeScreen.SetActive(true);
            strttime += Time.deltaTime;
            if(strttime > 1.2f)
            {
                Damage = false;
                damegeScreen.SetActive(false);
                strttime = 0f;
            }
        }
      //  strttime += Time.deltaTime;
       // RaycastHit hit;
/*        if(strttime > 2f)
        {
            Instantiate(bullet, bulletPos.position, transform.rotation);
            Instantiate(bullet, bulletPos1.position, transform.rotation);
            strttime = 0f;
        }
        if (Physics.Raycast(fpsCam.position + fpsCam.forward, fpsCam.forward, out hit, range))
        {
            if (hit.rigidbody != null)
            {
                Instantiate(bullet, bulletPos.position, Quaternion.identity);
                Instantiate(bullet, bulletPos1.position, Quaternion.identity);
                //hit.rigidbody.AddForce(-hit.normal * impactForce);
            }

          /*  ZombieInto e = hit.transform.GetComponent<ZombieInto>();
            if (e != null)
            {
                e.TakeDamage(damageAmount);
                return;
            } 
        } */
        if(health <= 0)
        {
            // Progress.Instance.PlayerInfo.gameStats = 2;
            gameMenu.SetActive(false);
            loosMenu.SetActive(true);
           // Progress.Instance.Save();
          //  ShowAdv();
        }
        
    }

    private void RotatePlayer()
    {
        float currentX = transform.position.x;
        float newRotatinZ;

        if(currentX < 0)
        {
            //rotate negative
            newRotatinZ = Mathf.Lerp(0f, -maxRotation, currentX / minX);
        }
        else
        {
            //rotate positive
            newRotatinZ = Mathf.Lerp(0f, maxRotation, currentX / maxX);
        }

        Vector3 currentRotationVector3 = new Vector3(0f, 0f, newRotatinZ);
        Quaternion newRotation = Quaternion.Euler(currentRotationVector3);
        transform.localRotation = newRotation;
    }

    private void CalculateBoundries()
    {
        Vector3 currentPosition = transform.position;

        currentPosition.x = Mathf.Clamp(currentPosition.x, minX, maxX);
        currentPosition.y = Mathf.Clamp(currentPosition.y, minY, maxY);

        transform.position = currentPosition;
    }

    private void SetUpBoundries()
    {
        float camDistance = Vector3.Distance(transform.position, Camera.main.transform.position);
        Vector2 bottomCorners = Camera.main.ViewportToWorldPoint(new Vector3(0f, 0f, camDistance));
        Vector2 topCorners = Camera.main.ViewportToWorldPoint(new Vector3(1f, 1f, camDistance));

        //calculate the size of the gameobject
        Bounds gameObjectBouds = GetComponent<Collider>().bounds;
        float objectWidth = gameObjectBouds.size.x;
        float objectHeight = gameObjectBouds.size.y;
        


        minX = bottomCorners.x + objectWidth;
        maxX = topCorners.x - objectWidth;

        minY = bottomCorners.y + objectHeight;
        maxY = topCorners.y - objectHeight;
    }

    private void MovePlayer()
    {
        float horizontalMovement = SimpleInput.GetAxis("Horizontal");
        float verticalMovement = SimpleInput.GetAxis("Vertical");

        Vector3 movementVector = new Vector3(horizontalMovement, verticalMovement, 0f);

        rb.velocity = movementVector * moveSpeed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BulletEnemy"))
        {
           // expireienceManager.AddExperience(50);
            health -= 5;
            Damage = true;
            Destroy(other.gameObject);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            // expireienceManager.AddExperience(50);
            health -= 20;
            Instantiate(explosion, other.transform.position, Quaternion.identity);
            Damage = true;
            Destroy(other.gameObject);
        }

        if (other.gameObject.CompareTag("Meteor"))
        {
            // expireienceManager.AddExperience(50);
            health -= 40;
            Instantiate(explosion, other.transform.position, Quaternion.identity);
            Damage = true;
            Destroy(other.gameObject);
        }
    }
}
