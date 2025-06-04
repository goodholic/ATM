using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PopupBank : MonoBehaviour
{
    [Header("Popup Panels")]
    [SerializeField] private GameObject depositPopupPanel;
    [SerializeField] private GameObject withdrawPopupPanel;
    
    [Header("Deposit Popup UI")]
    [SerializeField] private TextMeshProUGUI depositTitleText;
    [SerializeField] private TextMeshProUGUI depositCurrentCashText;
    [SerializeField] private TMP_InputField depositInputField;
    [SerializeField] private Button[] depositAmountButtons;
    [SerializeField] private Button depositConfirmButton;
    [SerializeField] private Button depositBackButton;
    
    [Header("Withdraw Popup UI")]
    [SerializeField] private TextMeshProUGUI withdrawTitleText;
    [SerializeField] private TextMeshProUGUI withdrawBalanceText;
    [SerializeField] private TMP_InputField withdrawInputField;
    [SerializeField] private Button[] withdrawAmountButtons;
    [SerializeField] private Button withdrawConfirmButton;
    [SerializeField] private Button withdrawBackButton;
    
    [Header("Quick Amount Settings")]
    private int[] quickAmounts = { 10000, 30000, 50000 };  // 1만원, 3만원, 5만원으로 수정
    
    [Header("References")]
    [SerializeField] private ATMManagerImproved atmManager;
    
    private void Start()
    {
        // 초기 설정
        SetupDepositButtons();
        SetupWithdrawButtons();
        SetupInputFields();
        
        // ATMManager 찾기
        if (atmManager == null)
        {
            atmManager = FindFirstObjectByType<ATMManagerImproved>();
        }
        
        // 초기에는 모든 팝업 숨기기
        HideAllPopups();
    }
    
    private void SetupDepositButtons()
    {
        // 입금 확인 버튼
        if (depositConfirmButton != null)
        {
            depositConfirmButton.onClick.RemoveAllListeners();
            depositConfirmButton.onClick.AddListener(OnDepositConfirm);
        }
        
        // 입금 뒤로가기 버튼
        if (depositBackButton != null)
        {
            depositBackButton.onClick.RemoveAllListeners();
            depositBackButton.onClick.AddListener(CloseDepositPopup);
        }
        
        // 빠른 금액 버튼들 설정 - 1만원, 3만원, 5만원
        for (int i = 0; i < depositAmountButtons.Length && i < quickAmounts.Length; i++)
        {
            int amount = quickAmounts[i];
            Button button = depositAmountButtons[i];
            
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => OnQuickAmountClick(depositInputField, amount));
            
            TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>();
            if (buttonText != null)
            {
                // 버튼 텍스트 설정
                switch(amount)
                {
                    case 10000:
                        buttonText.text = "1만원";
                        break;
                    case 30000:
                        buttonText.text = "3만원";
                        break;
                    case 50000:
                        buttonText.text = "5만원";
                        break;
                    default:
                        buttonText.text = string.Format("{0:N0}원", amount);
                        break;
                }
            }
        }
    }
    
    private void SetupWithdrawButtons()
    {
        // 출금 확인 버튼
        if (withdrawConfirmButton != null)
        {
            withdrawConfirmButton.onClick.RemoveAllListeners();
            withdrawConfirmButton.onClick.AddListener(OnWithdrawConfirm);
        }
        
        // 출금 뒤로가기 버튼
        if (withdrawBackButton != null)
        {
            withdrawBackButton.onClick.RemoveAllListeners();
            withdrawBackButton.onClick.AddListener(CloseWithdrawPopup);
        }
        
        // 빠른 금액 버튼들 설정 - 1만원, 3만원, 5만원
        for (int i = 0; i < withdrawAmountButtons.Length && i < quickAmounts.Length; i++)
        {
            int amount = quickAmounts[i];
            Button button = withdrawAmountButtons[i];
            
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => OnQuickAmountClick(withdrawInputField, amount));
            
            TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>();
            if (buttonText != null)
            {
                // 버튼 텍스트 설정
                switch(amount)
                {
                    case 10000:
                        buttonText.text = "1만원";
                        break;
                    case 30000:
                        buttonText.text = "3만원";
                        break;
                    case 50000:
                        buttonText.text = "5만원";
                        break;
                    default:
                        buttonText.text = string.Format("{0:N0}원", amount);
                        break;
                }
            }
        }
    }
    
    private void SetupInputFields()
    {
        // 입금 InputField 설정
        if (depositInputField != null)
        {
            depositInputField.contentType = TMP_InputField.ContentType.IntegerNumber;
            depositInputField.characterLimit = 9;
        }
        
        // 출금 InputField 설정
        if (withdrawInputField != null)
        {
            withdrawInputField.contentType = TMP_InputField.ContentType.IntegerNumber;
            withdrawInputField.characterLimit = 9;
        }
    }
    
    public void ShowDepositPopup()
    {
        HideAllPopups();
        
        if (depositPopupPanel != null)
        {
            depositPopupPanel.SetActive(true);
            
            // GameManager에서 UserData 가져오기
            UserData userData = GameManager.Instance.userData;
            
            // 현재 현금 표시
            if (depositCurrentCashText != null)
            {
                depositCurrentCashText.text = string.Format("현재 현금: {0:N0}원", userData.cash);
            }
            
            // 타이틀 설정
            if (depositTitleText != null)
            {
                depositTitleText.text = "입금하실 금액을 입력해주세요";
            }
            
            // InputField 초기화
            if (depositInputField != null)
            {
                depositInputField.text = "";
                depositInputField.Select();
            }
        }
    }
    
    public void ShowWithdrawPopup()
    {
        HideAllPopups();
        
        if (withdrawPopupPanel != null)
        {
            withdrawPopupPanel.SetActive(true);
            
            // GameManager에서 UserData 가져오기
            UserData userData = GameManager.Instance.userData;
            
            // 현재 잔액 표시
            if (withdrawBalanceText != null)
            {
                withdrawBalanceText.text = string.Format("현재 잔액: {0:N0}원", userData.balance);
            }
            
            // 타이틀 설정
            if (withdrawTitleText != null)
            {
                withdrawTitleText.text = "출금하실 금액을 입력해주세요";
            }
            
            // InputField 초기화
            if (withdrawInputField != null)
            {
                withdrawInputField.text = "";
                withdrawInputField.Select();
            }
        }
    }
    
    private void OnQuickAmountClick(TMP_InputField targetInput, int amount)
    {
        if (targetInput != null)
        {
            targetInput.text = amount.ToString();
        }
    }
    
    private void OnDepositConfirm()
    {
        string inputText = depositInputField.text;
        
        if (string.IsNullOrEmpty(inputText))
        {
            Debug.LogWarning("금액을 입력해주세요.");
            return;
        }
        
        int amount;
        if (!int.TryParse(inputText, out amount))
        {
            Debug.LogWarning("올바른 숫자를 입력해주세요.");
            return;
        }
        
        if (amount <= 0)
        {
            Debug.LogWarning("0보다 큰 금액을 입력해주세요.");
            return;
        }
        
        // 최대 금액 제한
        if (amount > 10000000)
        {
            Debug.LogWarning("1천만원 이하의 금액을 입력해주세요.");
            return;
        }
        
        // GameManager에서 UserData 가져오기
        UserData userData = GameManager.Instance.userData;
        
        // 현금 확인
        if (amount > userData.cash)
        {
            Debug.LogWarning(string.Format("현금이 부족합니다. 현재 현금: {0:N0}원", userData.cash));
            return;
        }
        
        // 입금 처리
        bool success = GameManager.Instance.DepositToBalance(amount);
        
        if (success)
        {
            CloseDepositPopup();
            
            // ATMManager UI 업데이트
            if (atmManager != null)
            {
                atmManager.UpdateDisplay();
            }
            
            Debug.Log(string.Format("{0:N0}원이 입금되었습니다.", amount));
        }
        else
        {
            Debug.LogWarning("입금 처리 중 오류가 발생했습니다.");
        }
    }
    
    private void OnWithdrawConfirm()
    {
        string inputText = withdrawInputField.text;
        
        if (string.IsNullOrEmpty(inputText))
        {
            Debug.LogWarning("금액을 입력해주세요.");
            return;
        }
        
        int amount;
        if (!int.TryParse(inputText, out amount))
        {
            Debug.LogWarning("올바른 숫자를 입력해주세요.");
            return;
        }
        
        if (amount <= 0)
        {
            Debug.LogWarning("0보다 큰 금액을 입력해주세요.");
            return;
        }
        
        // 최대 금액 제한
        if (amount > 10000000)
        {
            Debug.LogWarning("1천만원 이하의 금액을 입력해주세요.");
            return;
        }
        
        // GameManager에서 UserData 가져오기
        UserData userData = GameManager.Instance.userData;
        
        // 잔액 확인
        if (amount > userData.balance)
        {
            Debug.LogWarning(string.Format("잔액이 부족합니다. 현재 잔액: {0:N0}원", userData.balance));
            return;
        }
        
        // 출금 처리
        bool success = GameManager.Instance.WithdrawFromBalance(amount);
        
        if (success)
        {
            CloseWithdrawPopup();
            
            // ATMManager UI 업데이트
            if (atmManager != null)
            {
                atmManager.UpdateDisplay();
            }
            
            Debug.Log(string.Format("{0:N0}원이 출금되었습니다.", amount));
        }
        else
        {
            Debug.LogWarning("출금 처리 중 오류가 발생했습니다.");
        }
    }
    
    private void CloseDepositPopup()
    {
        if (depositPopupPanel != null)
        {
            depositPopupPanel.SetActive(false);
            depositInputField.text = "";
        }
    }
    
    private void CloseWithdrawPopup()
    {
        if (withdrawPopupPanel != null)
        {
            withdrawPopupPanel.SetActive(false);
            withdrawInputField.text = "";
        }
    }
    
    private void HideAllPopups()
    {
        if (depositPopupPanel != null) depositPopupPanel.SetActive(false);
        if (withdrawPopupPanel != null) withdrawPopupPanel.SetActive(false);
    }
    
    // Update에서 키보드 입력 처리
    private void Update()
    {
        // 입금 팝업에서 Enter/Escape 처리
        if (depositPopupPanel != null && depositPopupPanel.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                OnDepositConfirm();
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                CloseDepositPopup();
            }
        }
        
        // 출금 팝업에서 Enter/Escape 처리
        if (withdrawPopupPanel != null && withdrawPopupPanel.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                OnWithdrawConfirm();
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                CloseWithdrawPopup();
            }
        }
    }
}