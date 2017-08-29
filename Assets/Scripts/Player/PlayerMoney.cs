using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoney : MonoBehaviour {

    public int startingMoney = 50;
    [SerializeField] private int moneyIncrementValue = 5;
    [SerializeField] private float moneyIncrementTime = 2.0f;

    private int money = 0;
    public int Money
    {
        get{return money;}
        set{money = value;}
    }

    // Use this for initialization
    void Start ()
    {
        money = startingMoney;
        StartCoroutine("IncrimentMoney");
	}

    IEnumerator IncrimentMoney()
    {
        for (;;)
        {
            yield return new WaitForSeconds(moneyIncrementTime);
            money += moneyIncrementValue;
        }
    }

    public void SubtractMoney(int subtractedAmount)
    {
        money -= subtractedAmount;
    }

    void OnGUI()
    {
        GUI.Box(new Rect(0, 5, 140, 25), "Money = " + money);
    }
}
