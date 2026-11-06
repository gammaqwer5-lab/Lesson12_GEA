namespace GameEventsAnalyzer;

internal class Program
{
    static void Main()
    {
        Console.WriteLine("=== Game Events Analyzer ==="); 

        List<GameEvent> events =
            GameEventGenerator.Generate(
                count: 1_000_000,
                seed: 12345);

        Console.WriteLine($"Всего событий: {events.Count:N0}");

        var linqResult =
            GameEventAnalyzer.AnalyzeWithLinq(events);

        var plinqResult =
            GameEventAnalyzer.AnalyzeWithPlinq(events);

        Console.WriteLine();
        Console.WriteLine("Топ игроков:");

        foreach (var player in plinqResult.Take(10))
        {
            Console.WriteLine(
                $"Игрок {player.PlayerId}: " +
                $"убийств {player.KillCount}, " +
                $"очков {player.TotalPoints}");
        }

        Console.WriteLine();

        bool same = linqResult
            .SequenceEqual(plinqResult);

        Console.WriteLine(
            $"Результаты LINQ и PLINQ совпадают: {same}");
    }
}