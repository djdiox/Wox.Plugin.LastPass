
using OnePassword;
using OnePassword.Vaults;

namespace OnePasswordWrapper
{
    public class OnePasswordManagerWrapper
    {
        public OnePasswordManager Manager { get; set; }

        public string VaultName { get; set; } = "Private";

        public List<Vault> AvailableVaults = new List<Vault>();

        public Vault SelectedVault { get; set; } = null;


        public OnePasswordManagerWrapper(string path = "", string executable = "op.exe", bool verbose = true)
        {
            this.Manager = new OnePasswordManager(path, executable, verbose);
        }

        public Vault PerformLogin(string domain, string email, string secretKey, string password, string vault = "Private")
        {
            this.Manager.AddAccount(domain, email, secretKey, password);
            this.Manager.SignIn(password);
            this.AvailableVaults = this.Manager.GetVaults().ToList();
            this.SelectedVault = this.AvailableVaults.First(x => x.Name == vault);
            return this.SelectedVault;
        }
    }
}