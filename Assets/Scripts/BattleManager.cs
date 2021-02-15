using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;

public class BattleManager : MonoBehaviour
{

    private BattleManager battleManager;

    public TMP_InputField inputField;
    public string playerName;


    private void Awake()
    {
        battleManager = this;

    }

    public void StoreName()
    {
        playerName = inputField.text;
        Debug.Log(playerName);
    }

}
