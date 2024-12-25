namespace Budget;

public class Tests
{
    private BudgetService _budgetService;

    [SetUp]
    public void Setup()
    {
        _budgetService = new BudgetService(); 
    }

    [Test]
    public void invalid_period()
    {
        var actual = _budgetService.Query(new DateTime(2024, 1, 5), new DateTime(2024, 1, 1));
        Assert.That(actual, Is.EqualTo(0)); 
    }
}