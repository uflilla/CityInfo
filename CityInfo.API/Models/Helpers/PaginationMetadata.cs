namespace CityInfo.API.Models.Helpers
{
  public class PaginationMetadata
  {
    public int TotalPageCount { get; set; }
    public int TotalItemCount { get; set; }
    public int CurrentPage { get; set; }
    public int PageSize { get; set; }

    public PaginationMetadata(int itemCount, int currentPage, int pageSize)
    {
      TotalItemCount = itemCount;
      CurrentPage = currentPage;
      PageSize = pageSize;
      TotalPageCount = (int)Math.Ceiling(TotalItemCount / (double)PageSize);
    }
  }
}
