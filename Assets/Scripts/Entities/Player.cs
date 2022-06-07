using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField]
    private PlayerStats playerData;
    [SerializeField]
    private LayerMask playerLayer;
    [SerializeField]
    private float collisionSize;
    private Vector2 collisionBox => new Vector2(collisionSize,collisionSize);
    private Vector2 moveDirection;

    private bool playerCanMove;
    private bool canEatEnemies;

    public PlayerStats PlayerData { get => playerData; }
    public bool CanEatEnemies { set => canEatEnemies = value; }
    #region Init Methods
    private void Start()
    {
        playerLayer = ~playerLayer;      
    }

    private void OnEnable()
    {
        GameManager.instance.OnStartGame += GameStarted;
        GameManager.instance.OnPause += GamePaused;
        GameManager.instance.OnUnPause += GameUnPaused;
        GameManager.instance.OnFinishGame += GameOverScreen;
        transform.position = Vector2.zero;
    }

    private void OnDisable()
    {
        GameManager.instance.OnStartGame -= GameStarted;
        GameManager.instance.OnPause -= GamePaused;
        GameManager.instance.OnFinishGame -= GameOverScreen;
        GameManager.instance.OnUnPause -= GameUnPaused;
    }
    #endregion

    #region Events GameManager

    private void GameUnPaused(object sender, EventArgs e)
    {
        playerCanMove = true;
    }

    private void GameOverScreen(object sender, EventArgs e)
    {
        this.gameObject.SetActive(false);
        playerCanMove = false;
    }

    private void GamePaused(object sender, EventArgs e)
    {
        playerCanMove = false;
    }

    private void GameStarted(object sender, EventArgs e)
    {
        transform.position = Vector2.zero;
        this.gameObject.SetActive(true);
        playerCanMove = true;
    }
    #endregion

    #region PlayerLogic
    private void Update()
    {
        //return if its not intended for the playerToMove
        if (!playerCanMove)
            return;

        GetInput();
        ApplyMovement();
        CheckCollisions();
    }

    private void GetInput()
    {
        if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            moveDirection = Vector2.up;
        }
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            moveDirection = Vector2.down;
        }
        else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            moveDirection = Vector2.left;
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            moveDirection = Vector2.right;
        }
        else
        {
            moveDirection = Vector2.zero;
        }
    }
    private void ApplyMovement()
    {
        if (moveDirection == Vector2.zero)
            return;

        transform.Translate(moveDirection * playerData.MovementSpeed * Time.deltaTime, Space.World);

    }

    private void CheckCollisions()
    {
        Collider2D[] detectObjects = Physics2D.OverlapBoxAll(transform.position, collisionBox,0, playerLayer);

        if(detectObjects.Length != 0)
        {
         
            //collided with something
            if(detectObjects[0].TryGetComponent<ImEatable>(out ImEatable eatable))
            {
                if (eatable.canBeEated())
                {
                    eatable.Eaten();      
                }         
                else
                {
                    //when getting here, means an enemy touched me and he wasnt on the state of being able to be eaten
                    //therefore the player dies
                    if(!canEatEnemies)
                        GameManager.instance.GameFinish();
                    else
                        eatable.Eaten();


                }

            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireCube(transform.position, new Vector3(collisionSize, collisionSize, collisionSize));
    }
    #endregion
}
