using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ZombieInto : MonoBehaviour
{
    [SerializeField] private BirdMove crowPref;

    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private AudioSource voiceZomb;

  //  private GameObject crowTr;
    [SerializeField] private GameObject crow;
    [SerializeField] private GameObject blood;
    [SerializeField] private Transform bloodPos;

    private int enemyHP = 100;
    // private int count = 0;

    private int trX = 2;
    private int trZ = 3;
    private int dthTm = 18;
    private int trY = 9;

    private Transform enemyPos;

    private int enemyATrigger = Animator.StringToHash("Attack");
    private int enemyDTrigger = Animator.StringToHash("death");
    private float zSpeed = -0.9f;

    private IObjectPool<ZombieInto> enemyPool;
    private IObjectPool<BirdMove> crowPool;
    public GameObject fireZomb;
    // private string playerGm = "Player";
    // private string playerTr = "Tree";
    // private string playerCm = "MainCamera";
    // private string playerBr = "Bear";
    // private string playerZb = "Zombie";
    //  private string playerSw = "Saw";

    //  private int randomNum = 0;

   // public Sprite[] spriteHeroes;
  //  public SpriteRenderer spriteRendererHero;
   // private int randomBrawl;

    public void SetPool(IObjectPool<ZombieInto> pool)
    {
        enemyPool = pool;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == GameLogic.plrSlf)
        {
            animator.SetTrigger(enemyATrigger);
        } 

        if (other.gameObject.tag == GameLogic.enmBrs)
        {
            if (other.gameObject.transform.position.x > enemyPos.position.x)
            {
                rb.AddForce(-trX, rb.velocity.y, Random.Range(trZ, trY), ForceMode.Impulse);
            }

            if (other.gameObject.transform.position.x < enemyPos.position.x)
            {
                rb.AddForce(trX, rb.velocity.y, Random.Range(trZ, trY), ForceMode.Impulse);
            }
        }

        if (other.gameObject.tag == GameLogic.treeBrs)
        {
            if (other.gameObject.transform.position.x > enemyPos.position.x)
            {
                rb.AddForce(-trX, rb.velocity.y, trZ, ForceMode.Impulse);
            }

            if (other.gameObject.transform.position.x < enemyPos.position.x)
            {
                rb.AddForce(trX, rb.velocity.y, trZ, ForceMode.Impulse);
            }
        }


    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(GameLogic.plrCm))
        {
            enemyPool.Release(this);
            // Destroy(this.gameObject);
        }

        if (other.CompareTag(GameLogic.beerBrs))
        {
            crowPool.Get();
            //Instantiate(crow, crowTr.transform.position, Quaternion.identity);
            //  Instantiate(blood, bloodPos.transform.position, Quaternion.identity);
            animator.SetTrigger(enemyDTrigger);
            StartCoroutine(AnimDeath());
            //Destroy(this.gameObject, trZ);
        }

        if (other.CompareTag(GameLogic.zmbCfs))
        {
            voiceZomb.Play();
            // voiceZomb.SetActive(true);
        }

        if (other.CompareTag(GameLogic.sawBrs))
        {
            Instantiate(blood, bloodPos.transform.position, Quaternion.identity);
            enemyPool.Release(this);
            // Destroy(this.gameObject);

        }
    }
    // Start is called before the first frame update
    void Start()
    {
        int randomFire = Random.Range(0, 2);
        if(randomFire == 0)
        {
            fireZomb.SetActive(true);
          //  randomBrawl = Random.Range(0, 11);
        }
        else if(randomFire == 1)
        {
            fireZomb.SetActive(false);
         //   randomBrawl = Random.Range(11, 21);
        } 
        
       // spriteRendererHero.sprite = spriteHeroes[randomBrawl];
        enemyPos = transform;
        crowPool = new ObjectPool<BirdMove>(PreateEnemy, OnPet, OnPelease);
      //  crowTr = GameObject.FindGameObjectWithTag("Crow");
        //  animator = GetComponent<Animator>();
        // rb = GetComponent<Rigidbody>();
        transform.Rotate(0f, 180f, 0f);
        StartCoroutine(TimeDeath());
    }

    private void OnPet(BirdMove enemy)
    {
        enemy.gameObject.SetActive(true);
     //   StartCoroutine(RandomNumb());
        Transform randomSpawnPoint = GameLogic.crowPos;
        enemy.transform.position = randomSpawnPoint.position;
    }

    private void OnPelease(BirdMove enemy)
    {
        enemy.gameObject.SetActive(false);
    }

    private BirdMove PreateEnemy()
    {
        BirdMove enemy = Instantiate(crowPref, GameLogic.crowPos.position, Quaternion.identity);
        enemy.SetPool(crowPool);
        return enemy;

    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, zSpeed);
        if (Progress.Instance.PlayerInfo.gameOver == 1)
        {
            enemyPool.Release(this);
        }

        // Destroy(this.gameObject, dthTm);
    }

    public void TakeDamage(int damageAmount)
    {

        enemyHP -= damageAmount;
        if (enemyHP <= 0)
        {

            //   Dead = true;
            crowPool.Get();
            Instantiate(blood, bloodPos.transform.position, Quaternion.identity);
            //timeManager.DoSlowmotion();
            //  tmManager.DoSlowmotion();
            animator.SetTrigger(enemyDTrigger);
            StartCoroutine(AnimDeath());
            // Destroy(this.gameObject, trZ);
            // GetComponent<CapsuleCollider>().enabled = false;
            //  Destroy(this.gameObject, 5);
        }
        else
        {
            // Instantiate(bloodEffect, enemyPosition.position, Quaternion.identity);
            // animator.SetTrigger("damage");
        }
    }

    private IEnumerator AnimDeath()
    {
        yield return new WaitForSeconds(2.5f);
        enemyPool.Release(this);
        StopCoroutine(AnimDeath());
    }

    private IEnumerator TimeDeath()
    {
        yield return new WaitForSeconds(dthTm);
        enemyPool.Release(this);
        StopCoroutine(TimeDeath());
    }

  /*  private IEnumerator RandomNumb()
    {
        yield return new WaitForSeconds(0.01f);
        randomNum = Random.Range(0, 5);
        StopCoroutine(RandomNumb());

    } */
}
