using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BankData
{
    public string userName;
    public int currentCash;
    public int balance;
    public List<TransactionRecord> transactionHistory;

    public BankData(string name, int cash, int bal)
    {
        userName = name;
        currentCash = cash;
        balance = bal;
        transactionHistory = new List<TransactionRecord>();
    }
}

[System.Serializable]
public class TransactionRecord
{
    public enum TransactionType
    {
        Deposit,
        Withdraw
    }

    public TransactionType type;
    public int amount;
    public string dateTime;
    public int balanceAfter;

    public TransactionRecord(TransactionType t, int amt, int balAfter)
    {
        type = t;
        amount = amt;
        dateTime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        balanceAfter = balAfter;
    }
}

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
        if (PlayerPrefs.HasKey(SAVE_KEY))
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
    }

    public BankData GetBankData()
    {
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