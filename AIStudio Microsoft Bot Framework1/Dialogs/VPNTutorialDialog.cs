using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;

namespace AIStudio_Microsoft_Bot_Framework1.Dialogs
{
    [Serializable]
    public class VPNTutorialDialog : IDialog<bool>
    {
        public async Task StartAsync(IDialogContext context)
        {
            
            await context.PostAsync("Please download and install OpenVPN from https://openvpn.net/index.php/open-source/downloads.html. \n\n" +
                            "For Windows computer, you may follow the instruction at https://openvpn.net/index.php/access-server/docs/admin-guides-sp-859543150/howto-connect-client-configuration/395-how-to-install-the-openvpn-client-on-windows.html. \n");
            PromptDialog.Confirm(context, StepTwo, "Please Confirm after you have complete the above step"
                , attempts: 1, promptStyle: PromptStyle.Auto);

        }
        public async Task StepTwo(IDialogContext context, IAwaitable<bool> result)
        {
            if (await result)
            {
                await context.PostAsync("Please download your OpenVPN certificate from the external website or you can download it at https://www.nypscloud.net/Home/GenerateKey");
                PromptDialog.Confirm(context, StepThree, "Please Confirm after you have complete the above step"
                    , attempts: 1, promptStyle: PromptStyle.Auto);


            }
            else
            {
                await context.PostAsync("Please download and install OpenVPN from https://openvpn.net/index.php/open-source/downloads.html. \n" +
                            "For Windows computer, you may follow the instruction at https://openvpn.net/index.php/access-server/docs/admin-guides-sp-859543150/howto-connect-client-configuration/395-how-to-install-the-openvpn-client-on-windows.html. \n");
                PromptDialog.Confirm(context, StepTwo, "Please Confirm after you have complete the above step"
                    , attempts: 1, promptStyle: PromptStyle.Auto);
            }
        }
        public async Task StepThree(IDialogContext context, IAwaitable<bool> result)
        {
            if (await result)
            {
                await context.PostAsync("Extract all the config files to <OPENVPN_INSTALLATION_FOLDER>/config/ directory\n\n" +
                            "   For Windows computer, the default OpenVPN installation folder is at C:\\Program Files\\OpenVPN \n");
                PromptDialog.Confirm(context, StepFour, "Please Confirm after you have complete the above step"
                    , attempts: 1, promptStyle: PromptStyle.Auto);

            }
            else
            {
                await context.PostAsync("Please download your OpenVPN certificate from the external website or you can download it at https://www.nypscloud.net/Home/GenerateKey");
                PromptDialog.Confirm(context, StepThree, "Please Confirm after you have complete the above step"
                    , attempts: 1, promptStyle: PromptStyle.Auto);
            }
        }
        public async Task StepFour(IDialogContext context, IAwaitable<bool> result)
        {
            if (await result)
            {
                await context.PostAsync("Open a command prompt, press Win key -> Search for cmd -> Run command prompt, run the following command: \n\n        cd <OPENVPN_INSTALLATION_FOLDER>/bin/\n\n        openvpn ../config/client.ovpn");
                PromptDialog.Confirm(context, StepFive, "Please Confirm after you have complete the above step"
                    , attempts: 1, promptStyle: PromptStyle.Auto);

            }
            else
            {
                await context.PostAsync("Copy/move the VPN cert to <OPENVPN_INSTALLATION_FOLDER>/config/ directory\n\n" +
                            "   For Windows computer, the default OpenVPN installation folder is at C:\\Program Files\\OpenVPN \n");
                PromptDialog.Confirm(context, StepFour, "Please Confirm after you have complete the above step"
                    , attempts: 1, promptStyle: PromptStyle.Auto);
            }
        }
        public async Task StepFive(IDialogContext context, IAwaitable<bool> result)
        {
            if (await result)
            {
                await context.PostAsync("After the VPN finish connecting, you should see \"Initialization Sequence Completed\" message at the end of the output.");
                PromptDialog.Confirm(context, StepSix, "Did you see the output?"
                    , attempts: 1, promptStyle: PromptStyle.Auto);

            }
            else
            {
                await context.PostAsync("Open a command prompt, press Win key -> Search for cmd -> Run command prompt, run the following command: \n\n        cd <OPENVPN_INSTALLATION_FOLDER>\n\n        bin\\openvpn ..\\config\\client.ovpn");
                PromptDialog.Confirm(context, StepFive, "Please Confirm after you have complete the above step"
                    , attempts: 1, promptStyle: PromptStyle.Auto);
            }
        }
        public async Task StepSix(IDialogContext context, IAwaitable<bool> result)
        {
            if (await result)
            {
                await context.PostAsync("You have successfully connected to the VPN. Try accessing the internal website");
                context.Done(true);

            }
            else
            {
                await context.PostAsync("Try requesting another VPN certificate. If the problem persist, contact the administrator.");
                PromptDialog.Confirm(context, StepSeven, "Do you need me to guide you through requesting the VPN certificate again?"
                    , attempts: 1, promptStyle: PromptStyle.Auto);
            }
        }
        public async Task StepSeven(IDialogContext context, IAwaitable<bool> result)
        {
            if (await result)
            {
                await context.PostAsync("Ok sure! \n\nPlease requested a OpenVPN certificate from the external website");
                PromptDialog.Confirm(context, StepThree, "Please Confirm after you have complete the above step"
                    , attempts: 1, promptStyle: PromptStyle.Auto);
            }
            else
            {
                context.Done(false);
            }
        }
    }
}