using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ATMManager : MonoBehaviour
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
    
    [Header("Bank Data")]
    private int currentCash = 100000;
    private int balance = 50000;
    private string userName = "팀예찬";
    
    private void Start()
    {
        InitializeATM();
        SetupButtons();
    }
    
    private void InitializeATM()
    {
        // Title 설정
        titleText.text = "Sparta Bank";
        titleText.fontStyle = FontStyle.Bold;
        
        // ATM 텍스트 설정
        atmText.text = "ATM";
        
        // UserInfo 설정
        UpdateUserInfo();
        
        // 현금 정보 업데이트
        UpdateCashDisplay();
        
        // 잔액 정보 업데이트
        UpdateBalanceDisplay();
    }
    
    private void SetupButtons()
    {
        depositButton.onClick.AddListener(OnDepositClick);
        withdrawButton.onClick.AddListener(OnWithdrawClick);
        
        // 버튼 텍스트 설정
        depositButton.GetComponentInChildren<Text>().text = "입금";
        withdrawButton.GetComponentInChildren<Text>().text = "출금";
    }
    
    private void UpdateUserInfo()
    {
        userInfoText.text = userName;
    }
    
    private void UpdateCashDisplay()
    {
        currentCashText.text = string.Format("현금\n{0:N0}", currentCash);
    }
    
    private void UpdateBalanceDisplay()
    {
        balanceText.text = string.Format("Balance    {0:N0}", balance);
    }
    
    private void OnDepositClick()
    {
        // 입금 팝업 표시
        ShowDepositPopup();
    }
    
    private void OnWithdrawClick()
    {
        // 출금 팝업 표시
        ShowWithdrawPopup();
    }
    
    private void ShowDepositPopup()
    {
        // 입금 팝업 로직
        ATMPopup popup = FindFirstObjectByType<ATMPopup>();
        if (popup != null)
        {
            popup.ShowDepositPopup();
        }
        else
        {
            Debug.Log("입금 팝업 표시");
        }
    }
    
    private void ShowWithdrawPopup()
    {
        // 출금 팝업 로직
        ATMPopup popup = FindFirstObjectByType<ATMPopup>();
        if (popup != null)
        {
            popup.ShowWithdrawPopup();
        }
        else
        {
            Debug.Log("출금 팝업 표시");
        }
    }
    
    public void Deposit(int amount)
    {
        if (amount <= 0)
        {
            Debug.Log("올바른 금액을 입력해주세요.");
            return;
        }
        
        if (currentCash < amount)
        {
            Debug.Log("현금이 부족합니다.");
            return;
        }
        
        currentCash -= amount;
        balance += amount;
        
        UpdateCashDisplay();
        UpdateBalanceDisplay();
        
        Debug.Log(string.Format("{0}원이 입금되었습니다.", amount));
    }
    
    public void Withdraw(int amount)
    {
        if (amount <= 0)
        {
            Debug.Log("올바른 금액을 입력해주세요.");
            return;
        }
        
        if (balance < amount)
        {
            Debug.Log("잔액이 부족합니다.");
            return;
        }
        
        balance -= amount;
        currentCash += amount;
        
        UpdateCashDisplay();
        UpdateBalanceDisplay();
        
        Debug.Log(string.Format("{0}원이 출금되었습니다.", amount));
    }
}