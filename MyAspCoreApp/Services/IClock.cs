using System;

namespace MyAspCoreApp.Services
{
    public interface IClock
    {
        DateTimeOffset UtcNow { get; }
    }
}
