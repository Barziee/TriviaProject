using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class WebRequest : MonoBehaviour
{
    public List<string> CurrentDataQ;
    public List<string> CurrentDataB;
    public bool getBattleRequestDone;
    public int _questionID = 1;
    public string Correct_Answer;
    string QUri = "https://localhost:44395/api/Question/";
    string BUri = "https://localhost:44395/api/Battle?battleid=";

    [Header("Database to Buttons")]
    public TextMeshProUGUI Question_text;
    public TextMeshProUGUI Answer1;
    public TextMeshProUGUI Answer2;
    public TextMeshProUGUI Answer3;
    public TextMeshProUGUI Answer4;


    IEnumerator GetRequestQ(string uri, int ID)
    {
        CurrentDataQ.Clear();
        string uri_finale = uri + ID;
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri_finale))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.isNetworkError)
            {
                Debug.Log("There was an error");
            }
            else
            {
                //Debug.Log(System.Convert.ToString(webRequest.downloadHandler.text));
                string question_text = System.Convert.ToString(webRequest.downloadHandler.text);
                string[] Splits = question_text.Split(':', ',', '"');
                foreach (string t in Splits)
                {
                    if (t != "" && t != "{" && t != "Answer1" && t != "Answer2" && t != "Answer3" && t != "Answer4" && t != "}" && t != "Correct_Answer" && t != "Question_text")
                    {
                        Debug.Log(t);
                        CurrentDataQ.Add(t);
                    }
                }
                ApplyListToStringQ();
            }
        }
    }

    IEnumerator GetRequestB(string uri, int ID)
    {
        getBattleRequestDone = false;
        string uri_finale = uri + ID;
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri_finale))
        {
            yield return webRequest.SendWebRequest();
            if (webRequest.isNetworkError)
            {
                Debug.Log("There was an error");
            }
            else
            {
                string Battle_text = System.Convert.ToString(webRequest.downloadHandler.text);
                string[] Splits = Battle_text.Split(':', ',', '"');
                foreach (string t in Splits)
                {
                    if (t != "" && t != "{" && t != "BattleID" && t != "PlayerOneID" && t != "PlayerTwoID" && t != "PlayerOne_Time" && t != "PlayerTwo_Time" && t != "}" && t != "PlayerOne_Answers" && t != "PlayerTwo_Answers" && t!="IsDone")
                    {   
                         CurrentDataB.Add(t);
                    }
                }
            }
        }
        getBattleRequestDone = true;
    }

    public void get_question()
    {
        GameManager.instance.timeRemaining = GameManager.instance.timeForRound;
        StartCoroutine(GetRequestQ(QUri, _questionID));
        _questionID++;
    }

    public void get_Battle(int battleid)
    {
        StartCoroutine(GetRequestB(BUri, battleid));

    }

    public void EnterName(int battleid)
    {
        StartCoroutine(GetRequestQ(BUri, battleid));

    }

    public void ApplyListToStringQ()
    {
        Question_text.text = CurrentDataQ[0];
        Answer1.text = CurrentDataQ[1];
        Answer2.text = CurrentDataQ[2];
        Answer3.text = CurrentDataQ[3];
        Answer4.text = CurrentDataQ[4];
        Correct_Answer = CurrentDataQ[5];
    }

    public void ApplyListToStringB()
    {
        Question_text.text = CurrentDataB[0];
        Answer1.text = CurrentDataB[1];
        Answer2.text = CurrentDataB[2];
        Answer3.text = CurrentDataB[3];
        Answer4.text = CurrentDataB[4];
        Correct_Answer = CurrentDataB[5];
    }

    public void SendNameRequest()
    {
        StartCoroutine(SendPlayerName());
    }

    public void GetAnswerRequest()
    {
        StartCoroutine(GetAnswer());

    }

    public void GetDeleteRequest()
    {
        StartCoroutine(GetDelete());

    }

    IEnumerator SendPlayerName()
    {
        // https://localhost:44395/api/Battle?PlayerName={PlayerName}&battleID={battleID}&isPlayerOne={isPlayerOne}&IsDone={IsDone}
        string uri = "https://localhost:44395/api/Battle?PlayerName=" + GameManager.instance.playerName + "&battleID=" + GameManager.instance.battleID + "&isPlayerOne=" + GameManager.instance.isPlayerOne + "&IsDone=" + (GameManager.instance.isDone? 1:0);

        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            yield return webRequest.SendWebRequest();
            if (webRequest.isNetworkError)
            {
                Debug.Log("There was an error");
            }
            else
            {
                Debug.Log("SENT");
            }
        }
    }

    IEnumerator GetAnswer()
    {  
        string uri = "https://localhost:44395/api/Answer?battleID="+ GameManager.instance.battleID + "&isPlayerOne=" + GameManager.instance.isPlayerOne + "&Time=" + (GameManager.instance.timeForRound - GameManager.instance.timeRemaining);

        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.isNetworkError)
            {
                Debug.Log("There was an error");
            }
            else
            {
                Debug.Log("AnswerGotten");
            }
        }
    }

    IEnumerator GetDelete()
    {
        yield return new WaitForSeconds(1f);
        string uri = "https://localhost:44395/api/Answer?battleID=" + GameManager.instance.battleID;

        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.isNetworkError)
            {
                Debug.Log("There was an error");
            }
            else
            {
                Debug.Log("AnswerGotten");
            }
        }

    }
}