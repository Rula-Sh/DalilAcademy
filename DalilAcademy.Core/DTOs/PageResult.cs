namespace DalilAcademy.Core.DTOs
{
    public record PageResult<T>(List<T> Items, int Page, int PageSize, int TotalCount)
    {
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    }
    public record PriceFilterQueryDto(int Min = 0, int Max = 9999);
}
