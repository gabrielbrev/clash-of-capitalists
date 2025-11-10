using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerMoneyCard : MoneyCard
{
    private int GetAmountPerPlayer(int playerCount, int totalAmount)
    {
        if (playerCount == 1) 
        {
            return 0;
        }
        else
        {
            int dividedAmount = totalAmount / (playerCount - 1);
            return dividedAmount - (dividedAmount % 10_000);
        }
    }

    private void UpdateDescription(int playerCount)
    {
        string description;

        if (GetOperationType() == MonetaryOperation.Income)
        {
            description = $"Receba {GetAmount():C} de cada jogador.";
        }
        else
        {
            if (playerCount == 1) return;
            description = $"Pague {GetAmountPerPlayer(playerCount, GetAmount()):C} para cada jogador.";
        }
        
        SetDescription(description);
    }

    public override IEnumerator Use(Player player)
    {
        IReadOnlyList<Player> players = Player.GetAll();
        if (players.Count <= 1) yield break;

        if (GetOperationType() == MonetaryOperation.Income)
        {
            int totalAmount = 0;

            foreach (Player p in players)
            {
                if (p == player) continue;

                int amount = GetAmount();
                int balance = p.GetBalance();
                if (balance >= amount)
                {
                    yield return p.SubtractBalance(amount);
                    totalAmount += amount;
                }
            }

            player.AddBalance(totalAmount);
        }
        else
        {
            int amount = GetAmount();
            int amountPerPlayer = GetAmountPerPlayer(players.Count, amount);
            int newAmount = amountPerPlayer * (players.Count - 1);
            int balance = player.GetBalance();

            if (balance <= amount) yield break;

            foreach (Player p in players)
            {
                if (p == player) continue;
                p.AddBalance(amountPerPlayer);
            }

            yield return player.SubtractBalance(newAmount);
        }

        yield break;
    }

    void OnEnable()
    {
        Player.OnInstanceCountChange += UpdateDescription;
    }

    void OnDisable()
    {
        Player.OnInstanceCountChange -= UpdateDescription;
    }
}