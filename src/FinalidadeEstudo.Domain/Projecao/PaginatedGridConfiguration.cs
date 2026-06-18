using System.Globalization;

namespace FinalidadeEstudo.Domain.Projecao;

public class PaginatedGridConfiguration
{

    public string? OrderColumn { get; set; }

    public string? OrderDir { get; set; }
    public int Start { get; set; } = 0;
    public int OffSet { get; set; } = 120;

    public string SearchValue { get; set; } = string.Empty;

    public int? SearchValueIntFormat =>
        int.TryParse(SearchValue, out int result) ? result : null;

    public DateTime? SearchValueDateTimeFormat
    {
        get
        {
            return DateTime.TryParse(SearchValue, CultureInfo.InvariantCulture, DateTimeStyles.None, out var result)
                    ? result
                    : null;
        }
    }
}
