using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ATMUIStyleManager : MonoBehaviour
{
    [Header("Colors")]
    [SerializeField] private Color backgroundColor = new Color(0.8f, 0.5f, 0.4f, 1f); // 연한 갈색
    [SerializeField] private Color buttonColor = new Color(0.7f, 0.3f, 0.5f, 1f); // 보라색
    [SerializeField] private Color cashBoxColor = new Color(0.4f, 0.4f, 0.4f, 1f); // 회색
    [SerializeField] private Color textColor = Color.white;
    
    [Header("UI References")]
    [SerializeField] private Image backgroundPanel;
    [SerializeField] private Image cashBoxBackground;
    [SerializeField] private Button[] actionButtons;
    [SerializeField] private Text[] allTexts;
    
    [Header("Font Settings")]
    [SerializeField] private Font defaultFont;
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
        // 배경 색상 적용
        if (backgroundPanel != null)
        {
            backgroundPanel.color = backgroundColor;
        }
        
        // 현금 박스 색상 적용
        if (cashBoxBackground != null)
        {
            cashBoxBackground.color = cashBoxColor;
        }
        
        // 버튼 스타일 적용
        foreach (Button button in actionButtons)
        {
            if (button != null)
            {
                Image buttonImage = button.GetComponent<Image>();
                if (buttonImage != null)
                {
                    buttonImage.color = buttonColor;
                }
                
                Text buttonText = button.GetComponentInChildren<Text>();
                if (buttonText != null)
                {
                    buttonText.color = textColor;
                    buttonText.fontSize = buttonFontSize;
                    buttonText.fontStyle = FontStyle.Bold;
                    buttonText.alignment = TextAnchor.MiddleCenter;
                }
            }
        }
        
        // 텍스트 스타일 적용
        foreach (Text text in allTexts)
        {
            if (text != null)
            {
                text.color = textColor;
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
            Text titleText = titleObj.GetComponent<Text>();
            if (titleText != null)
            {
                titleText.fontSize = titleFontSize;
                titleText.fontStyle = FontStyle.Bold;
                titleText.alignment = TextAnchor.UpperCenter;
            }
        }
        
        // ATM 텍스트
        GameObject atmObj = GameObject.Find("ATMText");
        if (atmObj != null)
        {
            Text atmText = atmObj.GetComponent<Text>();
            if (atmText != null)
            {
                atmText.fontSize = atmFontSize;
                atmText.alignment = TextAnchor.UpperCenter;
            }
        }
        
        // Balance 텍스트
        GameObject balanceObj = GameObject.Find("BalanceText");
        if (balanceObj != null)
        {
            Text balanceText = balanceObj.GetComponent<Text>();
            if (balanceText != null)
            {
                balanceText.alignment = TextAnchor.UpperRight;
                balanceText.fontSize = defaultFontSize;
            }
        }
    }
    
    public void SetButtonHoverEffect(Button button, bool isHovered)
    {
        if (button != null)
        {
            Image buttonImage = button.GetComponent<Image>();
            if (buttonImage != null)
            {
                if (isHovered)
                {
                    buttonImage.color = new Color(buttonColor.r * 0.8f, buttonColor.g * 0.8f, buttonColor.b * 0.8f, 1f);
                }
                else
                {
                    buttonImage.color = buttonColor;
                }
            }
        }
    }
}