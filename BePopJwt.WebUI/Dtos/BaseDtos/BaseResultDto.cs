
namespace BePopJwt.WebUI.Dtos.BaseDtos
{
    public class BaseResultDto<T>
    {
        public T? Data { get; set; }
        public List<ErrorDetailDto>? Errors { get; set; }
        public bool IsSuccess => Errors == null || Errors.Count == 0;
    }
}
