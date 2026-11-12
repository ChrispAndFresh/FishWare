using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

/*
 * Chris Pimentel
 * 11-11-26
 * Waterfall Climb has the player tapping repeatedly to win
 */

public class WaterfallClimb : Touch_Minigame
{
    [Header("Target Numbers")]
    [SerializeField] private float targetNumberEasy = 100f;
    [SerializeField] private float targetNumberMedium = 175f;
    [SerializeField] private float targetNumberHard = 225f;
    private float targetNumber;

    [Header("Waterfall Climbing")]
    [SerializeField] private float baseSwimPower = 20f;
    private float swimPower;
    [SerializeField] private float swimDecay = 0.5f;
    private float swimMeter;

    [Header("Environment")]
    [SerializeField] private Transform waterfallTop;
    [SerializeField] private Transform waterfallBottom;
    [SerializeField] private GameObject fish;


    private void Awake()
    {
        //Set defaults
        swimMeter = 0f;
        winState = false;

        //Change target number based on dificulty
        switch (dificulty)
        {
            case Dificulty.Easy:
                targetNumber = targetNumberEasy;
                break;
            case Dificulty.Medium:
                targetNumber = targetNumberMedium;
                break;
            case Dificulty.Hard:
                targetNumber = targetNumberHard;
                break;
        }

        //Create fish
        fish = Instantiate(fish, waterfallBottom.position, Quaternion.identity, gameObject.transform);

        //Player cannot climb waterfall until game start
        swimPower = 0f;
    }

    public override IEnumerator StartGame()
    {
        //Flash Instructions

        yield return new WaitForSeconds(pauseForInstructions);

        //Remove Instructions

        swimPower = baseSwimPower;
    }

    public override void EndGame()
    {
        swimPower = 0f;
    }

    public override void OnTouch(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            swimMeter += swimPower;
            //base.OnTouch(context);
        }
    }

    private void Update()
    {
        swimMeter = Mathf.Clamp(swimMeter, 0, targetNumber);
        
        //Calculate fish position on waterfall
        float swimPercent = swimMeter / targetNumber;
        float waterfallPosition = (waterfallTop.position.y - waterfallBottom.position.y) * swimPercent;
        waterfallPosition += waterfallBottom.position.y;
        fish.transform.position = new Vector3(fish.transform.position.x, waterfallPosition, fish.transform.position.z);

        //If player has reached the top of the waterfall and has not yet "won"
        if (swimMeter >= targetNumber && !winState)
        {
            //Player has reached the win state
            winState = true;
        }

        //If player has won, leave
        if (winState)
            return;

        swimMeter -= swimDecay;
    }
}
