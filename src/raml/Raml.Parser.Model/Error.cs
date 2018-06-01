namespace Raml.Parser
{
    public class Error
    {
        public string Code { get; set; }
        public string Message { get; set; }
        public string Path { get; set; }
        public bool IsWarning { get; set; }
        public Position Start { get; set; }
        public Position End { get; set; }
    }

    public class Position
    {
        public int? Line { get; set; }
        public int? Col { get; set; }
        public int? Pos { get; set; }

        public bool HasValue
        {
            get { return !(Line == null && Col == null && Pos == null); }
        }
    }
}