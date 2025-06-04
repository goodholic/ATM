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
    public TextMeshProUGUI totalAssetsText; // 총 자산 표시 텍스트 (옵션)
    
    [Header("Input Fields")]
    public TMP_InputField depositAmountInput;    // 입금액 입력 필드
    public TMP_InputField withdrawAmountInput;   // 출금액 입력 필드
    
    [Header("Buttons")]
    public Button depositButton;    // 입금 버튼
    public Button withdrawButton;   // 출금 버튼
    public Button refreshButton;    // 새로고침 버튼
    public Button resetButton;      // 리셋 버튼 (테스트용)
    
    [Header("Message UI")]
    public TextMeshProUGUI messageText;        // 메시지 표시 텍스트 (옵션)
    public float messageDuration = 2f; // 메시지 표시 시간
    
    private Coroutine messageCoroutine;
    
    private void OnEnable()
    {
        // 데이터 변경 이벤트 구독
        GameManager.onDataChanged += Refresh;
    }
    
    private void OnDisable()
    {
        // 데이터 변경 이벤트 구독 해제
        GameManager.onDataChanged -= Refresh;
    }
    
    private void Start()
    {
        // 시작 시 UI 업데이트
        Refresh();
        
        // 버튼 이벤트 연결
        if (depositButton != null)
            depositButton.onClick.AddListener(OnDepositButtonClick);
            
        if (withdrawButton != null)
            withdrawButton.onClick.AddListener(OnWithdrawButtonClick);
            
        if (refreshButton != null)
            refreshButton.onClick.AddListener(Refresh);
            
        if (resetButton != null)
            resetButton.onClick.AddListener(OnResetButtonClick);
            
        // 입력 필드 초기화
        if (depositAmountInput != null)
            depositAmountInput.text = "";
            
        if (withdrawAmountInput != null)
            withdrawAmountInput.text = "";
    }
    
    // UI를 현재 UserData로 업데이트하는 메서드
    public void Refresh()
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
                
            if (totalAssetsText != null)
                totalAssetsText.text = $"총 자산: {GameManager.Instance.GetTotalAssets():N0}원";
                
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
                    ShowMessage("0원 이상의 금액을 입력해주세요.");
                    return;
                }
                
                if (GameManager.Instance.DepositToBalance(amount))
                {
                    ShowMessage($"{amount:N0}원이 입금되었습니다.");
                    depositAmountInput.text = "";
                }
                else
                {
                    ShowMessage("입금 실패: 현금이 부족합니다.");
                }
            }
            else
            {
                ShowMessage("올바른 금액을 입력해주세요.");
            }
        }
        else
        {
            ShowMessage("입금할 금액을 입력해주세요.");
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
                    ShowMessage("0원 이상의 금액을 입력해주세요.");
                    return;
                }
                
                if (GameManager.Instance.WithdrawFromBalance(amount))
                {
                    ShowMessage($"{amount:N0}원이 출금되었습니다.");
                    withdrawAmountInput.text = "";
                }
                else
                {
                    ShowMessage("출금 실패: 잔액이 부족합니다.");
                }
            }
            else
            {
                ShowMessage("올바른 금액을 입력해주세요.");
            }
        }
        else
        {
            ShowMessage("출금할 금액을 입력해주세요.");
        }
    }
    
    // 리셋 버튼 클릭 시 (테스트용)
    private void OnResetButtonClick()
    {
        GameManager.Instance.ResetUserData();
        ShowMessage("데이터가 초기값으로 리셋되었습니다.");
    }
    
    // 메시지 표시
    private void ShowMessage(string message)
    {
        if (messageText != null)
        {
            if (messageCoroutine != null)
                StopCoroutine(messageCoroutine);
                
            messageText.text = message;
            messageCoroutine = StartCoroutine(HideMessageAfterDelay());
        }
        
        Debug.Log($"ATM 메시지: {message}");
    }
    
    // 메시지 숨기기 코루틴
    private IEnumerator HideMessageAfterDelay()
    {
        yield return new WaitForSeconds(messageDuration);
        if (messageText != null)
            messageText.text = "";
    }
    
    private void OnDestroy()
    {
        // 버튼 이벤트 해제
        if (depositButton != null)
            depositButton.onClick.RemoveListener(OnDepositButtonClick);
            
        if (withdrawButton != null)
            withdrawButton.onClick.RemoveListener(OnWithdrawButtonClick);
            
        if (refreshButton != null)
            refreshButton.onClick.RemoveListener(Refresh);
            
        if (resetButton != null)
            resetButton.onClick.RemoveListener(OnResetButtonClick);
    }
}