using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class WebRequest : MonoBehaviour
{

    public List<string> CurrentData;

    [Header("Database Text")]
    public TextMeshProUGUI Question_text;
    public TextMeshProUGUI Answer1;
    public TextMeshProUGUI Answer2;
    public TextMeshProUGUI Answer3;
    public TextMeshProUGUI Answer4;

    public string Correct_Answer;

    public int _questionID;

    string QUri = "https://localhost:44356/api/Question/";
    string BUri = "https://localhost:44356/api/Question/";
    IEnumerator GetRequest(string uri, int ID)
    {
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
                       // Debug.Log(t);
                        CurrentData.Add(t);
                    }
                }

                ApplyListToString();
            }
        }
    }

    public void get_question(int QuestionID)
    {
        StartCoroutine(GetRequest(QUri, QuestionID));
        _questionID++;
    } 
    public void get_Battle(int battleid)
    {
        StartCoroutine(GetRequest(BUri, battleid));
        
    }  
    public void EnterName(int battleid)
    {
        StartCoroutine(GetRequest(BUri, battleid));
        
    }

    public void ApplyListToString()
    {
        Question_text.text = CurrentData[0];
        Answer1.text = CurrentData[1];
        Answer2.text = CurrentData[2];
        Answer3.text = CurrentData[3];
        Answer4.text = CurrentData[4];
        Correct_Answer = CurrentData[5];
    }

}