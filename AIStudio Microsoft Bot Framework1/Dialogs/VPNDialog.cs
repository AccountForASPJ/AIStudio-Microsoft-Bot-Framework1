using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace AIStudio_Microsoft_Bot_Framework1.Dialogs
{
    [Serializable]
    public class VPNDialog : IDialog<Boolean>
    {
        public async Task StartAsync(IDialogContext context)
        {
            
            PromptDialog.Choice(
                context: context,
              resume: receivedProblem,
              options: new string[] { "1. I don't have the VPN cert.",
                  "2. I don't know how to connect VPN",
                  "3. I don't know how to use OpenVPN.",
                  "4. I don't know how to install OpenVPN.",
                  "5. I can't access internal website"},
              prompt: "Which best describe your problem?",
              retry: "Please select the appropriate options",
              promptStyle: PromptStyle.Auto
                );
            //context.Wait(receivedProblem);
        }
        private async Task receivedProblem(IDialogContext context, IAwaitable<string> result)
        {
            var message = await result;

            switch (message[0])
            {
                case '1':
                    //"1. I don't have the VPN cert/I lost the VPN cert."
                    await context.PostAsync("Please request a VPN cert at the NYPSCloud external website.\n\n Navigate to Home -> Manage VPN Key.\n On the main pane, click request");
                    break;
                case '2':
                    //"2. I don't know how to connect VPN",
                    await context.PostAsync("1. Please download and install OpenVPN from https://openvpn.net/index.php/open-source/downloads.html. \n"+
                            "For Windows computer, you may follow the instruction at https://openvpn.net/index.php/access-server/docs/admin-guides-sp-859543150/howto-connect-client-configuration/395-how-to-install-the-openvpn-client-on-windows.html. \n" +
                            "2. After installation, Please request a VPN cert at the NYPSCloud external website.\n Navigate to Home -> Manage VPN Key.\n On the main pane, click request. \n"+
                            "3. Copy/move the VPN cert to <OPENVPN_INSTALLATION_FOLDER>/config/ directory\n" +
                            "   For Windows computer, the default OpenVPN installation folder is at C:\\Program Files\\OpenVPN \n"+
                            "4. Open a command prompt, run the following command: \n\n        cd <OPENVPN_INSTALLATION_FOLDER>/bin/\n\n        openvpn ../config/client.ovpn");
                    break;
                case '3':
                    //"3. I don't know how to use OpenVPN.",
                    await context.PostAsync("1. Copy/move the VPN cert to <OPENVPN_INSTALLATION_FOLDER>/config/directory\n" +
                            "2. For Windows computer, the default OpenVPN installation folder is at C:\\Program Files\\OpenVPN \n"+
                            "3. Open a command prompt, run the following command: \n\n        cd <OPENVPN_INSTALLATION_FOLDER>/bin/\n\n        openvpn ../config/client.ovpn");
                    break;
                case '4':
                    //"4. I don't know how to install OpenVPN."
                    await context.PostAsync("1. Please download OpenVPN installer from https://openvpn.net/index.php/open-source/downloads.html. \n" +
                            "2. For Windows computer, you may follow the instruction at https://openvpn.net/index.php/access-server/docs/admin-guides-sp-859543150/howto-connect-client-configuration/395-how-to-install-the-openvpn-client-on-windows.html. \n");
                    break;
                case '5':
                    //"5. I can't access internal website"
                    await context.PostAsync("Please try reconnecting to the VPN. If the problem persist contact the administrator.");
                    break;
                case '6':
                    await context.PostAsync("Try contacting the administrator.");
                    break;
                default:
                    await context.PostAsync("Sorry, I don't understand you.");
                    break;

            }
            context.Done(true);
    
        }
    }
}