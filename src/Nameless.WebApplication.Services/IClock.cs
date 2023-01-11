namespace Nameless.WebApplication.Services {

    public interface IClock {

        #region Properties

        DateTime UtcNow { get; }
        DateTimeOffset OffsetUtcNow {
            get {
                return new DateTimeOffset(new DateTime(
                    ticks: (DateTime.UtcNow.Ticks / TimeSpan.TicksPerSecond) * TimeSpan.TicksPerSecond,
                    kind: DateTimeKind.Utc
                ));
            }
        }

        #endregion
    }
}
