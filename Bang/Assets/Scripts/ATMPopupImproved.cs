using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ATMPopupImproved : MonoBehaviour
{
    [Header("Popup UI")]
    [SerializeField] private GameObject popupPanel;
    [SerializeField] private Text popupTitleText;
    [SerializeField] private InputField amountInputField;
    [SerializeField] private Button confirmButton;
    [SerializeField] private Button cancelButton;
    [SerializeField] private Text errorMessageText;
    [SerializeField] private Text currentAmountText;
    
    [Header("Quick Amount Buttons")]
    [SerializeField] private Button[] quickAmountButtons;
    private int[] quickAmounts = { 10000, 30000, 50000, 100000 };
    
    [Header("References")]
    [SerializeField] private ATMManagerImproved atmManager;
    
    private bool isDepositMode = true;
    private BankDataManager bankDataManager;
    
    private void Start()
    {
        bankDataManager = BankDataManager.Instance;
        
        confirmButton.onClick.AddListener(OnConfirmClick);
        cancelButton.onClick.AddListener(OnCancelClick);
        
        // 빠른 금액 버튼 설정
        SetupQuickAmountButtons();
        
        // InputField 설정
        amountInputField.contentType = InputField.ContentType.IntegerNumber;
        amountInputField.characterLimit = 9;
        
        // 초기에는 팝업 숨기기
        HidePopup();
    }
    
    private void SetupQuickAmountButtons()
    {
        for (int i = 0; i < quickAmountButtons.Length && i < quickAmounts.Length; i++)
        {
            int amount = quickAmounts[i];
            Button button = quickAmountButtons[i];
            
            button.onClick.AddListener(() => OnQuickAmountClick(amount));
            
            Text buttonText = button.GetComponentInChildren<Text>();
            if (buttonText != null)
            {
                buttonText.text = string.Format("{0:N0}원", amount);
            }
        }
    }
    
    private void OnQuickAmountClick(int amount)
    {
        amountInputField.text = amount.ToString();
    }
    
    public void ShowDepositPopup()
    {
        isDepositMode = true;
        popupTitleText.text = "입금하실 금액을 입력해주세요";
        
        BankData bankData = bankDataManager.GetBankData();
        currentAmountText.text = string.Format("현재 보유 현금: {0:N0}원", bankData.currentCash);
        
        amountInputField.text = "";
        errorMessageText.text = "";
        popupPanel.SetActive(true);
        
        // 입력 필드에 포커스
        amountInputField.Select();
    }
    
    public void ShowWithdrawPopup()
    {
        isDepositMode = false;
        popupTitleText.text = "출금하실 금액을 입력해주세요";
        
        BankData bankData = bankDataManager.GetBankData();
        currentAmountText.text = string.Format("현재 잔액: {0:N0}원", bankData.balance);
        
        amountInputField.text = "";
        errorMessageText.text = "";
        popupPanel.SetActive(true);
        
        // 입력 필드에 포커스
        amountInputField.Select();
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
        
        // 최대 금액 제한
        if (amount > 10000000)
        {
            ShowErrorMessage("1천만원 이하의 금액을 입력해주세요.");
            return;
        }
        
        // 금액 확인
        BankData bankData = bankDataManager.GetBankData();
        
        if (isDepositMode)
        {
            if (amount > bankData.currentCash)
            {
                ShowErrorMessage(string.Format("보유 현금이 부족합니다.\n(현재: {0:N0}원)", bankData.currentCash));
                return;
            }
            
            // 입금 처리
            atmManager.ProcessDeposit(amount);
        }
        else
        {
            if (amount > bankData.balance)
            {
                ShowErrorMessage(string.Format("잔액이 부족합니다.\n(현재: {0:N0}원)", bankData.balance));
                return;
            }
            
            // 출금 처리
            atmManager.ProcessWithdraw(amount);
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
        
        // 3초 후 메시지 지우기
        StopAllCoroutines();
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
    
    // Update에서 Enter 키 처리
    private void Update()
    {
        if (popupPanel.activeSelf && Input.GetKeyDown(KeyCode.Return))
        {
            OnConfirmClick();
        }
        else if (popupPanel.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            OnCancelClick();
        }
    }
}