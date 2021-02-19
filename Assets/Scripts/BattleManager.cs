using UnityEngine;
using TMPro;

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
