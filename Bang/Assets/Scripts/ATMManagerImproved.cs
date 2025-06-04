using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ATMManagerImproved : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject popupBank;
    [SerializeField] private Text titleText;
    [SerializeField] private Text atmText;
    [SerializeField] private Text userInfoText;
    [SerializeField] private Text currentCashText;
    [SerializeField] private Text balanceText;
    [SerializeField] private Button depositButton;
    [SerializeField] private Button withdrawButton;
    
    [Header("Popup References")]
    [SerializeField] private ATMPopup atmPopup;
    
    private BankDataManager bankDataManager;
    
    private void Start()
    {
        bankDataManager = BankDataManager.Instance;
        InitializeATM();
        SetupButtons();
    }
    
    private void InitializeATM()
    {
        // Title 설정 - Bold체로 작성
        titleText.text = "Sparta Bank";
        titleText.fontStyle = FontStyle.Bold;
        
        // ATM 텍스트 설정
        atmText.text = "ATM";
        
        // UserInfo 설정
        BankData bankData = bankDataManager.GetBankData();
        userInfoText.text = bankData.userName;
        
        // 현금 및 잔액 정보 업데이트
        UpdateDisplay();
    }
    
    private void SetupButtons()
    {
        depositButton.onClick.RemoveAllListeners();
        withdrawButton.onClick.RemoveAllListeners();
        
        depositButton.onClick.AddListener(OnDepositClick);
        withdrawButton.onClick.AddListener(OnWithdrawClick);
        
        // 버튼 텍스트 설정
        Text depositText = depositButton.GetComponentInChildren<Text>();
        Text withdrawText = withdrawButton.GetComponentInChildren<Text>();
        
        if (depositText != null)
        {
            depositText.text = "입금";
            depositText.fontStyle = FontStyle.Bold;
        }
        
        if (withdrawText != null)
        {
            withdrawText.text = "출금";
            withdrawText.fontStyle = FontStyle.Bold;
        }
    }
    
    public void UpdateDisplay()
    {
        BankData bankData = bankDataManager.GetBankData();
        
        // 현금 정보 업데이트 - string.Format 활용
        currentCashText.text = string.Format("현금\n{0:N0}", bankData.currentCash);
        currentCashText.fontStyle = FontStyle.Bold;
        
        // 잔액 정보 업데이트 - string.Format 활용
        balanceText.text = string.Format("Balance    {0:N0}", bankData.balance);
        balanceText.fontStyle = FontStyle.Bold;
    }
    
    private void OnDepositClick()
    {
        if (atmPopup != null)
        {
            atmPopup.ShowDepositPopup();
        }
        else
        {
            Debug.LogError("ATMPopup reference is missing!");
        }
    }
    
    private void OnWithdrawClick()
    {
        if (atmPopup != null)
        {
            atmPopup.ShowWithdrawPopup();
        }
        else
        {
            Debug.LogError("ATMPopup reference is missing!");
        }
    }
    
    public void ProcessDeposit(int amount)
    {
        bool success = bankDataManager.Deposit(amount);
        
        if (success)
        {
            UpdateDisplay();
            ShowMessage(string.Format("{0:N0}원이 입금되었습니다.", amount), Color.green);
        }
        else
        {
            ShowMessage("입금에 실패했습니다. 현금이 부족합니다.", Color.red);
        }
    }
    
    public void ProcessWithdraw(int amount)
    {
        bool success = bankDataManager.Withdraw(amount);
        
        if (success)
        {
            UpdateDisplay();
            ShowMessage(string.Format("{0:N0}원이 출금되었습니다.", amount), Color.green);
        }
        else
        {
            ShowMessage("출금에 실패했습니다. 잔액이 부족합니다.", Color.red);
        }
    }
    
    private void ShowMessage(string message, Color color)
    {
        Debug.Log(message);
        // 메시지 표시 UI가 있다면 여기서 처리
        StartCoroutine(ShowTemporaryMessage(message, color));
    }
    
    private IEnumerator ShowTemporaryMessage(string message, Color color)
    {
        // 임시 메시지 표시 로직
        yield return new WaitForSeconds(2f);
    }
    
    // 거래 내역 조회
    public void ShowTransactionHistory()
    {
        List<TransactionRecord> history = bankDataManager.GetTransactionHistory();
        
        foreach (TransactionRecord record in history)
        {
            string type = record.type == TransactionRecord.TransactionType.Deposit ? "입금" : "출금";
            Debug.Log(string.Format("[{0}] {1}: {2:N0}원, 잔액: {3:N0}원", 
                record.dateTime, type, record.amount, record.balanceAfter));
        }
    }
}