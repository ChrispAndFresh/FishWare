using System.Collections;
using UnityEngine;

/*
 * Chris Pimentel
 * 11-10-26
 * Handles shared functions for each minigame, such as time & dificulty
 */

//Dificulty options
public enum Dificulty { Easy, Medium, Hard };

public abstract class Base_Minigame : MonoBehaviour
{
    //Dificulty settings
    private Dificulty dificulty;

    [Header("Game Timer")]
    //How long each minigame lasts
    [SerializeField] private float gameTimeLength = 5f;
    private float gameTimer;

    [Header("Timer Dificulty Modifiers")]
    //Time for each minigame changes based on dificulty
    [SerializeField] protected float timerModifierEasy = 1f;
    [SerializeField] protected float timerModifierMedium = 0.75f;
    [SerializeField] protected float timerModifierHard = 0.5f;

    //Determines if the mingame was Won/Lost
    protected bool winState = false;
    
    //Reference to GameManager
    private GameManager gameManager;

    private void Awake()
    {
        //Get dificulty
        dificulty = FindAnyObjectByType<GameManager>().dificulty;

        //Change gameTimer based on dificulty
        switch (dificulty) 
        { 
            case Dificulty.Easy:
                gameTimer = gameTimeLength * timerModifierEasy;
                break;
            case Dificulty.Medium:
                gameTimer = gameTimeLength * timerModifierMedium;
                break;
            case Dificulty.Hard:
                gameTimer = gameTimeLength * timerModifierHard;
                break;
        }

        print("Time Length: " + gameTimer + " seconds");

        //StartTimer
        StartCoroutine(RunTimer());
    }


    private IEnumerator RunTimer()
    {
        yield return new WaitForSeconds(gameTimer);

        //End Minigame
        print("Minigame Ended");
        print("Time Elapsed: " + Time.time);
    }

}
