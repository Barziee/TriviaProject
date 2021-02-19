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


    private void Awake()
    {
        battleManager = this;

    }

    public void StoreName()
    {
        GameManager.instance.playerName = inputField.text;
        GameManager.instance.StartCoroutine(GameManager.instance.SetSettings());
    }

}
