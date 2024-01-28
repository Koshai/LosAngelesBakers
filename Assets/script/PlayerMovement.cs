using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public AudioSource backAudioSource;
    Animator animate;
    bool isMoving;

    public float speed = 1f;
    
    private Rigidbody2D rb;

    public PlayerController playerController;

    public GameObject elbow;
    public GameObject cup;

    private bool hasPressedZ = false;
    private bool hasPressedX = false;

    Vector3 currentDir;
    Vector3 lastWalkVector;

    float tapAgainToRunTime = 0.2f;

    public bool canRun = true;

    float lastWalk;

    bool isAttackingAnim = false;

    bool isHit = false;

    bool gameoverStarts = false;

    void Start()
    {
        backAudioSource.Play();
        rb = GetComponent<Rigidbody2D>();
        animate = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        Vector3 moveVector = currentDir * speed;
        if (!isAttackingAnim)
        {
            rb.MovePosition(transform.position + moveVector * Time.fixedDeltaTime);
        }
    }

    void Update()
    {
        if (!playerController.isAlive)
        {
            animate.SetBool("isMove", false);
            animate.SetBool("isAttack", false);
            animate.SetBool("isHit", false);
            Debug.Log("Player defeated");
            if (!gameoverStarts)
            {
                gameoverStarts = true;
                Destroy(rb);
                playerController.GetDeadSound();
                StartCoroutine("ShowPlayerEnding");
            }
            
            return;
        }
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        currentDir = new Vector3(h, v, -0.2f);
        currentDir.Normalize();

        if (!isAttackingAnim)
        {
            if ((v == 0 && h == 0))
            {
                isMoving = false;
            }
            else if (!isMoving && (v != 0 || h != 0))
            {
                isMoving = true;
                float dotProduct = Vector3.Dot(currentDir, lastWalkVector);

                if (canRun && Time.time < lastWalk + tapAgainToRunTime && dotProduct > 0)
                {
                    speed = 10f;
                }
                else
                {
                    speed = 5f;
                    if (h != 0)
                    {
                        lastWalkVector = currentDir;
                        lastWalk = Time.time;
                    }
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

            transform.position = new Vector3(transform.position.x, -4.7f,0);
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

        if (Input.GetKeyDown(KeyCode.Z) && !hasPressedZ)
        {
            if (elbow != null)
            {
                elbow.SetActive(true);
            }

            hasPressedZ = true;

            StartCoroutine(MakeElbowDisappear());
        }

        if (Input.GetKeyUp(KeyCode.Z) && hasPressedZ)
        {
            hasPressedZ = false;
        }

        if (Input.GetKeyDown(KeyCode.X) && !hasPressedX)
        {
            if (cup != null)
            {
                cup.SetActive(true);
            }

            hasPressedX = true;

            StartCoroutine(MakeCupDisappear());
        }

        if (Input.GetKeyUp(KeyCode.X) && hasPressedX)
        {
            hasPressedX = false;
        }
    }

    IEnumerator MakeElbowDisappear()
    {
        yield return new WaitForSeconds(0.2f);

        if (elbow != null)
        {
            elbow.SetActive(false);
        }
    }

    IEnumerator MakeCupDisappear()
    {
        yield return new WaitForSeconds(0.2f);

        if (cup != null)
        {
            cup.SetActive(false);
        }
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "EnemyElbow")
        {
            if (!isHit)
            {
                isHit = true;
                Debug.Log("Enemy elbow hit");
                playerController.GetHitSound();
                playerController.SetHealthBar(0.10f);
                animate.SetBool("isHit", true);
            }
            
        }
    }

    public void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.tag == "EnemyElbow")
        {
            if (isHit)
            {
                isHit = false;
                Debug.Log("Enemy elbow hit out");
                animate.SetBool("isHit", false);
            }
        }
    }

    IEnumerator ShowPlayerEnding()
    {
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(6);

    }
}
