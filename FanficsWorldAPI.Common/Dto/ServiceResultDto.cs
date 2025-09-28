namespace FanficsWorldAPI.Common.Dto
{
    public class ServiceResultDto
    {
        public bool IsSuccess { get; set; }

        public List<string> Errors { get; set; } = [];

        public ServiceResultDto() { }

        public ServiceResultDto(bool isSuccess)
        {
            IsSuccess = isSuccess;
        }

        public ServiceResultDto(bool isSuccess, params string[] errors) : this(isSuccess)
        {
            Errors = [..errors];
        }
    }

    public class ServiceResultDto<T> : ServiceResultDto
    {
        public T? Value { get; set; }

        public ServiceResultDto() : base() { }

        public ServiceResultDto(bool isSuccess) : base(isSuccess) { }

        public ServiceResultDto(bool isSuccess, params string[] errors) : base(isSuccess, errors) { }

        public ServiceResultDto(bool isSuccess, T? value, params string[] errors) : this(isSuccess, errors)
        {
            Value = value;
        }
    }
}
