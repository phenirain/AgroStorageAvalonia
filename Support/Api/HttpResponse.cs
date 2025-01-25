using System.Collections.Generic;

namespace desktop.Support.Api;

public class HttpResponse<T>
{
    public string Message { get; set; }
    public int Status { get; set; }
    public T? Data { get; set; }
}