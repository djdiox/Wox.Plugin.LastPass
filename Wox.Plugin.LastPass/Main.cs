using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Controls;
using System.Windows.Forms;
using MaterialSkin.Controls;
using Microsoft.VisualBasic;
using Wox.Plugin.OnePassword;
using Wox.Plugin.OnePassword.Models;

namespace Wox.Plugin.OnePassword
{
    public class Main : IPlugin
    {
        public static Authenticate form = new Authenticate();

        public void Init(PluginInitContext context)
        {
            form.vault = null;
        }

        public List<Result> Query(Query query)
        {
            if (form.vault == null)
            {
                List<Result> results = new List<Result>();
                results.Add(new Result()
                {
                    Title = "Login to 1Password",
                    SubTitle = "Press Enter to proceed",
                    IcoPath = "Images\\gray.png",
                    Action = e =>
                    {
                        form.Show();
                        return false;
                    }
                });
                return results;
            }
            else
            {
                List<Result> results = new List<Result>();
                for (var i = 0; i < form.vault.Accounts.Count; ++i)
                {
                    var account = form.vault.Accounts[i];
                    if (account.Title.Contains(query.Search) || account.AdditionalInformation.Contains(query.Search) || account.Urls[0].Href.Contains(query.Search))
                    {
                        results.Add(new Result()
                        {
                            Title = account.Title + " - [" + account.AdditionalInformation + "]",
                            SubTitle = account.Urls[0].Href.Replace(query.Search, "[" + query.Search + "]"),
                            IcoPath = "Images\\red.png",
                            Action = e =>
                            {
                                if (e.SpecialKeyState.CtrlPressed)
                                {
                                    Clipboard.SetText(account.AdditionalInformation);
                                }
                                else if (e.SpecialKeyState.ShiftPressed)
                                {
                                    Clipboard.SetText(account.Urls[0].Href);
                                } else
                                {
                                    // TODO Fix me
                                    Clipboard.SetText(account.AdditionalInformation);
                                }
                                return true;
                            }
                        });
                    }
                }
                return results;
            }
        }
    }


}
