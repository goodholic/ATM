using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ATMPopup : MonoBehaviour
{
    [Header("Popup UI")]
    [SerializeField] private GameObject popupPanel;
    [SerializeField] private Text popupTitleText;
    [SerializeField] private InputField amountInputField;
    [SerializeField] private Button confirmButton;
    [SerializeField] private Button cancelButton;
    [SerializeField] private Text errorMessageText;
    
    [Header("References")]
    [SerializeField] private ATMManager atmManager;
    
    private bool isDepositMode = true;
    
    private void Start()
    {
        confirmButton.onClick.AddListener(OnConfirmClick);
        cancelButton.onClick.AddListener(OnCancelClick);
        
        // 초기에는 팝업 숨기기
        HidePopup();
    }
    
    public void ShowDepositPopup()
    {
        isDepositMode = true;
        popupTitleText.text = "입금하실 금액을 입력해주세요";
        popupTitleText.fontStyle = FontStyle.Bold;
        amountInputField.text = "";
        errorMessageText.text = "";
        popupPanel.SetActive(true);
    }
    
    public void ShowWithdrawPopup()
    {
        isDepositMode = false;
        popupTitleText.text = "출금하실 금액을 입력해주세요";
        popupTitleText.fontStyle = FontStyle.Bold;
        amountInputField.text = "";
        errorMessageText.text = "";
        popupPanel.SetActive(true);
    }
    
    private void OnConfirmClick()
    {
        string inputText = amountInputField.text;
        
        if (string.IsNullOrEmpty(inputText))
        {
            ShowErrorMessage("금액을 입력해주세요.");
            return;
        }
        
        int amount;
        if (!int.TryParse(inputText, out amount))
        {
            ShowErrorMessage("올바른 숫자를 입력해주세요.");
            return;
        }
        
        if (amount <= 0)
        {
            ShowErrorMessage("0보다 큰 금액을 입력해주세요.");
            return;
        }
        
        if (isDepositMode)
        {
            // 입금 처리
            atmManager.Deposit(amount);
        }
        else
        {
            // 출금 처리
            atmManager.Withdraw(amount);
        }
        
        HidePopup();
    }
    
    private void OnCancelClick()
    {
        HidePopup();
    }
    
    private void ShowErrorMessage(string message)
    {
        errorMessageText.text = message;
        errorMessageText.color = Color.red;
        errorMessageText.fontStyle = FontStyle.Bold;
        
        // 3초 후 메시지 지우기
        StartCoroutine(ClearErrorMessageAfterDelay(3f));
    }
    
    private IEnumerator ClearErrorMessageAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        errorMessageText.text = "";
    }
    
    private void HidePopup()
    {
        popupPanel.SetActive(false);
        amountInputField.text = "";
        errorMessageText.text = "";
    }
}