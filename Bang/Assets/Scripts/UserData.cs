using System;
using UnityEngine;

[System.Serializable]
public class UserData
{
    public string name;      // 유저 이름
    public int cash;         // 현금
    public int balance;      // 통장 잔액
    
    // 생성자
    public UserData(string name, int cash, int balance)
    {
        this.name = name;
        this.cash = cash;
        this.balance = balance;
    }
    
    // 기본 생성자
    public UserData()
    {
        this.name = "";
        this.cash = 0;
        this.balance = 0;
    }
}