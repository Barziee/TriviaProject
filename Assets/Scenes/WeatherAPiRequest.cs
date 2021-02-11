using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using UnityEngine;
using UnityEngine.Networking;

public class WeatherAPiRequest : MonoBehaviour
{


    void Start()
    {
        // A correct website page.
        StartCoroutine(GetRequest("http://api.openweathermap.org/data/2.5/weather?q=London&appid=4fc7c98d5928c9e74603616e08d7f704"));

       
    }
   
    IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

           // string[] pages = uri.Split('/');
           // int page = pages.Length - 1;

            if (webRequest.isNetworkError)
            {
                Debug.Log(System.Convert.ToString(webRequest.error));
            }
            else
            {
                string output = System.Convert.ToString(webRequest.downloadHandler.text);
                string[] parsedOut = output.Split(',' , '}', ':', '{') ;
                foreach (var w in parsedOut)
                {
                    string n = w.ToString();
                    if (w != "" && w != "{" && w != "" && w !="{" && w!= "[")
                    {
                        Debug.Log(n);
                    }
                
                }
            }
        }
    }
}

    


