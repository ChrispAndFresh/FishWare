using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

/*
 * Chris Pimentel
 * 11-11-26
 * Manages playing minigames & screen transitions
 */

public class GameManager : MonoBehaviour
{
    //List of minigames
    [Header("Minigames List")]
    [SerializeField] private Base_Minigame[] minigames;
    private Base_Minigame currentMinigame;

    [Header("Screen Transition")]
    [SerializeField] private float screenChangeTime = 1f;
    [SerializeField] private SpriteRenderer backgroundScreen;
    [SerializeField] private float screenWipeTime = 4f;
    [SerializeField] private GameObject screenWipe;
    [SerializeField] private Transform screenWipeEnd;
    [SerializeField] private float borderCreationTime = 2f;
    [SerializeField][Range(0f, 1f)] private float cornerPercentTime = 0.5f;
    [SerializeField] private GameObject[] borderCorners = new GameObject[4]; 
    [SerializeField] private Transform[] outBorderPosition = new Transform[4];
    [SerializeField] private Transform[] inBorderPosition = new Transform[4];
    [SerializeField] private GameObject[] edges1;
    [SerializeField] private GameObject[] edges2;
    [SerializeField] private GameObject[] backgroundEdges;

    private bool blackScreenUp;

    public Dificulty dificulty { get; private set; }

    int numberOfGamesLeft;

    private void Start()
    {
        //Reset Edges visibility
        for (int i = 0; i < edges1.Length; i++)
            edges1[i].SetActive(false);
        for (int i = 0; i < edges2.Length; i++)
            edges2[i].SetActive(false);
        for (int i = 0; i < backgroundEdges.Length; i++)
            backgroundEdges[i].SetActive(false);

        //Reset Corner positions
        for (int i = 0; i < borderCorners.Length; i++)
            borderCorners[i].transform.position = outBorderPosition[i].position;

        //Sets screen to false;
        blackScreenUp = false;

        //Screen Wipe is inactive
        screenWipe.SetActive(false);

        StartCoroutine(FadeScreen());
    }

    /// <summary>
    /// Fade screen to black for minigames
    /// </summary>
    /// <returns></returns>
    private IEnumerator FadeScreen()
    {
        Color startColor = backgroundScreen.color;
        float elapsedTime = 0f;

        float startAlpha;
        float endAlpha;

        if (blackScreenUp)
        {
            startAlpha = 1f;
            endAlpha = 0f;
        }
        else
        {
            startAlpha = 0f;
            endAlpha = 1f;
        }

        //Fade background
        while (elapsedTime < screenChangeTime)
        {
            elapsedTime += Time.deltaTime;

            float newAlpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / screenChangeTime);

            backgroundScreen.color = new Color(startColor.r, startColor.g, startColor.b, newAlpha);
            yield return null;
        }

        //Flips check on whether screen is up or down
        blackScreenUp = !blackScreenUp;

        //Form border
        StartCoroutine(BorderCreation());
    }

    private IEnumerator BorderCreation()
    {
        yield return new WaitForSeconds(1f);

        //Twine Borders
        for (int i = 0; i < borderCorners.Length; i++)
        {
            StartCoroutine(ElasticTween(borderCorners[i], outBorderPosition[i], inBorderPosition[i], borderCreationTime * cornerPercentTime));
        }

        yield return new WaitForSeconds((borderCreationTime * cornerPercentTime) + 1f);

        //Make edges visible
        for (int i = 0; i < edges1.Length; i++)
        {
            edges1[i].SetActive(true);
        }

        yield return new WaitForSeconds(borderCreationTime * ((1 - cornerPercentTime) / 2));

        //Make edges visible
        for (int i = 0; i < edges2.Length; i++)
        {
            edges2[i].SetActive(true);
        }

        for (int i = 0; i < backgroundEdges.Length; i++)
            backgroundEdges[i].SetActive(true);

        //Create Screenwipe
        screenWipe.SetActive(true);

        //Start Minigames
        StartMinigames();

    }


    private IEnumerator ResetBorder()
    {
        //Make black edges invisible
        for (int i = 0; i < backgroundEdges.Length; i++)
            backgroundEdges[i].SetActive(false);

        //Make edges invisible
        for (int i = 0; i < edges2.Length; i++)
        {
            edges2[i].SetActive(false);
        }

        yield return new WaitForSeconds(borderCreationTime * ((1 - cornerPercentTime) / 2));

        //Make edges invisible
        for (int i = 0; i < edges1.Length; i++)
        {
            edges1[i].SetActive(false);
        }

        yield return new WaitForSeconds(1f);

        //Twine Borders
        for (int i = 0; i < borderCorners.Length; i++)
        {
            StartCoroutine(ElasticTween(borderCorners[i], inBorderPosition[i], outBorderPosition[i], borderCreationTime * cornerPercentTime));
        }

        StartCoroutine(FadeScreen());
    }

    //Tween
    private IEnumerator ElasticTween(GameObject obj, Transform startPosition, Transform targetPosition, float tweenTime)
    {
        float elapsedTime = 0f;

        while (elapsedTime < tweenTime)
        {
            elapsedTime += Time.deltaTime;

            float t = Mathf.Clamp01(elapsedTime / tweenTime);
            float elasticT = EaseOutElastic(t);

            obj.transform.position = Vector3.LerpUnclamped(startPosition.position, targetPosition.position, elasticT);

            yield return null;
            
        }

        obj.transform.position = targetPosition.position;
    }

    //Elastic Tween
    private float EaseOutElastic(float x)
    {
        if (x == 0f) 
            return 0f;
        if (x == 1f) 
            return 1f;

        float c4 = (2f * Mathf.PI) / 4f;
        return Mathf.Pow(2f, -10f * x) * Mathf.Sin((x * 10f - 0.75f) * c4) + 1f;
    }

    /// <summary>
    /// Returns a random minigame from pool of minigames
    /// </summary>
    /// <returns></returns>
    private Base_Minigame PickRandomMinigame()
    {
        return minigames[Random.Range(0, minigames.Length)];
    }

    /// <summary>
    /// Sets number of games and starts first game
    /// </summary>
    private void StartMinigames()
    {
        //Determine the number of games played in sucsession
        switch (dificulty)
        {
            case Dificulty.Easy:
                numberOfGamesLeft = 3;
                break;
            case Dificulty.Medium:
                numberOfGamesLeft = 5;
                break;
            case Dificulty.Hard:
                numberOfGamesLeft = 7;
                break;
        }

        //Starts minigame
        StartCoroutine(PlayNextMinigame());
    }

    public IEnumerator PlayNextMinigame()
    {
        //If there are still games to be played
        if (numberOfGamesLeft != 0)
        {
            currentMinigame = Instantiate(PickRandomMinigame(), Vector3.zero, Quaternion.identity);

            StartCoroutine(ScreenWipe());

            yield return new WaitForSeconds(screenWipeTime);

            print("IEnumerator Ended");

            StartCoroutine(currentMinigame.StartGame());
        }
    }

    private IEnumerator ScreenWipe()
    {
        print("Screen Wipe Started");

        //Start screen moving up
        float elapsedTime = 0f;

        Transform startPosition = screenWipe.transform;

        while (elapsedTime < screenWipeTime)
        {
            elapsedTime += Time.deltaTime;

            float t = Mathf.Clamp01(elapsedTime / screenWipeTime);
  
            screenWipe.transform.position = Vector3.Lerp(startPosition.position, screenWipeEnd.position, t);

            yield return null;

        }

        screenWipe.transform.position = screenWipeEnd.position;

        print("Screen Wipe Ended");
    }


}
