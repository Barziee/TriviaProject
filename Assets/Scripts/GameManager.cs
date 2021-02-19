using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public List<Button> buttons = new List<Button>();
    public WebRequest webRequest;
    public Canvas[] Canvases;

    [Header("Answer Check")]
    public GameObject correct;
    public GameObject wrong;
    public AudioSource correctSound;
    public AudioSource wrongSound;

    [Header("Timer")]
    public TextMeshProUGUI timer;
    public TextMeshProUGUI WinnerName;
    public TextMeshProUGUI OpponentName;
    public float timeForRound;
    public float timeRemaining;
    public bool isTimerRunning = false;

    private int maxQuestions = 5; // 4 questions on default
    public int battleID;
    public string playerName;
    public bool isDone = false;
    public bool isPlayerOne;


    void Start()
    {
        instance = this;
        Canvases[2].gameObject.SetActive(true);
        webRequest.get_question();
        Canvases[2].gameObject.SetActive(false);
    }

    void Update()
    {
        Timer();
    }

    public IEnumerator SetSettings()
    { 
        for (int i = 1; i < 6; i++)
        {
            webRequest.CurrentDataB.Clear();
            webRequest.get_Battle(i);
            yield return new WaitForSeconds(0.4f);
            if (webRequest.CurrentDataB[1] == 0.ToString())
            {
                battleID = i;
                isPlayerOne = true;
                webRequest.SendNameRequest();
                StartCoroutine(SetOppenentName());
                yield break; 
                //fill and return
            }
            else if (webRequest.CurrentDataB[2] == 0.ToString())
            {
                battleID = i;
                isPlayerOne = false;
                webRequest.SendNameRequest(); //false is player two
                StartCoroutine(SetOppenentName());
                yield break;
                //fill and return
            }
        }   
    }

    private IEnumerator SetOppenentName()
    {
        webRequest.CurrentDataB.Clear();
        webRequest.get_Battle(battleID);
        yield return new WaitForSeconds(0.4f);
        if (webRequest.CurrentDataB[2] == 0.ToString() || webRequest.CurrentDataB[1] == 0.ToString()) 
        {
            yield return new WaitForSeconds(2f);
            StartCoroutine(SetOppenentName());
        }
        else
        {
            OpponentName.text = isPlayerOne ? "Your opponent is " + webRequest.CurrentDataB[2] : "Your opponent is " + webRequest.CurrentDataB[1];
            yield return new WaitForSeconds(3f);
            Canvases[3].gameObject.SetActive(false);
            Canvases[2].gameObject.SetActive(true);
            isTimerRunning = true;
        }     
    }

    public void Timer()
    {
        if (isTimerRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                timer.text = Mathf.Round(timeRemaining).ToString();
            }
            else
            {
                Debug.Log("Time has ran out!");
                timeRemaining = 0;
                isTimerRunning = false;
                timer.text = timeRemaining.ToString();
            }
        }
    }

    public void CheckAnswer(int index)
    {
        if (buttons[index].GetComponentInChildren<TextMeshProUGUI>().text == webRequest.Correct_Answer)
        {
            Debug.Log("CORRECT!");
            StartCoroutine(Correct());
            webRequest.GetAnswerRequest();
        }
        else
        {
            Debug.Log("INCORRECT");
            StartCoroutine(False());
        }

        if (webRequest._questionID < maxQuestions)
        {
            webRequest.get_question();
        }
        else
        {
            if(webRequest._questionID == maxQuestions)
            {
                isDone = true;
                StartCoroutine(CheckWinner());
                webRequest.SendNameRequest();
            }
        }
    } 

    void EndGame()
    {
        for (int i = 0; i < 4; i++)
        {
            Canvases[i].gameObject.SetActive(false);
        }       
        Canvases[Canvases.Length - 1].gameObject.SetActive(true);
        webRequest.GetDeleteRequest();
    }

    public IEnumerator CheckWinner()
    {
        webRequest.CurrentDataB.Clear();
        webRequest.get_Battle(battleID);
        yield return new WaitForSeconds(1f);
        int.TryParse(webRequest.CurrentDataB[5], out int P1Answers);
        int.TryParse(webRequest.CurrentDataB[6], out int P2Answers);
        int.TryParse(webRequest.CurrentDataB[7], out int playersDone);
        Debug.Log(playersDone);
        yield return new WaitForSeconds(1f);
        if(playersDone == 2)
        {
            if (P1Answers > P2Answers)
            {
                Debug.Log("p1Winner" + "answer" + "/" + P1Answers);
                EndGame();
                WinnerName.text = webRequest.CurrentDataB[1];
            }
            else if (P1Answers < P2Answers)
            {
                Debug.Log("p2Winner" + "answer" + " / " + P2Answers);
                EndGame();
                WinnerName.text = webRequest.CurrentDataB[2];
            }
            else
            {
                Debug.Log("Equals");
            }
        }
        else 
        {
            yield return new WaitForSeconds(1f);
            StartCoroutine(CheckWinner());
        }

    }

    public void PlayButton()// 0 main menu 1 connecting 2 trivia
    {
        Canvases[0].gameObject.SetActive(false);
        Canvases[1].gameObject.SetActive(true);
    }

    public void ResumeButton()
    {
        Canvases[1].gameObject.SetActive(false);
        Canvases[0].gameObject.SetActive(true);
    }

    public void Register()
    {
        Canvases[1].gameObject.SetActive(false);
        Canvases[3].gameObject.SetActive(true);
    }


    public IEnumerator Correct()
    {
        correct.SetActive(true);
        correctSound.Play(0);
        yield return new WaitForSeconds(2f);
        correct.SetActive(false);
    }

    public IEnumerator False()
    {
        wrong.SetActive(true);
        wrongSound.Play();
        yield return new WaitForSeconds(2f);
        wrong.SetActive(false);
    }

}
