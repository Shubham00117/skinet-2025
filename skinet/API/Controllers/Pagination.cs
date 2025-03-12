using System;

namespace API.Controllers;

public class Pagination<T>(int pageIndex, int PageSize, int count, IReadOnlyList<T> data)
{
  public int pageIndex { get; set; } = pageIndex;
  public int PageSize { get; set; } = PageSize;
  public int Count { get; set; } = count;
  public IReadOnlyList<T> Data { get; set; } = data;


}
