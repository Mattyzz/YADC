using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    //Speed that the player can move
    public float speed;
    Rigidbody2D playerRigidbody;    //Attach to sprite
    float inputx;

    public LayerMask wallLayer;
    public float rayLength;
    bool canJump;
    public float jumpHeight;

    bool hurt;
    public float maxHealth;

    [SerializeField]
    float health;
    public float timeBetweenDamage;
    float iframe;


    Animator anim;

    SpriteRenderer rend;

    [SerializeField]
    int coins;

    public GameObject gameoverUI;
    bool gameover;

    public Image healthImage;
    public Text coinsText;
    // Start is called before the first frame update
    private void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        canJump = true;
        health = maxHealth;
        hurt = false;
        iframe = timeBetweenDamage;
        anim = GetComponent<Animator>();
        rend = GetComponent<SpriteRenderer>();
        coins = 0;
        gameover = false;
    }

    // Update is called once per frame
    void Update()
    {
        //GetAxisRaw gets either -1 or 1
        inputx = Input.GetAxisRaw("Horizontal");

        rend.flipX = (inputx < 0);
        //Player movement left or right
        if (inputx != 0)
            playerRigidbody.AddForce(Vector2.right * inputx * speed * Time.deltaTime);

        //If player is on the ground
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, rayLength, wallLayer);

        if (hit.collider != null)
        {
            canJump = true;
        }

        if (canJump && Input.GetKeyDown(KeyCode.Space))
        {
            playerRigidbody.AddForce(Vector2.up * jumpHeight);
            canJump = false;
        }
        //Shows the raycast
        Debug.DrawRay(transform.position, Vector2.down * rayLength);

        if (iframe > 0) iframe -= Time.deltaTime;

        //Test taking damage
        if (!hurt && Input.GetKeyDown(KeyCode.LeftControl))
            takeDamage(2);

        //Smooth Health bar decrementing
        healthImage.fillAmount = Mathf.Lerp(healthImage.fillAmount, health / maxHealth, Time.deltaTime * 10);
        coinsText.text = "X " + coins.ToString();

        //Links to Animator conditions
        anim.SetBool("moving", inputx != 0);
        anim.SetBool("canJump", canJump);
        anim.SetBool("hurt", hurt);

        //Restarting the game
        if (gameover && Input.anyKeyDown)
        {
            SceneManager.LoadScene("SampleScene");
            Time.timeScale = 1f;
        }
    }

    public void takeDamage(float damageAmmount)
    {
        if (iframe < 0)
        {
            health -= damageAmmount;
            hurt = true;
            Invoke("resetHurt", 0.2f);

            if (health <= 0)
            {
                //Game Lose
                GameOver();
            }
            iframe = timeBetweenDamage;
        }


    }

    private void GameOver()
    {
        gameover = true;
        gameoverUI.SetActive(true);
        Time.timeScale = 0f;
    }

    void resetHurt()
    {
        hurt = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Coin"))
        {
            coins++;
            collision.gameObject.SetActive(false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && playerRigidbody.velocity.y < 0)
        {
            //If the player is on the enemies head
            float boundsY = collision.gameObject.GetComponent<SpriteRenderer>().bounds.size.y / 2;
            if (transform.position.y > collision.gameObject.transform.position.y + boundsY)
            {
                //Bounces of enemy head
                playerRigidbody.AddForceAtPosition(-playerRigidbody.velocity.normalized * jumpHeight / 2, playerRigidbody.position);

                /*Makes enemy health go down when the player is on the enemies head
                 * needs to be adjsuted to new enemy
                 */

               // collision.gameObject.GetComponent<EnemyController>().Damage(5f);
            }
        }
    }
}
