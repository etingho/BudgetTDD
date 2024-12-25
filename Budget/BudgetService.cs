namespace Budget;

public class BudgetService
{
    public decimal Query(DateTime start, DateTime end)
    {
        if (start > end)
        {
            return 0;
        }
        return 100;
    }
}