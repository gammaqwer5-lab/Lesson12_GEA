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

        var unordered =
            GameEventAnalyzer.GetKillsUnordered(events);

        var ordered =
            GameEventAnalyzer.GetKillsOrdered(events);

        var sorted =
            GameEventAnalyzer.GetKillsSortedByTime(events);

        Console.WriteLine();
        Console.WriteLine($"Kill без AsOrdered: {unordered.Count}");
        Console.WriteLine($"Kill с AsOrdered: {ordered.Count}");
        Console.WriteLine($"Kill после сортировки: {sorted.Count}");

        Console.WriteLine();
        Console.WriteLine("Первые 10 событий с сохранением исходного порядка:");

        foreach (var gameEvent in ordered.Take(10))
        {
            Console.WriteLine(
                $"ID: {gameEvent.Id}, " +
                $"Игрок: {gameEvent.PlayerId}, " +
                $"Время: {gameEvent.Timestamp:HH:mm:ss}");
        }

        bool isOrdered = ordered
        .Select(e => e.Id)
        .SequenceEqual(
            ordered.Select(e => e.Id).OrderBy(id => id));

        Console.WriteLine($"Исходный порядок сохранён: {isOrdered}");

        BenchmarkRunner.Compare(events);
    }
}