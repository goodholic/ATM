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