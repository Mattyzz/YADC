using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using Unity.VisualScripting;

public class EnemyController: MonoBehaviour
{
    [SerializeField]
    protected Rigidbody2D rb2d;
    [SerializeField]
    protected Transform tran;
    [SerializeField]
    protected CapsuleCollider2D capCollider;
    [SerializeField]
    protected Animator anim;
    [SerializeField]
    protected float speed = 2f, viewRange = 10f;
    [SerializeField]
    protected float extraRaycastLength = 6f;// This is the extra length from the rigidbody that the raycast extends to detect the ground. It should be just below the rb in most cases.

    [SerializeField]
    protected CapsuleCollider2D hurtBoxCapCollider;

    public bool isMoving;

    public float damage = 1f;

    public bool isInPain;

    protected HeroKnight player;

    public Image healthImage;

    int attackFrame;

    //IFrame so you can't spam attacking
    public float timeBetweenDamage;
    float iframe;
    //Health Stuff
    public float maxHealth;
    float health;

    public float runSpeed;
    public float chaseRange;
    public float attackRange;

    public enum enemystates { move, chase, attack }
    public enemystates currentState = enemystates.move;

    protected float distance;
    public int direction;

    public float timeBetweenAttacks;
    protected float attackCooldown;

    public LayerMask wallLayer;
    public float rayLength;

    [SerializeField]
    protected float scaleX, scaleY;
    private void OnEnable()
    {
        health = maxHealth;
        direction = (Random.value >= 0.5f) ? 1 : -1;
        attackCooldown = timeBetweenAttacks;
        attackFrame = 0;
    }
    // Start is called before the first frame update
    void Awake()
    {
        iframe = timeBetweenDamage;
        rb2d = GetComponent<Rigidbody2D>();
        tran = GetComponent<Transform>();
        capCollider = GetComponent<CapsuleCollider2D>();
        anim = GetComponent<Animator>();
        player = FindObjectOfType<HeroKnight>();

    }


    void Update()
    {

        //Health Bar Update
        healthImage.fillAmount = Mathf.Lerp(healthImage.fillAmount, health / maxHealth, Time.deltaTime * 10);

        //Iframe timer
        if (iframe > 0) iframe -= Time.deltaTime;
        else hurtBoxCapCollider.isTrigger = true;

        //Attack Cooldowns
        if (attackCooldown > 0) attackCooldown -= Time.deltaTime;

        //Calling functions based on the state of the AI
        switch (currentState)
        {
            case enemystates.move:
                Move();
                break;
            case enemystates.chase:
                Chase();
                break;
            case enemystates.attack:
                Attack();
                break;
        }

}

    //Reused Code
    public void Move()
    {
        if (direction == -1) transform.localScale = new Vector3(-scaleX, scaleY, 1f);
        else transform.localScale = new Vector3(scaleX, scaleY, 1f);
        distance = Vector2.Distance(transform.position, player.transform.position);

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right * direction, rayLength, wallLayer);
        RaycastHit2D hitDown = Physics2D.Raycast(transform.position, Vector2.right * direction - Vector2.up, rayLength, wallLayer);

        anim.SetBool("isMoving", true);
        //Flips direction of movement if mob hits the wall.
        if (hit.collider != null) direction *= -1;

        //Flips Direction if mob cannot hit player
        if (hitDown.collider == null) direction *= -1;

        //Debug.DrawRay(transform.position, Vector2.right * direction * rayLength);

        if (distance <= chaseRange)
        {
            currentState = enemystates.chase;
        }
        //Vector2.right means X since the enemy can never move in Y
        rb2d.AddForce(Vector2.right * direction * speed * Time.deltaTime);
    }

    public void Chase()
    {
        distance = Vector2.Distance(transform.position, player.transform.position);

        if (transform.position.x > player.transform.position.x) // Player on left side of enemy
        {
            direction = -1; 
            transform.localScale = new Vector3(-scaleX, scaleY, 1f);
        }
        else
        {
            direction = 1; //Enemy moves right instead
            transform.localScale = new Vector3(scaleX, scaleY, 1f);
        }
            

        //If player gets out of the chaseRange.
        if (distance >= chaseRange)
        {
            currentState = enemystates.move;
        }

        //If player gets within the attackRange.
        if (distance <= attackRange)
        {
            currentState = enemystates.attack;
        }

        rb2d.AddForce(Vector2.right * direction * runSpeed * Time.deltaTime);

    }

    public void Attack()
    {
        if (attackCooldown <= 0)
        {
            //Increment attack frame
            attackFrame++;
            if (attackFrame > 2)
                attackFrame = 1;

            //Reset Attack Frames
            anim.SetTrigger("Attack" + attackFrame);
            attackCooldown = timeBetweenAttacks;
        }
        else
            currentState = enemystates.chase;
    }


    public void takeDamage(float _damage)
    {
        //SoundScript.play("Enemy Got Hit");
        if (iframe < 0)
        {
            Debug.Log("Mob Took Damage");
            health -= _damage;
            anim.SetTrigger("In Pain");
            hurtBoxCapCollider.isTrigger = false;

            if (health <= 0)
            {
                Debug.Log("Mob has Died");
                StartCoroutine(Die());
            }
            iframe = timeBetweenDamage;
        }
    }
    private IEnumerator Die()
    {
        anim.SetTrigger("Death");
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).speed);
        Destroy(gameObject);
    }

/*    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<HeroKnight>().takeDamage(damage);
        }
    }
    */
    private void isInPainOn()
    {
        isInPain = true;
    }

    private void isInPainOff()
    {
        isInPain = false;
    }
}

