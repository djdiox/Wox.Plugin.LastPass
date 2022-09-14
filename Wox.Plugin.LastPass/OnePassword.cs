using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wox.Plugin.OnePassword.Models;
using CliWrap;
using Newtonsoft.Json;
using System.Windows.Controls.Primitives;

namespace Wox.Plugin.OnePassword
{

    public class OnePasswordManager
    {
        public string Path { get; set; } = "op.exe";

        public bool IsLoggedIn { get; set; } = false;

        public List<Vault> Vaults { get; set; } = new List<Vault>();

        public List<OnePasswordItem> Items { get; set; } = new List<OnePasswordItem>();

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

        private async Task<ExecutionResult> ExecuteCliCommand(string cmd, List<string> cliParams)
        {
            if (cliParams is null)
            {
                throw new ArgumentNullException(nameof(cliParams));
            }
            cliParams.Add("--format json");

            var stdOutBuffer = new StringBuilder();
            var stdErrBuffer = new StringBuilder();
            var result = await Cli.Wrap(this.Path)
                .WithArguments(new[] { cmd, }.Concat(cliParams))
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
            } else
            {
                returnObj.Success = true;
            }
            return returnObj;
        }

        public async Task<List<Vault>> GetVaults()
        {
            var cliParams = new List<string>();
            var vaults = await this.ExecuteCliCommand("vault list", cliParams);
            this.Vaults = JsonConvert.DeserializeObject<List<Vault>>(vaults.Data);
            return this.Vaults;
        }

        public async Task<List<OnePasswordItem>> GetItems()
        {
            var cliParams = new List<string>();
            cliParams.Add("--long");
            var onePasswordItems = await this.ExecuteCliCommand("item list", cliParams);
            this.Items = JsonConvert.DeserializeObject<List<OnePasswordItem>>(onePasswordItems.Data);
            return this.Items;
        }
    }
}
