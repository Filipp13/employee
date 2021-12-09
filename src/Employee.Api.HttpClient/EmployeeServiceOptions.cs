namespace Employee.Api.ServiceClient
{
    public sealed class EmployeeServiceOptions
    {
        public const string SectionName = "Endpoints:Employee";

        public string? BaseAddress { get; set; }

        /// <summary>
        ///   Получает или задаёт общее время ожидания для всех попыток в секундах.
        /// </summary>
        public int OverallTimeout { get; set; } = 60;

        /// <summary>
        ///   Получает или задаёт время ожидания для каждой отдельной попыти в секундах.
        /// </summary>
        /// <remarks>
        ///   Общее время выполнения запроса ограничено значением <see cref="OverallTimeout" />.
        ///   Независимо от количества повторных попыток.
        /// </remarks>
        public int TryTimeout { get; set; } = 6;

        /// <summary>
        ///   Получает или задаёт время задержки первой повторной попытки в секундах.
        /// </summary>
        /// <remarks>
        ///   Последующие попытки будут выполняться с экспоненциальной задержкой.
        /// </remarks>
        public int RetryDelay { get; set; } = 2;

        /// <summary>Задаёт максимальное количество повторных попыток.</summary>
        public int RetryCount { get; set; } = 10;
    }
}