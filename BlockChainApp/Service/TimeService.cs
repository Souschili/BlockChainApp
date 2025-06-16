using BlockChainApp.Service.Interfaces;

namespace BlockChainApp.Service
{
    /// <summary>
    /// Default implementation of <see cref="ITimeService"/> using system clock.
    /// </summary>
    internal class TimeService : ITimeService
    {
        /// <inheritdoc/>
        public DateTime GetLocalTime() => DateTime.Now;

        /// <inheritdoc/>
        public DateTime GetUtcTime() => DateTime.UtcNow;
    }
}
