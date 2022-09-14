using OnePassword.Vaults;

namespace OnePassword
{
    public class OnePasswordManagerWrapper
    {
        public OnePasswordManager Manager { get; set; }

        public string VaultName { get; set; } = "Private";

        public List<Vault> AvailableVaults = new List<Vault>();

        public Vault SelectedVault { get; set; } = null;


        public OnePasswordManagerWrapper(string path = "", string executable = "op.exe", bool verbose = true)
        {
            Manager = new OnePasswordManager(path, executable, verbose);
        }

        public Vault PerformLogin(string domain, string email, string secretKey, string password, string vault = "Private")
        {
            Manager.AddAccount(domain, email, secretKey, password);
            Manager.SignIn(password);
            AvailableVaults = Manager.GetVaults().ToList();
            SelectedVault = AvailableVaults.First(x => x.Name == vault);
            return SelectedVault;
        }
    }
}