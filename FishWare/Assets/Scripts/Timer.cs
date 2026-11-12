using UnityEngine;
using UnityEngine.UI;

/*
 * Chris Pimentel
 * 11-12-26
 * Handles timer behavior for minigame
 */

public class Timer : MonoBehaviour
{
    //private float totalTime;
    private float currentTime;
    private Slider slider;

    //Determines if the timer is running or not
    private bool running;

    private void Start()
    {
        slider = GetComponent<Slider>();
        gameObject.SetActive(false);

        running = false;
    }

    /// <summary>
    /// Set timer to run down as the minigame plays
    /// </summary>
    /// <param name="time"></param>
    public void SetTimer(float time)
    {
        print("Timer Set");

        //Set all values to minigame time limit
        slider.maxValue = time;
        slider.value = time;
        currentTime = time;

        //Allow timer to run
        running = true;
    }


    /// <summary>
    /// Set the timer to a full bar
    /// </summary>
    public void FillTimer()
    {
        slider.maxValue = 1;
        slider.value = 1;
    }
    

    // Update is called once per frame
    void Update()
    {
        //Check if timer is running
        if (running)
        {
            //If time is left, decrease time on timer
            if (currentTime > 0f)
            {
                currentTime -= Time.deltaTime; //Decrease time
                slider.value = Mathf.Clamp(currentTime, 0f, slider.maxValue); //Update timer on UI
            }
            else //If timer has finished
            {
                //Update timer
                currentTime = 0f;
                slider.value = 0f;

                //Timer is no longer running
                running = false;
            }
        }
    }
}
