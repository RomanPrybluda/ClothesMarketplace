namespace Domain.Сommon.Wrappers
{
    public class ValidationSummary(List<Exception> ExceptionsList)
    {
        public bool IsValid => ExceptionsList.Count == 0 || ExceptionsList is null;

        public List<Exception> GetExceptionsList()
        {
            return ExceptionsList;
        }

        public string GetErrorMessage()
        {
            if (ExceptionsList is null || ExceptionsList.Count == 0)
                return string.Empty;
            var errorMessages = ExceptionsList.Select(e => e.Message).ToList();
            return string.Join(", ", errorMessages);
        }
    }
}
