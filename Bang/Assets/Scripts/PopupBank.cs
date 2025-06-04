using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupBank : MonoBehaviour
{
    [Header("Popup Settings")]
    [SerializeField] private GameObject depositPopupPanel;
    [SerializeField] private GameObject withdrawPopupPanel;
    
    [Header("Deposit Popup UI")]
    [SerializeField] private Text depositTitleText;
    [SerializeField] private InputField depositAmountInput;
    [SerializeField] private Button depositConfirmButton;
    [SerializeField] private Button depositCancelButton;
    [SerializeField] private Text depositErrorText;
    
    [Header("Withdraw Popup UI")]
    [SerializeField] private Text withdrawTitleText;
    [SerializeField] private InputField withdrawAmountInput;
    [SerializeField] private Button withdrawConfirmButton;
    [SerializeField] private Button withdrawCancelButton;
    [SerializeField] private Text withdrawErrorText;
    
    [Header("Quick Amount Buttons")]
    [SerializeField] private Button[] depositQuickButtons;
    [SerializeField] private Button[] withdrawQuickButtons;
    private int[] quickAmounts = { 10000, 30000, 50000, 100000 };
    
    private ATMManager atmManager;
    private ATMManagerImproved atmManagerImproved;
    
    private void Awake()
    {
        // ATMManager 찾기
        atmManager = FindFirstObjectByType<ATMManager>();
        atmManagerImproved = FindFirstObjectByType<ATMManagerImproved>();
        
        // 초기에는 모든 팝업 숨기기
        HideAllPopups();
    }
    
    private void Start()
    {
        // 버튼 이벤트 설정
        SetupButtonEvents();
        SetupQuickAmountButtons();
        
        // InputField 설정
        if (depositAmountInput != null)
        {
            depositAmountInput.contentType = InputField.ContentType.IntegerNumber;
            depositAmountInput.characterLimit = 9;
        }
        
        if (withdrawAmountInput != null)
        {
            withdrawAmountInput.contentType = InputField.ContentType.IntegerNumber;
            withdrawAmountInput.characterLimit = 9;
        }
    }
    
    private void SetupButtonEvents()
    {
        // 입금 팝업 버튼들
        if (depositConfirmButton != null)
            depositConfirmButton.onClick.AddListener(OnDepositConfirm);
            
        if (depositCancelButton != null)
            depositCancelButton.onClick.AddListener(OnDepositCancel);
            
        // 출금 팝업 버튼들
        if (withdrawConfirmButton != null)
            withdrawConfirmButton.onClick.AddListener(OnWithdrawConfirm);
            
        if (withdrawCancelButton != null)
            withdrawCancelButton.onClick.AddListener(OnWithdrawCancel);
    }
    
    private void SetupQuickAmountButtons()
    {
        // 입금 빠른 금액 버튼
        for (int i = 0; i < depositQuickButtons.Length && i < quickAmounts.Length; i++)
        {
            int amount = quickAmounts[i];
            Button button = depositQuickButtons[i];
            
            if (button != null)
            {
                button.onClick.AddListener(() => SetDepositAmount(amount));
                
                Text buttonText = button.GetComponentInChildren<Text>();
                if (buttonText != null)
                {
                    buttonText.text = string.Format("{0:N0}원", amount);
                    buttonText.fontStyle = FontStyle.Bold;
                }
            }
        }
        
        // 출금 빠른 금액 버튼
        for (int i = 0; i < withdrawQuickButtons.Length && i < quickAmounts.Length; i++)
        {
            int amount = quickAmounts[i];
            Button button = withdrawQuickButtons[i];
            
            if (button != null)
            {
                button.onClick.AddListener(() => SetWithdrawAmount(amount));
                
                Text buttonText = button.GetComponentInChildren<Text>();
                if (buttonText != null)
                {
                    buttonText.text = string.Format("{0:N0}원", amount);
                    buttonText.fontStyle = FontStyle.Bold;
                }
            }
        }
    }
    
    // 입금 팝업 표시
    public void ShowDepositPopup()
    {
        HideAllPopups();
        
        if (depositPopupPanel != null)
        {
            depositPopupPanel.SetActive(true);
            
            // UI 초기화
            if (depositTitleText != null)
            {
                depositTitleText.text = "입금하실 금액을 입력해주세요";
                depositTitleText.fontStyle = FontStyle.Bold;
            }
            
            if (depositAmountInput != null)
            {
                depositAmountInput.text = "";
                depositAmountInput.Select();
            }
            
            if (depositErrorText != null)
            {
                depositErrorText.text = "";
            }
        }
    }
    
    // 출금 팝업 표시
    public void ShowWithdrawPopup()
    {
        HideAllPopups();
        
        if (withdrawPopupPanel != null)
        {
            withdrawPopupPanel.SetActive(true);
            
            // UI 초기화
            if (withdrawTitleText != null)
            {
                withdrawTitleText.text = "출금하실 금액을 입력해주세요";
                withdrawTitleText.fontStyle = FontStyle.Bold;
            }
            
            if (withdrawAmountInput != null)
            {
                withdrawAmountInput.text = "";
                withdrawAmountInput.Select();
            }
            
            if (withdrawErrorText != null)
            {
                withdrawErrorText.text = "";
            }
        }
    }
    
    // 모든 팝업 숨기기
    public void HideAllPopups()
    {
        if (depositPopupPanel != null)
            depositPopupPanel.SetActive(false);
            
        if (withdrawPopupPanel != null)
            withdrawPopupPanel.SetActive(false);
    }
    
    // 입금 금액 설정
    private void SetDepositAmount(int amount)
    {
        if (depositAmountInput != null)
            depositAmountInput.text = amount.ToString();
    }
    
    // 출금 금액 설정
    private void SetWithdrawAmount(int amount)
    {
        if (withdrawAmountInput != null)
            withdrawAmountInput.text = amount.ToString();
    }
    
    // 입금 확인
    private void OnDepositConfirm()
    {
        if (depositAmountInput == null) return;
        
        string inputText = depositAmountInput.text;
        
        if (string.IsNullOrEmpty(inputText))
        {
            ShowDepositError("금액을 입력해주세요.");
            return;
        }
        
        int amount;
        if (!int.TryParse(inputText, out amount))
        {
            ShowDepositError("올바른 숫자를 입력해주세요.");
            return;
        }
        
        if (amount <= 0)
        {
            ShowDepositError("0보다 큰 금액을 입력해주세요.");
            return;
        }
        
        if (amount > 10000000)
        {
            ShowDepositError("1천만원 이하의 금액을 입력해주세요.");
            return;
        }
        
        // ATMManager에 입금 처리 요청
        bool success = false;
        
        if (atmManagerImproved != null)
        {
            atmManagerImproved.ProcessDeposit(amount);
            success = true;
        }
        else if (atmManager != null)
        {
            atmManager.Deposit(amount);
            success = true;
        }
        
        if (success)
        {
            HideAllPopups();
        }
        else
        {
            ShowDepositError("입금 처리에 실패했습니다.");
        }
    }
    
    // 입금 취소
    private void OnDepositCancel()
    {
        HideAllPopups();
    }
    
    // 출금 확인
    private void OnWithdrawConfirm()
    {
        if (withdrawAmountInput == null) return;
        
        string inputText = withdrawAmountInput.text;
        
        if (string.IsNullOrEmpty(inputText))
        {
            ShowWithdrawError("금액을 입력해주세요.");
            return;
        }
        
        int amount;
        if (!int.TryParse(inputText, out amount))
        {
            ShowWithdrawError("올바른 숫자를 입력해주세요.");
            return;
        }
        
        if (amount <= 0)
        {
            ShowWithdrawError("0보다 큰 금액을 입력해주세요.");
            return;
        }
        
        if (amount > 10000000)
        {
            ShowWithdrawError("1천만원 이하의 금액을 입력해주세요.");
            return;
        }
        
        // ATMManager에 출금 처리 요청
        bool success = false;
        
        if (atmManagerImproved != null)
        {
            atmManagerImproved.ProcessWithdraw(amount);
            success = true;
        }
        else if (atmManager != null)
        {
            atmManager.Withdraw(amount);
            success = true;
        }
        
        if (success)
        {
            HideAllPopups();
        }
        else
        {
            ShowWithdrawError("출금 처리에 실패했습니다.");
        }
    }
    
    // 출금 취소
    private void OnWithdrawCancel()
    {
        HideAllPopups();
    }
    
    // 입금 에러 메시지 표시
    private void ShowDepositError(string message)
    {
        if (depositErrorText != null)
        {
            depositErrorText.text = message;
            depositErrorText.color = Color.red;
            depositErrorText.fontStyle = FontStyle.Bold;
            
            StopAllCoroutines();
            StartCoroutine(ClearErrorMessageAfterDelay(depositErrorText, 3f));
        }
    }
    
    // 출금 에러 메시지 표시
    private void ShowWithdrawError(string message)
    {
        if (withdrawErrorText != null)
        {
            withdrawErrorText.text = message;
            withdrawErrorText.color = Color.red;
            withdrawErrorText.fontStyle = FontStyle.Bold;
            
            StopAllCoroutines();
            StartCoroutine(ClearErrorMessageAfterDelay(withdrawErrorText, 3f));
        }
    }
    
    // 에러 메시지 제거 코루틴
    private IEnumerator ClearErrorMessageAfterDelay(Text errorText, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (errorText != null)
            errorText.text = "";
    }
    
    // Update에서 키보드 입력 처리
    private void Update()
    {
        // 입금 팝업이 활성화되어 있을 때
        if (depositPopupPanel != null && depositPopupPanel.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                OnDepositConfirm();
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                OnDepositCancel();
            }
        }
        
        // 출금 팝업이 활성화되어 있을 때
        if (withdrawPopupPanel != null && withdrawPopupPanel.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                OnWithdrawConfirm();
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                OnWithdrawCancel();
            }
        }
    }
    
    private void OnDestroy()
    {
        // 버튼 이벤트 해제
        if (depositConfirmButton != null)
            depositConfirmButton.onClick.RemoveListener(OnDepositConfirm);
            
        if (depositCancelButton != null)
            depositCancelButton.onClick.RemoveListener(OnDepositCancel);
            
        if (withdrawConfirmButton != null)
            withdrawConfirmButton.onClick.RemoveListener(OnWithdrawConfirm);
            
        if (withdrawCancelButton != null)
            withdrawCancelButton.onClick.RemoveListener(OnWithdrawCancel);
    }
}