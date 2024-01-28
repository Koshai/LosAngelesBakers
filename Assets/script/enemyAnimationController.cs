using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class enemyAnimationController : MonoBehaviour
{
    float healthbar = 1;

    bool isHit = false;
    bool isAlive = true;

    bool isMoving = false;
    bool isAttacking = false;

    public Image healthBarImage;

    public AudioSource shaqAudioSource;

    public AudioClip[] audioClips;

    public enum EnemyAction
    {
        None,
        Wait,
        Attack,
        Chase,
        Roam
    }

    public GameObject elbow;

    Animator animate;
    public PlayerDetector detector;

    public float attackReachMin = 1f;
    public float attackReachMax = 2f;
    public float personalSpace = 0.75f;

    public float attackOffset = 2f;
    public float minTimeToFollow = 2f;
    public float maxTimeToFollow = 5f;
    public float minTimeToAttack = 1f;
    public float maxTimeToAttack = 3f;

    public GameObject[] followPoints;

    private Vector2 targetPosition;

    private bool isFollowingPlayer = false;

    private Rigidbody2D rb;

    public EnemyAction currentAction = EnemyAction.None;

    private float decisionDuration;

    GameObject playerObj;

    float speed;
    float currentSpeed;

    private float timeToFollow;
    private float timeToAttack;

    public float attackAlignmentThreshold = 2f;
    public float verticalAlignmentThreshold = 0.2f;

    float tempTime = 0.0f;

    bool gameoverStarts = false;

    AnimatorStateInfo stateInfo;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animate = GetComponent<Animator>();

        playerObj = GameObject.FindGameObjectWithTag("Player");

        SetRandomTimers();
    }

    // Update is called once per frame
    /*void Update()
    {
        animate.SetFloat("hMove", hMovement_E);
        animate.SetFloat("vMove", vMovement_E);
        animate.SetBool("isMove", detectMove_E);
        animate.SetBool("isAttack", detectAttack_E);

    }*/

    private void Wait()
    {
        animate.SetBool("isAttack", false);
        animate.SetBool("isMove", false);
        StopMovement();
    }

    private void Attack()
    {
        FaceTarget(playerObj.transform.position);
        Debug.Log("Attacking");
        animate.SetBool("isMove", false);
        animate.SetBool("isAttack", true);
        StartCoroutine("ShowElbow");
        if (stateInfo.IsName("attack") && stateInfo.normalizedTime >= 1.0f)
        {
            animate.SetBool("isAttack", false);
            isAttacking = false;
        }
    }

    

    public void FaceTarget(Vector3 targetPoint)
    {
        if (transform.position.x - targetPoint.x > 0)
        {
            this.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            this.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }

    public void StopMovement()
    {
        currentSpeed = 0;
    }

    void Update()
    {
        if (!isAlive)
        {
            animate.SetBool("isMove", false);
            animate.SetBool("isAttack", false);
            animate.SetBool("isHit", false);
            Debug.Log("Enemy defeated");
            if (!gameoverStarts)
            {
                gameoverStarts = true;
                Destroy(rb);
                StartCoroutine("ShowEnemyEnding");
            }
            return;
        }
        tempTime += Time.deltaTime;
        bool playerNearby = detector.heroIsNearby;

        if (playerNearby)
        {
            // Randomize next time to follow
            SetRandomTimers();

            // Player is nearby, start following
            isFollowingPlayer = true;
        }
        else
        {
            // Player is not nearby, roam
            isFollowingPlayer = false;
            Roam();
        }

        if (isFollowingPlayer)
        {
            if ((Mathf.Abs(followPoints[0].transform.position.x - transform.position.x) < 0.5) || (Mathf.Abs(followPoints[1].transform.position.x - transform.position.x) < 0.5))
            {
                if (tempTime > 1.5f)
                {
                    tempTime = 0;
                    Attack();
                }
            }
            else
            {
                {
                    if (!isMoving && !isAttacking)
                    {
                        isMoving = true;
                        animate.SetBool("isMove", true);
                        animate.SetBool("isAttack", false);
                    }
                    FollowPlayer();
                }
            }
        }

        if (transform.position.y > 0.85f)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);

            transform.position = new Vector3(transform.position.x, 0.85f, 0);
        }
        if (transform.position.y < -4.7f)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);

            transform.position = new Vector3(transform.position.x, -4.7f, 0);
        }
        if (transform.position.x < -15.9f)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);

            transform.position = new Vector3(-15.9f, transform.position.y, 0);
        }
        if (transform.position.x > 15.9f)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);

            transform.position = new Vector3(15.9f, transform.position.y, 0);
        }
    }

    void Roam()
    {
        animate.SetBool("isAttack", false);
        animate.SetBool("isMove", true);
        float horizontalMovement = Mathf.Sin(Time.time * 2f);
        rb.velocity = new Vector2(horizontalMovement, 0f);
    }

    void FollowPlayer()
    {
        Vector2 nearestDirectionOne = ((Vector2)followPoints[0].transform.position - (Vector2)transform.position).normalized;
        Vector2 nearestDirectionTwo = ((Vector2)followPoints[1].transform.position - (Vector2)transform.position).normalized;

        float horizontalDistanceOne = Mathf.Abs(followPoints[0].transform.position.x - transform.position.x);
        float horizontalDistanceTwo = Mathf.Abs(followPoints[1].transform.position.x - transform.position.x);

        if (horizontalDistanceOne < horizontalDistanceTwo)
        {
            rb.velocity = nearestDirectionOne * 3f;
        }
        else
        {
            rb.velocity = nearestDirectionTwo * 3f;
        }
    }

    void SetRandomTimers()
    {
        // Set random times for the next follow and attack events
        timeToFollow = Time.time + Random.Range(minTimeToFollow, maxTimeToFollow);
        timeToAttack = Time.time + Random.Range(minTimeToAttack, maxTimeToAttack);
    }

    IEnumerator ShowElbow()
    {
        if (elbow != null)
        {
            elbow.SetActive(true);
        }
        yield return new WaitForSeconds(0.5f);
        if (elbow != null)
        {
            elbow.SetActive(false);
        }
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "PlayerElbow")
        {
            if (!isHit)
            {
                isHit = true;
                Debug.Log("Player elbow hit");
                SetHealthBar(0.05f);
                animate.SetBool("isHit", true);
            }

        }

        if (collider.tag == "PlayerCup")
        {
            if (!isHit)
            {
                isHit = true;
                Debug.Log("Player cup hit");
                SetHealthBar(0.2f);
                animate.SetBool("isHit", true);
            }

        }
    }

    public void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.tag == "PlayerElbow")
        {
            if (isHit)
            {
                isHit = false;
                Debug.Log("Player cup hit out");
                animate.SetBool("isHit", false);
            }
        }

        if (collider.tag == "PlayerCup")
        {
            if (isHit)
            {
                isHit = false;
                Debug.Log("Player cup hit out");
                animate.SetBool("isHit", false);
            }
        }
    }

    public void SetHealthBar(float health)
    {
        healthbar -= health;
        healthBarImage.fillAmount = healthbar;

        if (healthbar <= 0)
        {
            isAlive = false;
            GetDeadSound();
        }
        else
        {
            GetHitSound();
        }
    }

    void GetHitSound()
    {
        shaqAudioSource.clip = audioClips[0];
        shaqAudioSource.Play();
    }

    void GetDeadSound()
    {
        shaqAudioSource.clip = audioClips[1];
        shaqAudioSource.Play();
    }

    IEnumerator ShowEnemyEnding()
    {
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(5);
    }
}
