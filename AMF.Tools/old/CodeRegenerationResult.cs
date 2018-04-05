namespace MuleSoft.RAML.Tools
{
    public class CodeRegenerationResult
    {

        public static CodeRegenerationResult Success(string content)
        {
            return new CodeRegenerationResult { Content = content, IsSuccess = true };
        }

        public static CodeRegenerationResult Error(string errors)
        {
            return new CodeRegenerationResult { ErrorMessage = errors };
        }

        public bool IsSuccess { get; set; }
        public string Content { get; set; }
        public string ErrorMessage { get; set; }
    }
}