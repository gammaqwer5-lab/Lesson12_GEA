namespace GameEventsAnalyzer;
public static partial class GameEventAnalyzer
{
    public static List<PlayerStatistics> AnalyzeWithLinq(
        IEnumerable<GameEvent> events)
    {
        ArgumentNullException.ThrowIfNull(events);

        return events
            .Where(e => e.Type == "Kill")
            .GroupBy(e => e.PlayerId)
            .Select(group => new PlayerStatistics(
                PlayerId: group.Key,
                KillCount: group.Count(),
                TotalPoints: group.Sum(e => (long)e.Points)))
            .OrderByDescending(x => x.TotalPoints)
            .ThenBy(x => x.PlayerId)
            .ToList();
    }
}
// PLINQ.
// Для учебного проекта это удобный пример, 
// но при очень больших объёмах данных 
// группировка и сортировка могут потребовать значительной памяти и времени.
public static partial class GameEventAnalyzer
{
    public static List<PlayerStatistics> AnalyzeWithPlinq(
        IEnumerable<GameEvent> events)
    {
        ArgumentNullException.ThrowIfNull(events);
        // GroupBy, Select и OrderByDescending здесь выполняются в рамках PLINQ-запроса.
        // Итоговый список уже отсортирован по сумме очков, поэтому `AsOrdered()` 
        // для сохранения исходного порядка событий нам не требуется.
        return events
            .AsParallel()
            .Where(e => e.Type == "Kill")
            .GroupBy(e => e.PlayerId)
            .Select(group => new PlayerStatistics(
                PlayerId: group.Key,
                KillCount: group.Count(),
                TotalPoints: group.Sum(e => (long)e.Points)))
            .OrderByDescending(x => x.TotalPoints)
            .ThenBy(x => x.PlayerId)
            .ToList();
    }
}