using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IReset
{
    private Vector3 startingPosition;
    private Vector3 startingWalkDirection;
    private Animator animator;

    [SerializeField] private Vector3 walkDirection = Vector3.left;
    [SerializeField] private int walkingSpeed = 2;
    [SerializeField] private float wanderRange = 1.5f;

    private bool isDead = false;
    
    // Start is called before the first frame update
    void Start()
    {
        startingPosition = gameObject.transform.position;
        startingWalkDirection = walkDirection;
        animator = gameObject.GetComponent<Animator>();

        LevelController.Instance.SubscribeResettable(this);
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead)
        {
            return;
        }

        if (walkDirection == Vector3.left && transform.position.x <= startingPosition.x - wanderRange)
        {
            walkDirection = Vector3.right;
        }
        else if (walkDirection == Vector3.right && transform.position.x >= startingPosition.x + wanderRange)
        {
            walkDirection = Vector3.left;
        }
        transform.position += walkDirection * walkingSpeed * Time.deltaTime;
    }

    void OnDestroy()
    {
        LevelController.Instance.UnsubscribeResettable(this);
    }

    public bool GetIsDead()
    {
        return isDead;
    }

    public void Die()
    {
        Hide();
    }

    public void Reset()
    {
        animator.SetBool("Die", false);
        isDead = false;
        gameObject.transform.position = startingPosition;
        walkDirection = startingWalkDirection;
    }

    public void Hide()
    {
        animator.SetBool("Die", true);
        isDead = true;
    }
}
