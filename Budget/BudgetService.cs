using NSubstitute;

namespace Budget;

public class BudgetService
{
    private IBudgetRepo _iBugetRepo;

    public BudgetService(IBudgetRepo iBugetRepo)
    {
        _iBugetRepo = iBugetRepo;
    }

    public decimal Query(DateTime start, DateTime end)
    {
        if (start > end)
        {
            return 0;
        }

        var budgetList = _iBugetRepo.GetAll();
        budgetList = budgetList.Where(x =>
            Convert.ToInt32(start.ToString("yyyyMM")) <= Convert.ToInt32(x.YearMonth) &&
            Convert.ToInt32(end.ToString("yyyyMM")) >= Convert.ToInt32(x.YearMonth)).ToList();

        return (budgetList.Sum(x => x.Amount) - GetAmountOfStartMonth(budgetList.FirstOrDefault(), start)) -
               (GetAmountOfEndMonth(budgetList.LastOrDefault(), end));
    }

    private decimal GetAmountOfStartMonth(BudgetObj budgetOfMonth, DateTime start)
    {
        var daysToCalculate = start.Day - 1;
        return (budgetOfMonth.Amount / DateTime.DaysInMonth(start.Year, start.Month)) * daysToCalculate;
    }

    private decimal GetAmountOfEndMonth(BudgetObj budgetOfMonth, DateTime end)
    {
        var daysInMonth = DateTime.DaysInMonth(end.Year, end.Month);
        return (budgetOfMonth.Amount / daysInMonth) * (daysInMonth - end.Day);
    }
}