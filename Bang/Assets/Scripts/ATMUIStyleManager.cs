using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ATMUIStyleManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Image backgroundPanel;
    [SerializeField] private Image cashBoxBackground;
    [SerializeField] private Button[] actionButtons;
    [SerializeField] private TextMeshProUGUI[] allTexts;
    
    [Header("Font Settings")]
    [SerializeField] private TMP_FontAsset defaultFont;
    [SerializeField] private int titleFontSize = 48;
    [SerializeField] private int atmFontSize = 36;
    [SerializeField] private int defaultFontSize = 24;
    [SerializeField] private int buttonFontSize = 28;
    
    private void Start()
    {
        ApplyUIStyles();
    }
    
    private void ApplyUIStyles()
    {
        // 버튼 스타일 적용
        foreach (Button button in actionButtons)
        {
            if (button != null)
            {
                TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>();
                if (buttonText != null)
                {
                    buttonText.fontSize = buttonFontSize;
                    buttonText.alignment = TextAlignmentOptions.Center;
                }
            }
        }
        
        // 텍스트 스타일 적용
        foreach (TextMeshProUGUI text in allTexts)
        {
            if (text != null)
            {
                if (defaultFont != null)
                {
                    text.font = defaultFont;
                }
            }
        }
        
        // 특별한 텍스트 스타일 적용
        ApplySpecialTextStyles();
    }
    
    private void ApplySpecialTextStyles()
    {
        // Sparta Bank 타이틀
        GameObject titleObj = GameObject.Find("TitleText");
        if (titleObj != null)
        {
            TextMeshProUGUI titleText = titleObj.GetComponent<TextMeshProUGUI>();
            if (titleText != null)
            {
                titleText.fontSize = titleFontSize;
                titleText.alignment = TextAlignmentOptions.Top;
            }
        }
        
        // ATM 텍스트
        GameObject atmObj = GameObject.Find("ATMText");
        if (atmObj != null)
        {
            TextMeshProUGUI atmText = atmObj.GetComponent<TextMeshProUGUI>();
            if (atmText != null)
            {
                atmText.fontSize = atmFontSize;
                atmText.alignment = TextAlignmentOptions.Top;
            }
        }
        
        // Balance 텍스트
        GameObject balanceObj = GameObject.Find("BalanceText");
        if (balanceObj != null)
        {
            TextMeshProUGUI balanceText = balanceObj.GetComponent<TextMeshProUGUI>();
            if (balanceText != null)
            {
                balanceText.alignment = TextAlignmentOptions.TopRight;
                balanceText.fontSize = defaultFontSize;
            }
        }
    }
}