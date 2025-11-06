public class Dice
{
    public static (int, int) Roll()
    {
        return (UnityEngine.Random.Range(1, 7), UnityEngine.Random.Range(1, 7));
    }
}