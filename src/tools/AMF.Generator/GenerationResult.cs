using System.CodeDom.Compiler;
using System.Text;

namespace MuleSoft.RAMLGen
{
    public class GenerationResult
    {
        public string Content { get; set; }
        public Encoding Encoding { get; set; }

        public CompilerErrorCollection Errors { get; set; }
    }
}