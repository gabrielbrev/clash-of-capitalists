using System;
using System.Collections;
using UnityEngine;

public abstract class MoneyCard : Card
{
    [SerializeField] private MonetaryOperation type;
    [SerializeField] private int amount;

    protected MonetaryOperation GetOperationType()
    {
        return type;
    }

    protected int GetAmount()
    {
        return amount;
    }
}