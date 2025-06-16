namespace BlockChainApp.Service.Interfaces
{
    /// <summary>
    /// Provides methods to retrieve current date and time.
    /// </summary>
    public interface ITimeService
    {
        /// <summary>
        /// Gets the current local time.
        /// </summary>
        /// <returns>The local <see cref="DateTime"/>.</returns>
        DateTime GetLocalTime();

        /// <summary>
        /// Gets the current UTC time.
        /// </summary>
        /// <returns>The UTC <see cref="DateTime"/>.</returns>
        DateTime GetUtcTime();
    }
}

