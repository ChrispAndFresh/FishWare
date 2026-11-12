using System.Collections;
using UnityEngine;
using TMPro;

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
    [SerializeField] protected Dificulty dificulty;

    [Header("Game Timer")]
    //How long each minigame lasts
    [SerializeField] private float gameTimeLength = 5f;
    private float gameTimer;
    [SerializeField] protected float pauseForInstructions = 1.5f;

    [Header("Timer Dificulty Modifiers")]
    //Time for each minigame changes based on dificulty
    [SerializeField] protected float timerModifierEasy = 1f;
    [SerializeField] protected float timerModifierMedium = 0.75f;
    [SerializeField] protected float timerModifierHard = 0.5f;

    [Header("Game Information")]
    [SerializeField] protected string instructions;

    //Determines if the mingame was Won/Lost
    protected bool winState = false;
    
    //Reference to GameManager
    private GameManager gameManager;

    private void Awake()
    {
        //Get dificulty
<<<<<<< HEAD
        dificulty = gameManager.dificulty;
        //Get Timer
        timerObject = FindAnyObjectByType<Timer>();

        /*
        //Update instructions UI
        if (instructionPanel.GetComponent<Canvas>() != null)
        {
            //Update text
            instructionPanel.GetComponentInChildren<TMP_Text>().text = instructions;
        }
        */

        if (timerObject != null)
            print("Timer found");
        else
            print("Timer not found");
=======
        dificulty = FindAnyObjectByType<GameManager>().dificulty;
>>>>>>> parent of 7ed8d71 (Screen Transitions complete)

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

    }

    public virtual IEnumerator StartGame()
    {
        //Flash instructions
        //StartCoroutine(FlashInstructions());

        yield return new WaitForSeconds(pauseForInstructions);

        //Allow for player control
    }

    public virtual void EndGame()
    {
        //Take away player control
    }

    private IEnumerator RunTimer()
    {
        yield return new WaitForSeconds(gameTimer);

        //End Minigame
        print("Minigame Ended");
        print("Time Elapsed: " + Time.time);
    }

    /*
    protected IEnumerator FlashInstructions()
    {
        float timeElapsed = 0f;

        while (timeElapsed < pauseForInstructions)
        {
            
            instructionPanel.SetActive(!instructionPanel.activeSelf);
            yield return new WaitForSeconds(pauseForInstructions / 4);
            timeElapsed += pauseForInstructions / 4;
        }
    }
    */
}
