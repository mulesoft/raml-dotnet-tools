namespace Raml.Parser
{
    public class Error
    {
        public string Code { get; set; }
        public string Message { get; set; }
        public int? Line { get; set; }
        public int? Col { get; set; }
        public string Path { get; set; }
        public bool IsWarning { get; set; }
    }
}