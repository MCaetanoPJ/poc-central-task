using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CentralTask.Infra.Data.Context;

public class DateTimeConverter : ValueConverter<DateTime, DateTime>
{
    public DateTimeConverter() : base(
        dateTime => dateTime.ToUniversalTime(),
        dateTime => dateTime)
    {
    }
}