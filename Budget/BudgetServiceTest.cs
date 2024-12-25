using NSubstitute;

namespace Budget;

public class BudgetServiceTest
{
    private BudgetService _budgetService;
    private IBudgetRepo? _ibugetRepo;

    [SetUp]
    public void Setup()
    {
        _ibugetRepo = Substitute.For<IBudgetRepo>();
        _budgetService = new BudgetService(_ibugetRepo);
    }

    [Test]
    public void invalid_period()
    {
        var actual = _budgetService.Query(new DateTime(2024, 1, 5), new DateTime(2024, 1, 1));
        AmountShouldBe(actual, 0);
    }

    private static void AmountShouldBe(decimal actual, int expected)
    {
        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void part_of_day_in_same_month_from_first_day()
    {
        GivenBudgetList();
        var actual = _budgetService.Query(new DateTime(2024, 1, 1), new DateTime(2024, 1, 5));
        AmountShouldBe(actual, 500);
    }

    [Test]
    public void whole_month()
    {
        GivenBudgetList();
        var actual = _budgetService.Query(new DateTime(2024, 1, 1), new DateTime(2024, 1, 31));
        AmountShouldBe(actual, 3100);
    }

    [Test]
    public void part_of_day_in_same_month_from_not_first_day()
    {
        GivenBudgetList();
        var actual = _budgetService.Query(new DateTime(2024, 1, 28), new DateTime(2024, 1, 31));
        AmountShouldBe(actual, 400);
    }

    [Test]
    public void part_of_day_in_different_month_until_last_day()
    {
        GivenBudgetList();
        var actual = _budgetService.Query(new DateTime(2024, 1, 28), new DateTime(2024, 2, 29));
        AmountShouldBe(actual, 690);
    }

    [Test]
    public void part_of_day_in_different_month_from_first_day()
    {
        GivenBudgetList();
        var actual = _budgetService.Query(new DateTime(2024, 1, 1), new DateTime(2024, 2, 2));
        AmountShouldBe(actual, 3120);
    }

    private void GivenBudgetList()
    {
        _ibugetRepo.GetAll().ReturnsForAnyArgs(x => new List<BudgetObj>()
        {
            new BudgetObj() { YearMonth = "202401", Amount = 3100 },
            new BudgetObj() { YearMonth = "202402", Amount = 290 }
        });
    }
}