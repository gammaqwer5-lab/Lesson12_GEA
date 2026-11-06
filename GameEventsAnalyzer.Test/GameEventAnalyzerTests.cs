using GameEventsAnalyzer;

namespace GameEventsAnalyzer.Tests;

public class GameEventAnalyzerTests
{
    private static readonly DateTime TestDate =
        new(2026, 1, 1);

    [Fact]
    public void LinqAndPlinq_ReturnSameStatistics()
    {
        var events = new List<GameEvent>
        {
            new(1, 1, "Kill", 100, TestDate),
            new(2, 1, "Kill", 150, TestDate),
            new(3, 2, "Kill", 200, TestDate),
            new(4, 1, "Death", 0, TestDate),
            new(5, 2, "Loot", 20, TestDate)
        };

        var linq = GameEventAnalyzer.AnalyzeWithLinq(events);
        var plinq = GameEventAnalyzer.AnalyzeWithPlinq(events);

        Assert.Equal(linq, plinq);
        Assert.Equal(2, linq.Count);

        Assert.Equal(
            new PlayerStatistics(1, 2, 250),
            linq[0]);

        Assert.Equal(
            new PlayerStatistics(2, 1, 200),
            linq[1]);
    }

    [Fact]
    public void Analyze_EmptyCollection_ReturnsEmptyResult()
    {
        var events = new List<GameEvent>();
        var result =
            GameEventAnalyzer.AnalyzeWithPlinq(events);

        Assert.Empty(result);
    }

    [Fact]
    public void Analyze_WithoutKills_ReturnsEmptyResult()
    {
        var events = new List<GameEvent>
        {
            new(1, 1, "Death", 0, TestDate),
            new(2, 2, "Loot", 20, TestDate)
        };
        var result =
            GameEventAnalyzer.AnalyzeWithPlinq(events);
        Assert.Empty(result);
    }

    [Fact]
    public void Analyze_Null_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(
            () => GameEventAnalyzer.AnalyzeWithLinq(null!));

        Assert.Throws<ArgumentNullException>(
            () => GameEventAnalyzer.AnalyzeWithPlinq(null!));
    }

    [Fact]
    public void GetKillsOrdered_PreservesSourceOrder()
    {
        var events = new List<GameEvent>
        {
            new(1, 1, "Kill", 100, TestDate),
            new(2, 1, "Death", 0, TestDate),
            new(3, 2, "Kill", 200, TestDate),
            new(4, 1, "Kill", 150, TestDate)
        };

        var result =
            GameEventAnalyzer.GetKillsOrdered(events);

        Assert.Equal(
            new[] { 1, 3, 4 },
            result.Select(e => e.Id));
    }

    [Fact]
    public void GetKillsSortedByTime_SortsEvents()
    {
        var events = new List<GameEvent>
        {
            new(1, 1, "Kill", 100, TestDate.AddMinutes(5)),
            new(2, 1, "Kill", 150, TestDate.AddMinutes(1)),
            new(3, 2, "Kill", 200, TestDate.AddMinutes(3))
        };

        var result =
            GameEventAnalyzer.GetKillsSortedByTime(events);

        Assert.Equal(
            new[] { 2, 3, 1 },
            result.Select(e => e.Id));
    }
}