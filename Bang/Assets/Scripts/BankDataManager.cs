using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BankDataManager : MonoBehaviour
{
    private static BankDataManager instance;
    public static BankDataManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindFirstObjectByType<BankDataManager>();
                if (instance == null)
                {
                    GameObject go = new GameObject("BankDataManager");
                    instance = go.AddComponent<BankDataManager>();
                }
            }
            return instance;
        }
    }

    private BankData currentBankData;
    private const string SAVE_KEY = "BankData";

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            LoadBankData();
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void LoadBankData()
    {
        // GameManager가 있으면 GameManager의 데이터를 사용
        if (GameManager.Instance != null && GameManager.Instance.userData != null)
        {
            UserData userData = GameManager.Instance.userData;
            currentBankData = new BankData(userData.name, userData.cash, userData.balance);
        }
        else if (PlayerPrefs.HasKey(SAVE_KEY))
        {
            string jsonData = PlayerPrefs.GetString(SAVE_KEY);
            currentBankData = JsonUtility.FromJson<BankData>(jsonData);
        }
        else
        {
            // 초기 데이터 설정
            currentBankData = new BankData("팀예찬", 100000, 50000);
            SaveBankData();
        }
    }

    private void SaveBankData()
    {
        string jsonData = JsonUtility.ToJson(currentBankData);
        PlayerPrefs.SetString(SAVE_KEY, jsonData);
        PlayerPrefs.Save();
        
        // GameManager에도 데이터 동기화
        if (GameManager.Instance != null && GameManager.Instance.userData != null)
        {
            GameManager.Instance.userData.name = currentBankData.userName;
            GameManager.Instance.userData.cash = currentBankData.currentCash;
            GameManager.Instance.userData.balance = currentBankData.balance;
        }
    }

    public BankData GetBankData()
    {
        // GameManager에서 최신 데이터 가져오기
        if (GameManager.Instance != null && GameManager.Instance.userData != null)
        {
            UserData userData = GameManager.Instance.userData;
            currentBankData.userName = userData.name;
            currentBankData.currentCash = userData.cash;
            currentBankData.balance = userData.balance;
        }
        
        return currentBankData;
    }

    public bool Deposit(int amount)
    {
        if (amount <= 0 || currentBankData.currentCash < amount)
        {
            return false;
        }

        currentBankData.currentCash -= amount;
        currentBankData.balance += amount;

        // 거래 기록 추가
        TransactionRecord record = new TransactionRecord(
            TransactionRecord.TransactionType.Deposit,
            amount,
            currentBankData.balance
        );
        currentBankData.transactionHistory.Add(record);

        SaveBankData();
        return true;
    }

    public bool Withdraw(int amount)
    {
        if (amount <= 0 || currentBankData.balance < amount)
        {
            return false;
        }

        currentBankData.balance -= amount;
        currentBankData.currentCash += amount;

        // 거래 기록 추가
        TransactionRecord record = new TransactionRecord(
            TransactionRecord.TransactionType.Withdraw,
            amount,
            currentBankData.balance
        );
        currentBankData.transactionHistory.Add(record);

        SaveBankData();
        return true;
    }

    public List<TransactionRecord> GetTransactionHistory()
    {
        return currentBankData.transactionHistory;
    }

    public void ResetData()
    {
        currentBankData = new BankData("팀예찬", 100000, 50000);
        SaveBankData();
    }
}