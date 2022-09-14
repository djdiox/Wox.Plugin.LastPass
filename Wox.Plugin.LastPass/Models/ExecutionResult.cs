using CliWrap;

namespace Wox.Plugin.OnePassword.Models
{
    public class ExecutionResult
    {
        public bool Success { get; set; } = false;

        public string Data { get; set; }

        public string Error { get; set; }

        public CommandResult CmdResult { get; set; }
    }
}
