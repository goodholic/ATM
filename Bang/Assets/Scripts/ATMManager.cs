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
    
    [Header("Popup Reference")]
    [SerializeField] private PopupBank popupBankScript;
    
    [Header("Bank Data")]
    private int currentCash = 100000;
    private int balance = 50000;
    private string userName = "팀예찬";
    
    private void Start()
    {
        InitializeATM();
        SetupButtons();
        
        // PopupBank 스크립트 찾기
        if (popupBankScript == null)
        {
            popupBankScript = FindFirstObjectByType<PopupBank>();
        }
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
        // 기존 리스너 제거
        depositButton.onClick.RemoveAllListeners();
        withdrawButton.onClick.RemoveAllListeners();
        
        // 새로운 리스너 추가
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
        // PopupBank를 사용한 입금 팝업 표시
        if (popupBankScript != null)
        {
            popupBankScript.ShowDepositPopup();
        }
        else if (popupBank != null)
        {
            // PopupBank 스크립트가 없으면 GameObject를 직접 활성화
            Transform depositPopup = popupBank.transform.Find("DepositPopup");
            if (depositPopup != null)
            {
                depositPopup.gameObject.SetActive(true);
            }
            Debug.Log("입금 팝업 표시");
        }
        else
        {
            Debug.LogWarning("PopupBank를 찾을 수 없습니다.");
        }
    }
    
    private void OnWithdrawClick()
    {
        // PopupBank를 사용한 출금 팝업 표시
        if (popupBankScript != null)
        {
            popupBankScript.ShowWithdrawPopup();
        }
        else if (popupBank != null)
        {
            // PopupBank 스크립트가 없으면 GameObject를 직접 활성화
            Transform withdrawPopup = popupBank.transform.Find("WithdrawPopup");
            if (withdrawPopup != null)
            {
                withdrawPopup.gameObject.SetActive(true);
            }
            Debug.Log("출금 팝업 표시");
        }
        else
        {
            Debug.LogWarning("PopupBank를 찾을 수 없습니다.");
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
        
        // 성공 메시지 표시 (옵션)
        StartCoroutine(ShowSuccessMessage(string.Format("{0:N0}원이 입금되었습니다.", amount)));
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
        
        // 성공 메시지 표시 (옵션)
        StartCoroutine(ShowSuccessMessage(string.Format("{0:N0}원이 출금되었습니다.", amount)));
    }
    
    private IEnumerator ShowSuccessMessage(string message)
    {
        // 메시지 표시 UI가 있다면 여기서 처리
        Debug.Log(message);
        yield return new WaitForSeconds(2f);
    }
    
    // 현재 현금 반환
    public int GetCurrentCash()
    {
        return currentCash;
    }
    
    // 현재 잔액 반환
    public int GetBalance()
    {
        return balance;
    }
}