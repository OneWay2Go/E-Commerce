namespace ECommerce.Application.Models;

public class ApiResult<T>
{
    private ApiResult(T data, bool succeeded, string errors)
    {
        Data = data;
        Succeeded = succeeded;
        Errors = errors;
    }

    public T Data { get; }
    public bool Succeeded { get; }
    public string Errors { get; }

    public static ApiResult<T> Success(T data)
    {
        return new ApiResult<T>(data, true, string.Empty);
    }

    public static ApiResult<T> Failure(string errors)
    {
        return new ApiResult<T>(default, false, errors);
    }
}
