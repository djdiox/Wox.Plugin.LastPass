using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CliWrap;

namespace Wox.Plugin.LastPass
{
    public class ExecutionResult
    {
        public bool Success { get; set; } = false;

        public string Data { get; set; }

        public string Error { get; set; }

        public CommandResult CmdResult { get; set; }
    }
    public class OnePassword
    {
        public string Path { get; set; } = "op.exe";

        public bool IsLoggedIn { get; set; } = false;

        public List<string> Vaults { get; set; } = new List<string>();

        public async Task<bool> Login(string username)
        {
            List<string> parameters = new List<string>();
            if (string.IsNullOrEmpty(username) == false)
            {
                parameters.Add("--account " + username);
            }
            var result = await this.ExecuteCliCommand("signin", parameters);
            this.IsLoggedIn = result.CmdResult.ExitCode != -1;
            return this.IsLoggedIn;
        }

        private async Task<ExecutionResult> ExecuteCliCommand(string cmd, List<string> parameters)
        {
            if (parameters is null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            var stdOutBuffer = new StringBuilder();
            var stdErrBuffer = new StringBuilder();
            var result = await Cli.Wrap(this.Path)
                .WithArguments(new[] { cmd, }.Concat(parameters))
                .WithStandardOutputPipe(PipeTarget.ToStringBuilder(stdOutBuffer))
                .WithStandardErrorPipe(PipeTarget.ToStringBuilder(stdErrBuffer))
                .ExecuteAsync();
            var stdOut = stdOutBuffer.ToString();
            var stdErr = stdErrBuffer.ToString();
            var returnObj = new ExecutionResult
            {
                CmdResult = result,
                Data = stdOut
            };
            if (!string.IsNullOrEmpty(stdErr))
            {
                returnObj.Error = stdErr;
            }else
            {
                returnObj.Success = true;
            }
            return returnObj;
        }

        public async Task<List<string>> GetVaults()
        {
            var vaults = await this.ExecuteCliCommand("list vaults", new List<string>());
            this.Vaults = vaults.Data.Split('\n').ToList();
            return this.Vaults;

        }

        private void Result_OutputDataReceived(object sender, System.Diagnostics.DataReceivedEventArgs e)
        {
            Console.WriteLine("Received Result", e.Data);
        }

        private void Result_ErrorDataReceived(object sender, System.Diagnostics.DataReceivedEventArgs e)
        {
            Console.WriteLine("Error while processing 1Password CLI", e.Data);
        }
    }
}
