using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Koko.Utilities;
public class PowerUpFreeze : MonoBehaviour, ImEatable
{
    [SerializeField]
    private PUEatEnemies myData;


    #region GAME MANAGER EVENTS

    private void OnEnable()
    {
        GameManager.instance.OnFinishGame += GameOverScreen;
    }

    private void OnDisable()
    {
        GameManager.instance.OnFinishGame -= GameOverScreen;
    }

    private void GameOverScreen(object sender, EventArgs e)
    {
        this.gameObject.SetActive(false);

    }
    #endregion
    public bool canBeEated()
    {
        return true;
    }

    public void Eaten()
    {
        GameManager.instance.EnableEatEnemiesMode(myData.TimeToEat);
        GameManager.instance.GainPoints(myData.ScoreOnEated, 2);
        KokoUtilities.CreateWorldText(new Vector2(transform.position.x, transform.position.y), 1f, myData.TextAnimation, myData.TextDirection, GameManager.instance.ReturnText(), myData.ScoreOnEated);
        this.gameObject.SetActive(false);
    }

    public uint ReturnScoreValue()
    {
        return myData.ScoreOnEated;
    }


  
}
