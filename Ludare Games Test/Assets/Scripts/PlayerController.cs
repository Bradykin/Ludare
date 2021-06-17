using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour, IReset
{
    [SerializeField] private SpriteRenderer model;
    [SerializeField] private AudioSource deathSound;
    [SerializeField] private AudioSource enemyStompSound;
    [SerializeField] private AudioSource coinSound;
    
    private InputController inputController;
    private Collider2D collider2D;

    private Vector3 startingPosition;

    private const float movementSpeed = 4.0f;
    private const float jumpAmount = 7.0f;
    private const float fallAmount = 14.0f;

    private float jumpVelocity = 0.0f;
    private bool isGrounded = true;
    private bool hasDoubleJumped = false;
    private Collider2D groundedOnCollider = null;

    private bool isDead = false;

    // Start is called before the first frame update
    void Start()
    {
        inputController = gameObject.GetComponent<InputController>();
        collider2D = gameObject.GetComponent<Collider2D>();
        
        inputController.moveLeftAction += TryMoveLeft;
        inputController.moveRightAction += TryMoveRight;
        inputController.jumpAction += TryJump;

        startingPosition = gameObject.transform.position;
        LevelController.Instance.SubscribeResettable(this);
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead)
        {
            return;
        }

        if (groundedOnCollider == null)
        {
            isGrounded = false;
        }
        else
        {
            float distance = collider2D.Distance(groundedOnCollider).distance;

            if (distance >= 0.01f)
            {
                isGrounded = false;
            }
        }
        
        if (!isGrounded)
        {
            transform.position += Vector3.up * Time.deltaTime * jumpVelocity;
            jumpVelocity -= Time.deltaTime * fallAmount;
        }
    }

    void OnDestroy()
    {
        LevelController.Instance.UnsubscribeResettable(this);
    }

    private void TryMoveLeft()
    {
        if (isDead)
        {
            return;
        }
        
        transform.position += Vector3.left * Time.deltaTime * movementSpeed;
    }

    private void TryMoveRight()
    {
        if (isDead)
        {
            return;
        }

        transform.position += Vector3.right * Time.deltaTime * movementSpeed;
    }

    private void TryJump()
    {
        if (CanJump())
        {
            if (!isGrounded)
            {
                hasDoubleJumped = true;
            }
            isGrounded = false;
            jumpVelocity = jumpAmount;
        }
    }

    private bool CanJump()
    {
        return !hasDoubleJumped && !isDead;
    }

    private void Die()
    {
        StartCoroutine("Respawn");
    }

    private IEnumerator Respawn()
    {
        model.material.color = new Color(model.material.color.r, model.material.color.g, model.material.color.b, 0.5f);
        isDead = true;
        Vector3 respawnPoint = startingPosition;
        deathSound.Play();
        LevelController.Instance.OnRunLost();
        float ghostSpeed = 5.0f * Mathf.Max(1.0f, Vector3.Distance(transform.position, respawnPoint) / 15.0f);

        while (transform.position != respawnPoint)
        {
            if (Vector3.Distance(transform.position, respawnPoint) <= 0.1f)
            {
                transform.position = respawnPoint;
            }
            else
            {
                Vector3 direction = (respawnPoint - transform.position).normalized;
                transform.position += direction * Time.deltaTime * ghostSpeed;
            }
            yield return null;
        }

        LevelController.Instance.ResetAll();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (isDead)
        {
            return;
        }

        Vector2 collisionAngle = (col.ClosestPoint(transform.position) - (Vector2)transform.position).normalized;

        Platform platform = col.gameObject.GetComponent<Platform>();
        Hazard hazard = col.gameObject.GetComponent<Hazard>();
        Enemy enemy = col.gameObject.GetComponent<Enemy>();
        Coin coin = col.gameObject.GetComponent<Coin>();
        Flag flag = col.gameObject.GetComponent<Flag>();

        //If collided with a wall
        if (platform != null)
        {


            if (collisionAngle.x == -1.0f)
            {
                float colliderGap = collider2D.bounds.min.x - col.bounds.max.x;
                transform.position += Vector3.left * colliderGap;
            }
            else if (collisionAngle.x == 1.0f)
            {
                float colliderGap = col.bounds.min.x - collider2D.bounds.max.x;
                transform.position += Vector3.right * colliderGap;
            }

            //else if (collisionAngle.y <= -0.5f)
            else if (collider2D.bounds.min.y >= col.transform.position.y)
            {
                //Player is above
                isGrounded = true;
                hasDoubleJumped = false;
                jumpVelocity = 0.0f;
                float colliderGap = collider2D.bounds.min.y - col.bounds.max.y;
                transform.position += Vector3.down * colliderGap;
                groundedOnCollider = col;
            }
            else if (collider2D.bounds.max.y <= col.transform.position.y)
            {
                //Player is below
                jumpVelocity = 0.0f;
            }
        }
        //If collided with a hazard
        else if (hazard != null)
        {
            Die();
        }
        //If collided with an enemy
        else if (enemy != null)
        {
            if (enemy.GetIsDead())
            {
                return;
            }
            
            //If goomba stomping
            if (collider2D.bounds.min.y >= col.transform.position.y)
            {
                enemy.Die();
                enemyStompSound.Play();
                LevelController.Instance.GainScore(20);
            }
            else
            {
                Die();
            }
        }
        else if (coin != null)
        {
            coin.Collect();
            hasDoubleJumped = false;
            coinSound.Play();
            LevelController.Instance.GainScore(10);
        }
        else if (flag != null)
        {
            LevelController.Instance.OnRunEnd();
            SceneManager.LoadScene(flag.LevelName);
        }
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (isDead)
        {
            return;
        }

        Vector2 collisionAngle = (col.ClosestPoint(transform.position) - (Vector2)transform.position).normalized;

        //If collided with a wall
        if (col.gameObject.GetComponent<Platform>() != null)
        {
            if (collisionAngle.x == -1.0f)
            {
                float colliderGap = collider2D.bounds.min.x - col.bounds.max.x;
                transform.position += Vector3.left * colliderGap;
            }
            else if (collisionAngle.x == 1.0f)
            {
                float colliderGap = col.bounds.min.x - collider2D.bounds.max.x;
                transform.position += Vector3.right * colliderGap;
            }

            /*if (collider2D.bounds.min.y >= col.transform.position.y)
            {
                //Do Nothing
            }
            else if (collider2D.bounds.max.y <= col.transform.position.y)
            {
                //Do Nothing
            }
            else if (collider2D.bounds.min.x >= col.transform.position.x)
            {
                float colliderGap = collider2D.bounds.min.x - col.bounds.max.x;
                transform.position += Vector3.left * colliderGap;
            }
            else if (collider2D.bounds.max.x <= col.transform.position.x)
            {
                float colliderGap = col.bounds.min.x - collider2D.bounds.max.x;
                transform.position += Vector3.right * colliderGap;
            }*/
        }
    }

    public void Reset()
    {
        gameObject.transform.position = startingPosition;
        gameObject.SetActive(true);
        jumpVelocity = 0.0f;
        isGrounded = true;
        hasDoubleJumped = false;
        isDead = false;
        model.material.color = new Color(model.material.color.r, model.material.color.g, model.material.color.b, 1.0f);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
