using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Koko.Utilities;
using System;
public class Dot : MonoBehaviour, ImEatable
{
    [SerializeField]
    private DotStats myData;
    private SpriteRenderer myRenderer;
    public DotStats DotData { get => myData; }

    private void Start()
    {
        myRenderer = GetComponent<SpriteRenderer>();
        myRenderer.color = myData.DotColor;
    }


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

        GameManager.instance.GainPoints(myData.ScoreOnEated, 0);
        KokoUtilities.CreateWorldText(new Vector2(transform.position.x,transform.position.y), 1f, myData.TextAnimation, myData.TextDirection, GameManager.instance.ReturnText(), myData.ScoreOnEated);
        this.gameObject.SetActive(false);
    }

    public uint ReturnScoreValue()
    {
        return myData.ScoreOnEated;
    }
}
