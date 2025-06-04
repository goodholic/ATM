using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ATMUIController : MonoBehaviour
{
    [Header("User Info UI")]
    public TextMeshProUGUI nameText;       // 이름 표시 텍스트
    public TextMeshProUGUI cashText;       // 현금 표시 텍스트
    public TextMeshProUGUI balanceText;    // 통장 잔액 표시 텍스트
    
    [Header("Input Fields")]
    public TMP_InputField depositAmountInput;    // 입금액 입력 필드
    public TMP_InputField withdrawAmountInput;   // 출금액 입력 필드
    
    [Header("Buttons")]
    public Button depositButton;    // 입금 버튼
    public Button withdrawButton;   // 출금 버튼
    
    private void Start()
    {
        // 시작 시 UI 업데이트
        UpdateUI();
        
        // 버튼 이벤트 연결
        if (depositButton != null)
            depositButton.onClick.AddListener(OnDepositButtonClick);
            
        if (withdrawButton != null)
            withdrawButton.onClick.AddListener(OnWithdrawButtonClick);
            
        // 입력 필드 초기화
        if (depositAmountInput != null)
            depositAmountInput.text = "";
            
        if (withdrawAmountInput != null)
            withdrawAmountInput.text = "";
    }
    
    // UI를 현재 UserData로 업데이트하는 메서드
    public void UpdateUI()
    {
        if (GameManager.Instance != null && GameManager.Instance.userData != null)
        {
            UserData userData = GameManager.Instance.userData;
            
            if (nameText != null)
                nameText.text = userData.name;
                
            if (cashText != null)
                cashText.text = $"{userData.cash:N0}원";
                
            if (balanceText != null)
                balanceText.text = $"{userData.balance:N0}원";
                
            Debug.Log("UI가 업데이트되었습니다.");
        }
        else
        {
            Debug.LogError("GameManager 또는 UserData를 찾을 수 없습니다.");
        }
    }
    
    // 입금 버튼 클릭 시
    private void OnDepositButtonClick()
    {
        if (depositAmountInput != null && !string.IsNullOrEmpty(depositAmountInput.text))
        {
            if (int.TryParse(depositAmountInput.text, out int amount))
            {
                if (amount <= 0)
                {
                    Debug.LogWarning("0원 이상의 금액을 입력해주세요.");
                    return;
                }
                
                if (GameManager.Instance.DepositToBalance(amount))
                {
                    Debug.Log($"{amount:N0}원이 입금되었습니다.");
                    depositAmountInput.text = "";
                    UpdateUI();
                }
                else
                {
                    Debug.LogWarning("입금 실패: 현금이 부족합니다.");
                }
            }
            else
            {
                Debug.LogWarning("올바른 금액을 입력해주세요.");
            }
        }
        else
        {
            Debug.LogWarning("입금할 금액을 입력해주세요.");
        }
    }
    
    // 출금 버튼 클릭 시
    private void OnWithdrawButtonClick()
    {
        if (withdrawAmountInput != null && !string.IsNullOrEmpty(withdrawAmountInput.text))
        {
            if (int.TryParse(withdrawAmountInput.text, out int amount))
            {
                if (amount <= 0)
                {
                    Debug.LogWarning("0원 이상의 금액을 입력해주세요.");
                    return;
                }
                
                if (GameManager.Instance.WithdrawFromBalance(amount))
                {
                    Debug.Log($"{amount:N0}원이 출금되었습니다.");
                    withdrawAmountInput.text = "";
                    UpdateUI();
                }
                else
                {
                    Debug.LogWarning("출금 실패: 잔액이 부족합니다.");
                }
            }
            else
            {
                Debug.LogWarning("올바른 금액을 입력해주세요.");
            }
        }
        else
        {
            Debug.LogWarning("출금할 금액을 입력해주세요.");
        }
    }
    
    private void OnDestroy()
    {
        // 버튼 이벤트 해제
        if (depositButton != null)
            depositButton.onClick.RemoveListener(OnDepositButtonClick);
            
        if (withdrawButton != null)
            withdrawButton.onClick.RemoveListener(OnWithdrawButtonClick);
    }
}