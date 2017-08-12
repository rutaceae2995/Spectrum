using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Spectrum.Commands;

namespace Spectrum.UnitTest
{
    /// <summary>
    /// TimeShiftDelegateCommand test class.
    /// </summary>
    [TestClass]
    public class TimeShiftDelegateCommandTest
    {
        /// <summary>
        /// Gets or sets a TestContext instance.
        /// </summary>
        public TestContext TestContext
        {
            get;
            set;
        }

        /// <summary>
        /// Delay time test.
        /// </summary>
        [TestMethod]
        [TestCase(0)]
        [TestCase(1000)]
        [TestCase(3000)]
        [TestCase(5000)]
        public async Task DelayExecuteCommandTest()
        {
            await this.TestContext.RunAsync(
                (int ms) =>
                {
                    var sw = new System.Diagnostics.Stopwatch();
                    sw.Start();
                    var instance = new TimeShiftDelegateCommand(
                        () =>
                        {
                            var elapsed = sw.ElapsedMilliseconds;
                            Assert.IsTrue(elapsed >= ms && elapsed < ms + 50);
                        },
                        ms,
                        0,
                        false);
                    return instance.ExecuteAsync();
                });
        }

        /// <summary>
        /// Cooling time test.
        /// </summary>
        [TestMethod]
        [TestCase(1100, 1)]
        [TestCase(900, 2)]
        [TestCase(480, 2)]
        [TestCase(210, 5)]
        [TestCase(90, 10)]
        public async Task IntervalExecuteCommandTest()
        {
            await this.TestContext.RunAsync(
                async (int ms, int expectedCount) =>
                {
                    int count = 0;
                    var instance = new TimeShiftDelegateCommand(
                        () =>
                        {
                            count++;
                        },
                        0,
                        ms,
                        false);

                    for (int i = 0; i < 10; i++)
                    {
                        await instance.ExecuteAsync();
                        Task.Delay(100).Wait();
                    }

                    count.Is(expectedCount);
                });

        }

        /// <summary>
        /// Cooling time test.
        /// </summary>
        [TestMethod]
        [TestCase(1500, 2)]
        [TestCase(1100, 2)]
        [TestCase(500, 3)]
        [TestCase(250, 5)]
        [TestCase(200, 6)]
        [TestCase(50, 10)]
        public async Task IntervalExecuteCommandTest2()
        {
            await this.TestContext.RunAsync(
                async (int ms, int expectedCount) =>
                {
                    int count = 0;
                    var instance = new TimeShiftDelegateCommand(
                        () =>
                        {
                            count++;
                        },
                        0,
                        ms,
                        true);

                    for (int i = 0; i < 10; i++)
                    {
                        instance.Execute();
                        await Task.Delay(100);
                    }

                    await Task.Delay(1000);
                    count.Is(expectedCount);
                });
        }
    }
}