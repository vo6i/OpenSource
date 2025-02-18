using System.Collections;
using System.Collections.Generic;
//using System.Runtime.InteropServices;
using UnityEngine;
using TMPro;


public class PlayerMovementTutorial : MonoBehaviour
{
   /* [DllImport("__Internal")]
    private static extern void ShowAdv();

    [DllImport("__Internal")]
    private static extern void SetToLeaderboard(int value); */

    // public QuestManager questManager;
    [Header("Movement")]
    public float moveSpeed = 3f;
    [SerializeField] private CameraShake cameraShake;

    [SerializeField] private GameObject bear;
    [SerializeField] private Transform positionBear;

  //  private string playerPn = "Tree";
  //  private string playerBl = "Enemy";
   // private string playerAm = "Ammo";
  //  private string playerIn = "Horizontal";

    //public lb_BirdController birdControl;

    //  public float groundDrag;

    //  public float jumpForce;
    //  public float jumpCooldown;
    //  public float airMultiplier;
    // bool readyToJump;

    [SerializeField] private GunSwitcher scoreScript;

    // [HideInInspector] public float walkSpeed;
    //  [HideInInspector] public float sprintSpeed;

    // [Header("Keybinds")]
    //  public KeyCode jumpKey = KeyCode.Space;

    //  [Header("Ground Check")]
    //  public float playerHeight;
    //    public LayerMask whatIsGround;
    //   bool grounded;

    [SerializeField] private Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    [SerializeField] private Rigidbody rb;
  //  float starttime = 0f;

    [SerializeField] private GameObject playerSound;
    [SerializeField] private GameObject cnvsRstrt;
    [SerializeField] private Animator animator;

    bool Right;
    int index;

    private int trX = 5;
    private int trZ = 30;
    private int spdChng = 1;
    private int mxSpd = 8;
    private int lwSdSpd = 2;
    private int bgSdSpd = 4;
    private int bgSd = 6;
    private float rotHd = 0.3f;


    // [SerializeField] private GameObject ratget;
    // [SerializeField] private Transform rvnHokd;
    int controlSelect;
    int lastRunScore;
    int health;
    int bestScore;

    private Transform playreTransfom;
    public float sideSpeed = 0.7f;

    private int leftTrigger = Animator.StringToHash("Left");
    private int rightTrigger = Animator.StringToHash("Right");
    private int leftSideTrigger = Animator.StringToHash("LFT");
    private int rightSideTrigger = Animator.StringToHash("RGHT");

    private void Start()
    {
        playreTransfom = transform;
        //new lines 10.09.24
        bestScore = PlayerPrefs.GetInt("Score");
        // starttime += Time.deltaTime;

        health = 100;
        // rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        //  readyToJump = true;
        if (Progress.Instance.PlayerInfo.bearS == 1)
        {
            Instantiate(bear, positionBear.position, Quaternion.identity);
        }

        StartCoroutine(HeightControl());

        /* if(playreTransfom.position.y < 2)
         {
             playreTransfom.position = new Vector3(0f, 2.5f, 0f);
         } */


    }

    private void Update()
    {
        if (Progress.Instance.PlayerInfo.gameOver != 1)
            MovePlayer();
        else
            moveSpeed = 0f;

        //starttime += Time.deltaTime;

        //new lines 10.09.24


        if (moveSpeed < bgSd)
        {
            playerSound.SetActive(false);
        }
        if (moveSpeed > bgSd)
        {
            playerSound.SetActive(true);
        }

        if (GameLogic.spawntime >= GameLogic.zombieIndex && moveSpeed < mxSpd)
        {
            //  playerSound.SetActive(false);
            moveSpeed += spdChng;
            sideSpeed = lwSdSpd;
          //  starttime = 0;
        }
      /*  else 
        {
            //   playerSound.SetActive(true);
            starttime += Time.deltaTime;
           // 

        } */

        if(moveSpeed >= mxSpd)
        {
            sideSpeed = bgSdSpd;
        }

        YabdeIno();
        SpeedControl();

        if (health <= 0)
        {
            moveSpeed = 0;
            lastRunScore = scoreScript.runScore;
            //lastRunScore = scoreScript.runScore;  int.Parse(scoreScript.scoreText.text.ToString());
            //   int bestScore = Progress.Instance.PlayerInfo.bestScore;
            if (bestScore < lastRunScore)
            {
                Progress.Instance.PlayerInfo.bestScore = lastRunScore;
                PlayerPrefs.SetInt("Score", lastRunScore);
               // Progress.Instance.Save();
                // This is function only for YANDEX GAMES BUT I NOT LOVE THIS  SetToLeaderboard(lastRunScore);
                //  SetToLeaderboard(Progress.Instance.PlayerInfo.bestScore);

            }
            else
            {
                Progress.Instance.PlayerInfo.bestScore = bestScore;
                PlayerPrefs.SetInt("Score", bestScore);
            }

        }
    }

    /*  private void FixedUpdate()
      {
          if (Progress.Instance.PlayerInfo.gameOver != 1)
              MovePlayer();
          else
              return;
      } */

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == GameLogic.treeBrs)
        {
            var pLayerTransform = playreTransfom.position.x;
            if (other.gameObject.transform.position.x > pLayerTransform)
            {
                animator.SetTrigger(rightTrigger);
                cameraShake.ShouldShake = true;

                rb.AddForce(-trX, rb.velocity.y, -trZ, ForceMode.Impulse);
                moveSpeed = bgSdSpd;

            }

            if (other.gameObject.transform.position.x < pLayerTransform)
            {

                animator.SetTrigger(leftTrigger);
                cameraShake.ShouldShake = true;
                rb.AddForce(trX, rb.velocity.y, -trZ, ForceMode.Impulse);
                moveSpeed = bgSdSpd;


            }

        }

        if (other.gameObject.tag == GameLogic.enmBrs)
        {
            moveSpeed = 0f;
            cnvsRstrt.SetActive(true);
            Progress.Instance.PlayerInfo.gameOver = 1;


            health = 0;


        }
    }

    private void YabdeIno()
    {
        horizontalInput = SimpleInput.GetAxis(GameLogic.plrInp);
        verticalInput = moveSpeed;
        if (horizontalInput > rotHd)
        {
            animator.SetBool(rightSideTrigger, true);
            animator.SetBool(leftSideTrigger, false);
            /*  animator.ResetTrigger("Left");
              animator.SetTrigger("Right"); */
        }

        if (horizontalInput < -rotHd)
        {

            animator.SetBool(rightSideTrigger, false);
            animator.SetBool(leftSideTrigger, true);
            /*  animator.ResetTrigger("Right");
              animator.SetTrigger("Left"); */

        }
        else if (horizontalInput < rotHd && horizontalInput > -rotHd)
        {
            animator.SetBool(leftSideTrigger, false);
            animator.SetBool(rightSideTrigger, false);
            /* animator.ResetTrigger("Right");
             animator.ResetTrigger("Left"); */
        }
    }


    private void MovePlayer()
    {
        // calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput * sideSpeed;

        //on ground
        // if(grounded)
        // rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        rb.velocity = new Vector3(horizontalInput * sideSpeed, rb.velocity.y, moveSpeed);
        // in air
        // else if(!grounded)
        // rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force); 
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        // limit velocity if needed
        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = moveSpeed * flatVel.normalized;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private IEnumerator HeightControl()
    {
        yield return new WaitForSeconds(2.5f);
        if (playreTransfom.position.y < 2)
        {
            playreTransfom.position = new Vector3(0f, 2.5f, 0f);
        }
        StopCoroutine(HeightControl());
    }
}