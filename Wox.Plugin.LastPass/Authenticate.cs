using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MaterialSkin;
using MaterialSkin.Controls;
using Wox.Plugin.OnePassword.Models;
using System.Threading;

namespace Wox.Plugin.OnePassword
{
    public partial class Authenticate : MaterialForm
    {
        public Vault vault { get; set; }
        public TwoFactor twofactor { get; set; }

        private void Authenticate_Load(object sender, EventArgs e)
        {
            usernameTextField.Text = Properties.Settings.Default.username;
            passwordTextField.Text = Properties.Settings.Default.password;
            rememberUsernameCheckBox.Checked = Properties.Settings.Default.saveusername;
            rememberPasswordCheckBox.Checked = Properties.Settings.Default.savepassword;
            selectedVaultRememberCheckBox.Checked = Properties.Settings.Default.savevault;
        }

        public Authenticate()
        {
            InitializeComponent();
            MaterialSkinManager materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;

            // Configure color schema
            materialSkinManager.ColorScheme = new ColorScheme(
                Primary.Blue400, Primary.Blue500,
                Primary.Blue500, Accent.LightBlue200,
                TextShade.WHITE
            );
            setEnabled(true);
        }

        private void loginButton_Click(object sender, EventArgs e)
        {
            if (usernameTextField.Text != "")
            {
                try
                {
                    var manager = new OnePasswordManager();
                    var thread = new Thread(
                       async () =>
                       {
                           await manager.Login(usernameTextField.Text, selectedVaultTextbox.Text);
                           //await manager.GetVaults();
                           manager.Vaults.ForEach(vault =>
                           {
                               vault.Accounts = manager.Items.Where<OnePasswordItem>(item => item.Vault.Id == vault.Id).ToList();
                           });
                           vault = manager.Vaults.Find((res) => res.Name == selectedVaultTextbox.Text);
                       });

                    thread.Start();
                    thread.Join();

                    if (rememberUsernameCheckBox.Checked)
                        Properties.Settings.Default.username = usernameTextField.Text;
                    else
                        Properties.Settings.Default.username = "";

                    if (rememberPasswordCheckBox.Checked)
                        Properties.Settings.Default.password = passwordTextField.Text;
                    else
                        Properties.Settings.Default.password = "";

                    if(selectedVaultRememberCheckBox.Checked)
                    {
                        Properties.Settings.Default.vault = selectedVaultTextbox.Text;
                    }
                    Properties.Settings.Default.savevault = selectedVaultRememberCheckBox.Checked;
                    Properties.Settings.Default.saveusername = rememberUsernameCheckBox.Checked;
                    Properties.Settings.Default.savepassword = rememberPasswordCheckBox.Checked;
                    Properties.Settings.Default.Save();

                    
                    Main.form.twofactor.Close();
                    Main.form.twofactor.Dispose();

                    Main.form.Close();
                    Main.form.Dispose();
                    MessageBox.Show("You have successfully logged in.");
                }
                catch (Exception ex)
                {
                    vault = null;
                    MessageBox.Show("Something went wrong, maybe wrong username or password?\nDescription: " + ex.Message, "Error", MessageBoxButtons.OK);
                }

            }
            else
            {
                MessageBox.Show("You are missing some information!", "You are missing some information!", MessageBoxButtons.OK);
            }
        }


        public class TwoFactorUI : Ui
        {
            public override string ProvideSecondFactorPassword(SecondFactorMethod method)
            {
                string code = "";
                var thread = new Thread(
                       () =>
                       {
                           Main.form.twofactor = new TwoFactor();
                           var result = Main.form.twofactor.ShowDialog();

                           if (result == DialogResult.OK)
                           {
                               code = Main.form.twofactor.authcode;
                           }
                           else
                           {
                               code = "";
                           }
                       });

                thread.Start();
                thread.Join();
                return code;
            }

            public override void AskToApproveOutOfBand(OutOfBandMethod method)
            {
                var thread = new Thread(
                    () =>
                    {
                        Main.form.twofactor = new TwoFactor();
                        Main.form.twofactor.showConfirmOnApp();
                        Main.form.twofactor.ShowDialog();
                    });
                thread.Start();
            }
        }

        private void setEnabled(bool enabled)
        {
            if(!enabled)
            {
                usernameTextField.Enabled = true;
                passwordTextField.Enabled = true;
                rememberUsernameCheckBox.Enabled = true;
                rememberPasswordCheckBox.Enabled = true;
            }
            else
            {
                usernameTextField.Text = "";
                usernameTextField.Enabled = false;
                passwordTextField.Text = "";
                passwordTextField.Enabled = false;
                rememberUsernameCheckBox.Checked = false;
                rememberUsernameCheckBox.Enabled = false;
                rememberPasswordCheckBox.Enabled = false;
                rememberPasswordCheckBox.Checked = false;
            }
        }

        private void materialCheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            var result = ((CheckBox)sender).Checked;
            setEnabled(result);
        }
    }
}
