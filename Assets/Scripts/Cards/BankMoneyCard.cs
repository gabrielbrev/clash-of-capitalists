using System.Collections;

public class BankMoneyCard : MoneyCard
{
    public override IEnumerator Use(Player player)
    {
        int amount = GetAmount();

        if (GetOperationType() == MonetaryOperation.Income)
        {
            player.AddBalance(amount);
        }
        else
        {
            int balance = player.GetBalance();
            if (balance > amount)
            {
                yield return player.SubtractBalance(amount);
            }
        }

        yield break;
    }

    void Start() {
        string description;
        int amount = GetAmount();

        if (GetOperationType() == MonetaryOperation.Income)
        {
            description = $"Receba {amount:C} do banco."; 
        }
        else
        {
            description = $"Pague {amount:C} ao banco.";
        }

        SetDescription(description);
    }
}