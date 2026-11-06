using Microsoft.VisualStudio.TestPlatform.ObjectModel;

namespace GameEventsAnalyzer.Test
{
    public class UnitTest1
    {
        DateTime TestDate = new(2026, 1, 1);

        [Fact]
        public void Todo5()
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

            Assert.True(linq.SequenceEqual(plinq));
        }
        
        [Fact]
        public void Todo6()
        {
            var events = new List<GameEvent>();

            var linq = GameEventAnalyzer.AnalyzeWithLinq(events);
            var plinq = GameEventAnalyzer.AnalyzeWithPlinq(events);

            Assert.Empty(linq);
            Assert.Empty(plinq);
        }
        
        [Fact]
        public void Todo7()
        {
            var events = new List<GameEvent>
            {
                new(1, 1, "Death", 0, TestDate),
                new(2, 2, "Loot", 20, TestDate)
            };

            var linq = GameEventAnalyzer.AnalyzeWithLinq(events);
            var plinq = GameEventAnalyzer.AnalyzeWithPlinq(events);

            Assert.Empty(linq);
            Assert.Empty(plinq);
        }
        
        [Fact]
        public void Todo8()
        {
            var events = new List<GameEvent>
            {
                new(1, 7, "Kill", 100, TestDate),
                new(2, 7, "Kill", 50, TestDate),
                new(3, 7, "Death", 0, TestDate)
            };

            var linq = GameEventAnalyzer.AnalyzeWithLinq(events);
            var plinq = GameEventAnalyzer.AnalyzeWithPlinq(events);

            var linqPlayer = Assert.Single(linq);
            var plinqPlayer = Assert.Single(plinq);

            Assert.Equal(7, linqPlayer.PlayerId);
            Assert.Equal(2, linqPlayer.KillCount);
            Assert.Equal(150L, linqPlayer.TotalPoints);

            Assert.Equal(7, plinqPlayer.PlayerId);
            Assert.Equal(2, plinqPlayer.KillCount);
            Assert.Equal(150L, plinqPlayer.TotalPoints);
        }
        
        [Fact]
        public void Todo9()
        {
            Assert.Throws<ArgumentNullException>(() => GameEventAnalyzer.AnalyzeWithLinq(null!));
            Assert.Throws<ArgumentNullException>(() => GameEventAnalyzer.AnalyzeWithPlinq(null!));
        }
    }
}