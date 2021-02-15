using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Timer")]
    public TextMeshProUGUI timer;
    public float timeRemaining;
    public bool isTimerRunning = false;

    public List<Button> buttons = new List<Button>();
    public WebRequest webRequest;
    public Canvas[] Canvases = new Canvas[3];
    private int battleid;
    private int playerid;

    void Start()
    {
        isTimerRunning = true;
    }

    void Update()
    {
        Timer();

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
        }
        else
        {
            Debug.Log("INCORRECT");
        }

        webRequest.get_question(webRequest._questionID);
    }

    public void PlayButton()
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
        Canvases[2].gameObject.SetActive(true);
    }

}
