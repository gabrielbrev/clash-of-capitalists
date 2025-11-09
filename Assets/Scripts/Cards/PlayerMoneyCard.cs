using System;
using System.Collections;
using System.Collections.Generic;

public class PlayerMoneyCard : MoneyCard
{
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
            int amountPerPlayer = amount / (players.Count - 1);
            int balance = player.GetBalance();

            if (balance <= amount) yield break;

            foreach (Player p in players)
            {
                if (p == player) continue;
                p.AddBalance(amountPerPlayer);
            }

            yield return player.SubtractBalance(amount);
        }

        yield break;
    }

    void Start() {
        string description;

        if (GetOperationType() == MonetaryOperation.Income)
        {
            description = $"Receba {GetAmount():C} de cada jogador."; 
        }
        else
        {
            IReadOnlyList<Player> players = Player.GetAll();
            int amount = GetAmount();
            int amountPerPlayer = amount / (players.Count - 1);
            description = $"Pague {amountPerPlayer:C} para cada jogador.";
        }

        SetDescription(description);
    }
}