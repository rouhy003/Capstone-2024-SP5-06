using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public GameObject gameUI;
    public GameObject gameMenu;
    public TextMeshProUGUI timeUI;
    public TextMeshProUGUI text;

    public void UpdateText(string newText)
    {
        text.SetText(newText);
    }

    public void UpdateTime(string time)
    {
        timeUI.SetText(time);
    }

    public void SetGameUI(bool isActive)
    {
        gameUI.SetActive(isActive);
    }
    
    public void SetGameMenu(bool isActive)
    {
        gameMenu.SetActive(isActive);
    }
}
