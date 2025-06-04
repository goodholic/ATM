using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ATMManagerImproved : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject popupBank;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI atmText;
    [SerializeField] private TextMeshProUGUI userInfoText;
    [SerializeField] private TextMeshProUGUI currentCashText;
    [SerializeField] private TextMeshProUGUI balanceText;
    [SerializeField] private Button depositButton;
    [SerializeField] private Button withdrawButton;
    
    [Header("Popup References")]
    [SerializeField] private PopupBank popupBankScript;
    
    private void OnEnable()
    {
        // 데이터 변경 이벤트 구독
        GameManager.onDataChanged += UpdateDisplay;
    }
    
    private void OnDisable()
    {
        // 데이터 변경 이벤트 구독 해제
        GameManager.onDataChanged -= UpdateDisplay;
    }
    
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
        
        // ATM 텍스트 설정
        atmText.text = "ATM";
        
        // GameManager에서 UserData 가져와서 표시
        if (GameManager.Instance != null && GameManager.Instance.userData != null)
        {
            userInfoText.text = GameManager.Instance.userData.name;
        }
        
        // 현금 및 잔액 정보 업데이트
        UpdateDisplay();
    }
    
    private void SetupButtons()
    {
        depositButton.onClick.RemoveAllListeners();
        withdrawButton.onClick.RemoveAllListeners();
        
        depositButton.onClick.AddListener(OnDepositClick);
        withdrawButton.onClick.AddListener(OnWithdrawClick);
        
        // 버튼 텍스트 설정 - TMPro 컴포넌트 확인
        TextMeshProUGUI depositText = depositButton.GetComponentInChildren<TextMeshProUGUI>();
        TextMeshProUGUI withdrawText = withdrawButton.GetComponentInChildren<TextMeshProUGUI>();
        
        // TMPro가 없으면 기본 Text 컴포넌트 확인
        if (depositText == null)
        {
            Text depositTextLegacy = depositButton.GetComponentInChildren<Text>();
            if (depositTextLegacy != null)
            {
                depositTextLegacy.text = "입금";
            }
        }
        else
        {
            depositText.text = "입금";
        }
        
        if (withdrawText == null)
        {
            Text withdrawTextLegacy = withdrawButton.GetComponentInChildren<Text>();
            if (withdrawTextLegacy != null)
            {
                withdrawTextLegacy.text = "출금";
            }
        }
        else
        {
            withdrawText.text = "출금";
        }
    }
    
    public void UpdateDisplay()
    {
        if (GameManager.Instance != null && GameManager.Instance.userData != null)
        {
            UserData userData = GameManager.Instance.userData;
            
            // 현금 정보 업데이트 - string.Format 활용
            currentCashText.text = string.Format("현금\n{0:N0}", userData.cash);
            
            // 잔액 정보 업데이트 - string.Format 활용
            balanceText.text = string.Format("Balance    {0:N0}", userData.balance);
            
            // 유저 이름 업데이트
            userInfoText.text = userData.name;
        }
    }
    
    private void OnDepositClick()
    {
        // PopupBank를 우선적으로 사용
        if (popupBankScript != null)
        {
            popupBankScript.ShowDepositPopup();
        }
        else if (popupBank != null)
        {
            // PopupBank GameObject를 직접 제어
            Transform depositPopup = popupBank.transform.Find("DepositPopup");
            if (depositPopup != null)
            {
                depositPopup.gameObject.SetActive(true);
            }
            Debug.Log("입금 팝업 표시");
        }
        else
        {
            Debug.LogError("PopupBank reference가 없습니다!");
        }
    }
    
    private void OnWithdrawClick()
    {
        // PopupBank를 우선적으로 사용
        if (popupBankScript != null)
        {
            popupBankScript.ShowWithdrawPopup();
        }
        else if (popupBank != null)
        {
            // PopupBank GameObject를 직접 제어
            Transform withdrawPopup = popupBank.transform.Find("WithdrawPopup");
            if (withdrawPopup != null)
            {
                withdrawPopup.gameObject.SetActive(true);
            }
            Debug.Log("출금 팝업 표시");
        }
        else
        {
            Debug.LogError("PopupBank reference가 없습니다!");
        }
    }
    
    public void ProcessDeposit(int amount)
    {
        bool success = GameManager.Instance.DepositToBalance(amount);
        
        if (success)
        {
            ShowMessage(string.Format("{0:N0}원이 입금되었습니다.", amount));
        }
        else
        {
            ShowMessage("입금에 실패했습니다. 현금이 부족합니다.");
        }
    }
    
    public void ProcessWithdraw(int amount)
    {
        bool success = GameManager.Instance.WithdrawFromBalance(amount);
        
        if (success)
        {
            ShowMessage(string.Format("{0:N0}원이 출금되었습니다.", amount));
        }
        else
        {
            ShowMessage("출금에 실패했습니다. 잔액이 부족합니다.");
        }
    }
    
    private void ShowMessage(string message)
    {
        Debug.Log(message);
        // 메시지 표시 UI가 있다면 여기서 처리
        StartCoroutine(ShowTemporaryMessage(message));
    }
    
    private IEnumerator ShowTemporaryMessage(string message)
    {
        // 임시 메시지 표시 로직
        yield return new WaitForSeconds(2f);
    }
    
    // 거래 내역 조회 (BankDataManager를 사용하는 경우)
    public void ShowTransactionHistory()
    {
        // GameManager를 사용하므로 이 기능은 별도 구현 필요
        Debug.Log("거래 내역 기능은 별도 구현이 필요합니다.");
    }
    
    // PopupBank에서 접근할 수 있도록 public 메서드 추가
    public BankData GetBankData()
    {
        // GameManager의 UserData를 BankData로 변환
        UserData userData = GameManager.Instance.userData;
        return new BankData(userData.name, userData.cash, userData.balance);
    }
}