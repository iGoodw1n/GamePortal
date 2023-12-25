namespace GamePortal;

public class NumberService
{
    public Action OnNewNumber { get; set; }
    public List<int> Numbers = new();

    public void AddNumber(int number)
    {
        Numbers.Add(number);
        OnNewNumber?.Invoke();
    }
}
