using CliWrap;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wox.Plugin.OnePassword.Models;

namespace Wox.Plugin.OnePassword
{

    public class OnePasswordManager
    {
        public string Path { get; set; } = "op.exe";

        public bool IsLoggedIn { get; set; } = false;

        public List<Vault> Vaults { get; set; } = new List<Vault>();

        public Vault SelectedVault { get; set; } = new Vault();
        public List<OnePasswordItem> Items { get; set; } = new List<OnePasswordItem>();

        public async Task<Vault> Login(string username, string vaultName = "Private")
        {
            List<string> parameters = new List<string>();
            if (string.IsNullOrEmpty(username) == false)
            {
                parameters.Add("--account " + username);
            }
            var result = await ExecuteCliCommand("signin", parameters);
            IsLoggedIn = result.CmdResult.ExitCode != -1;
            SelectedVault = this.Vaults.Find(e => e.Name == vaultName);
            Items = await this.GetItems();
            SelectedVault.Accounts = Items.Where(e => e.Vault.Name == vaultName).ToList();
            return this.SelectedVault;
        }

        private async Task<ExecutionResult> ExecuteCliCommand(string cmd, List<string> cliParams)
        {
            if (cliParams is null)
            {
                throw new ArgumentNullException(nameof(cliParams));
            }
            cliParams.Add("--format json");
            cliParams.Add("--no-color");

            var stdOutBuffer = new StringBuilder();
            var stdErrBuffer = new StringBuilder();
            var result = await Cli.Wrap(Path)
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
            var vaults = await ExecuteCliCommand("vault list", cliParams);
            Vaults = JsonConvert.DeserializeObject<List<Vault>>(vaults.Data);
            return Vaults;
        }

        public async Task<OnePasswordItem> GetItem(string itemId, bool shareLink)
        {
            var cliParams = new List<string>();
            if(shareLink)
            {
                cliParams.Add("--share-link");
            }
            var onePasswordItems = await ExecuteCliCommand("item get " + itemId, cliParams);
            var onePasswordItem = JsonConvert.DeserializeObject<OnePasswordItem>(onePasswordItems.Data);
            return onePasswordItem;
        }

        public async Task<List<OnePasswordItem>> GetItems()
        {
            var cliParams = new List<string> {"--long"};
            var onePasswordItems = await this.ExecuteCliCommand("item list", cliParams);
            Items = JsonConvert.DeserializeObject<List<OnePasswordItem>>(onePasswordItems.Data);
            return Items;
        }
    }
}
