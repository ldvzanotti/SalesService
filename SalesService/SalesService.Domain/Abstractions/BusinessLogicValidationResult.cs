namespace SalesService.Domain.Abstractions
{
    public record BusinessLogicValidationResult
    {
        public bool IsValid { get; set; }
        public List<string> Errors { get; set; }

        private BusinessLogicValidationResult(bool isValid, List<string> errors)
        {
            IsValid = isValid;
            Errors = errors;
        }

        public static BusinessLogicValidationResult Invalid(string error) => new(false, [error]);

        public static BusinessLogicValidationResult Invalid(List<string> errors) => new(false, errors);

        public static BusinessLogicValidationResult Valid() => new(true, []);
    }
}
