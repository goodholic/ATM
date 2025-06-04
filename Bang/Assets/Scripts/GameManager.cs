using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // 싱글톤 인스턴스
    public static GameManager Instance;
    
    // UserData 클래스 변수
    public UserData userData;
    
    // 데이터 변경 이벤트 (UI 자동 업데이트용)
    public delegate void OnDataChanged();
    public static event OnDataChanged onDataChanged;
    
    private void Awake()
    {
        // 싱글톤 패턴 구현
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            
            // UserData 초기화 - PDF의 예시 데이터로 초기화
            userData = new UserData("김개호", 115000, 85000);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    private void Start()
    {
        // 시작 시 데이터 로그 출력
        PrintUserData();
    }
    
    // UserData 업데이트 메서드
    public void UpdateUserData(string name, int cash, int balance)
    {
        userData.name = name;
        userData.cash = cash;
        userData.balance = balance;
        
        // 데이터 변경 이벤트 발생
        onDataChanged?.Invoke();
    }
    
    // 현금에서 통장으로 입금
    public bool DepositToBalance(int amount)
    {
        if (userData.cash >= amount && amount > 0)
        {
            userData.cash -= amount;
            userData.balance += amount;
            
            Debug.Log($"입금 완료: {amount}원");
            Debug.Log($"현재 현금: {userData.cash}원, 통장 잔액: {userData.balance}원");
            
            // 데이터 변경 이벤트 발생
            onDataChanged?.Invoke();
            
            return true;
        }
        else
        {
            Debug.LogWarning($"입금 실패: 금액이 부족합니다. (요청 금액: {amount}원, 보유 현금: {userData.cash}원)");
            return false;
        }
    }
    
    // 통장에서 현금으로 출금
    public bool WithdrawFromBalance(int amount)
    {
        if (userData.balance >= amount && amount > 0)
        {
            userData.balance -= amount;
            userData.cash += amount;
            
            Debug.Log($"출금 완료: {amount}원");
            Debug.Log($"현재 현금: {userData.cash}원, 통장 잔액: {userData.balance}원");
            
            // 데이터 변경 이벤트 발생
            onDataChanged?.Invoke();
            
            return true;
        }
        else
        {
            Debug.LogWarning($"출금 실패: 잔액이 부족합니다. (요청 금액: {amount}원, 통장 잔액: {userData.balance}원)");
            return false;
        }
    }
    
    // 전체 자산 계산
    public int GetTotalAssets()
    {
        return userData.cash + userData.balance;
    }
    
    // 현재 유저 데이터 로그 출력
    public void PrintUserData()
    {
        Debug.Log("=== 현재 유저 데이터 ===");
        Debug.Log($"이름: {userData.name}");
        Debug.Log($"현금: {userData.cash}원");
        Debug.Log($"통장 잔액: {userData.balance}원");
        Debug.Log($"총 자산: {GetTotalAssets()}원");
        Debug.Log("======================");
    }
    
    // 데이터 리셋 (테스트용)
    public void ResetUserData()
    {
        userData = new UserData("김개호", 115000, 85000);
        onDataChanged?.Invoke();
        Debug.Log("유저 데이터가 초기값으로 리셋되었습니다.");
    }
}