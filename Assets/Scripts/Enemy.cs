using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Koko.Utilities;
using System;

public class Enemy : MonoBehaviour, ImEatable
{
    [SerializeField]
    private EnemyVariants enemyData;
    private EnemiesStats myStats;

    private Vector2 moveDirection;
    private float timerDirection;
    private bool canMove;

    public bool CanMove { set => canMove = value; }
    
    #region init
    private void OnEnable()
    {
        GameManager.instance.OnStartGame += GameStarted;
        GameManager.instance.OnPause += GamePaused;
        GameManager.instance.OnUnPause += GameUnPaused;
        GameManager.instance.OnFinishGame += GameOverScreen;


        if(GameManager.instance.RandomizeEnemies)
        {
            myStats = enemyData.Variants[UnityEngine.Random.Range(0, enemyData.Variants.Count)];
        }
        else
        {
            myStats = enemyData.Variants[0];
        }

        timerDirection = myStats.WalkTime;
        if (!MoveableEnemy())
        {
            if (myStats.CanMoveHorizontally)
            {
                moveDirection = Vector2.right;
            }
            else
            {
                moveDirection = Vector2.up;
            }
        }
    }

    private void OnDisable()
    {
        GameManager.instance.OnStartGame -= GameStarted;
        GameManager.instance.OnPause -= GamePaused;
        GameManager.instance.OnFinishGame -= GameOverScreen;
        GameManager.instance.OnUnPause -= GameUnPaused;
    }
    #endregion

    #region GameEvents
    private void GameUnPaused(object sender, EventArgs e)
    {
       canMove = true;
    }

    private void GameOverScreen(object sender, EventArgs e)
    {
        this.gameObject.SetActive(false);
        canMove = false;
    }

    private void GamePaused(object sender, EventArgs e)
    {
        canMove = false;
    }

    private void GameStarted(object sender, EventArgs e)
    {
        canMove = true;
    }
    #endregion

    #region EnemyLogic

    private void Update()
    {
        if (!canMove)
            return;
        ApplyMovement();
        CheckTimers();
    }

    void CheckTimers()
    {
        //if it enters ehre, it means the enemy is static
        if (MoveableEnemy())
            return;

        timerDirection -= Time.deltaTime;
        if(timerDirection <=0)
        {
            timerDirection = myStats.WalkTime;
            int rand = UnityEngine.Random.Range(0, 100);
            CheckDirectionToMove(rand);
          
        }
    }

    //asigns a direction for the enemy, perhaps i should add the behaviour for the enmies that go on a designed path
    //so theres
    void CheckDirectionToMove(int rand)
    {
        //enemies that can move freely go here
        if (myStats.CanMoveHorizontally && myStats.CanMoveVertically)
        {
            if (rand <= 25)
                moveDirection = Vector2.left;
            else if (rand > 25 && rand <= 50)
                moveDirection = Vector2.right;
            else if (rand > 50 && rand <= 75)
                moveDirection = Vector2.up;
            else if (rand > 75 && rand <= 100)
                moveDirection = Vector2.down;
        }
        else
        //Enemies that can only move horizontally or vertically   
        {
            moveDirection = -moveDirection;
        
        }
        /*else
        //Enemies that can only move vertically
        if (myStats.CanMoveVertically)
        {
            if (rand < 50)
                moveDirection = Vector2.up;
            else
                moveDirection = Vector2.down;
        }*/
    }


    void ApplyMovement()
    {
        //if it enters ehre, it means the enemy is static
        if (MoveableEnemy())
            return;

        transform.Translate(moveDirection * myStats.MovementSpeed * Time.deltaTime, Space.World);
    }
    #endregion

    bool MoveableEnemy()
    {
        return !myStats.CanMoveHorizontally && !myStats.CanMoveVertically;
    }

    #region INTERFACE EATABLE
    public bool canBeEated()
    {
        return false;
    }

    public void Eaten()
    {
        GameManager.instance.GainPoints(myStats.ScoreWhenEaten, 1);
        KokoUtilities.CreateWorldText(new Vector2(transform.position.x, transform.position.y), 1f, myStats.TextAnimation, myStats.TextDirection, GameManager.instance.ReturnText(),myStats.ScoreWhenEaten);
        this.gameObject.SetActive(false);
    }

    public uint ReturnScoreValue()
    {
        throw new System.NotImplementedException();
    }
    #endregion
}
