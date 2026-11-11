using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    //List of minigames
    [Header("Minigames List")]
    [SerializeField] private Base_Minigame[] minigames;

    [Header("Screen Transition")]
    [SerializeField] private float screenChangeTime = 5f;
    [SerializeField] private SpriteRenderer backgroundScreen;

    public Dificulty dificulty { get; private set; }

    private void Awake()
    { 
        StartCoroutine(StartMinigames());
    }

    public IEnumerator StartMinigames()
    {
        StartCoroutine(ChangeScreen());
         
        yield return new WaitForSeconds(screenChangeTime);

        print("Transition ended");
    }

    private IEnumerator ChangeScreen()
    {
        Color startColor = backgroundScreen.color;
        float elapsedTime = 0f;

        while (elapsedTime < screenChangeTime)
        {
            elapsedTime += Time.deltaTime;

            float newAlpha = Mathf.Lerp(0f, 1f, elapsedTime / screenChangeTime);

            backgroundScreen.color = new Color(startColor.r, startColor.g, startColor.b, newAlpha);
            yield return null;
        }
    }
    
}
