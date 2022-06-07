using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Koko.Utilities;
public class PowerUpKillEnemies : MonoBehaviour, ImEatable
{
    [SerializeField]
    private PUKillEnemies myData;


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
        if(myData.KillAllEnemies)
            GameManager.instance.KillAllEnemies();
        else
            GameManager.instance.KillEnemies(myData.EnemiesToKill);

        GameManager.instance.GainPoints(myData.ScoreOnEATED, 2);
        KokoUtilities.CreateWorldText(new Vector2(transform.position.x, transform.position.y), 1f, myData.TextAnimation, myData.TextDirection, GameManager.instance.ReturnText(), myData.ScoreOnEATED);
        this.gameObject.SetActive(false);
    }

    public uint ReturnScoreValue()
    {
        return myData.ScoreOnEATED;
    }


}
