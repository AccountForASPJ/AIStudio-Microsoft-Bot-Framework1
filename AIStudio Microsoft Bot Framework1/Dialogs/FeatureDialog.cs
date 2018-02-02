using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace AIStudio_Microsoft_Bot_Framework1.Dialogs
{
    [Serializable]
    public class FeatureDialog: IDialog<Boolean>
    {
        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync("NYP Secured Cloud, aka NYPScloud, is a platform to give NYP students a chance to host and learn how to manage a website. You can host your school project on NYPScloud for free. ");
            PromptDialog.Choice(
                context: context,
              resume: Introduce,
              options: new string[] { "1. Secure VPN connection to internal network",
                  "2. External Website",
                  "3. Internal Website",
                  "4. NYPScloud architecture",
                  "5. Security Features"},
              prompt: "These are the overview of the features we offer. Click to learn more. ",
              retry: "Please select the appropriate options",
              promptStyle: PromptStyle.Auto
                );
            //context.Wait(receivedProblem);
        }
        private async Task Introduce(IDialogContext context, IAwaitable<string> result)
        {
            var message = await result;
            Boolean quit = false;
            switch (message[0])
            {
                case '1':
                    //"1. Secure VPN connection to internal network",
                    await context.PostAsync("NYPScloud has a internal network where your website are hosted at. You are required to connect to the internal network via OpenVPN to access to the internal network resources. This is to provide more security.");
                    break;
                case '2':
                    //"2. External Website",
                    await context.PostAsync("NYPScloud external website allows you to request the VPN certificate needed to connect to the internal network. You can also monitor your website here.\n\n Go to https://www.nypscloud.net to access our external website.");
                    break;
                case '3':
                    //"3. Internal Website",
                    await context.PostAsync("NYPScloud internal website allows you to create and configure your website. You need to establish a VPN connection to our internal network before you can access our internal network");
                    break;
                case '4':
                    //"4. NYPScloud architecture",
                    await context.PostAsync("NYPScloud consist of a internal cloud network, a external VPN server and a external website. Here is our network topology: ");
                    break;
                
                default:
                    await context.PostAsync("Sorry, I don't understand you.");
                    quit = true;
                    break;

            }
            if (quit)
            {
                context.Done(true);
            }
            else
            {
                PromptDialog.Choice(
                    context: context,
                    resume: Introduce,
                    options: new string[] { "1. Secure VPN connection to internal network",
                        "2. External Website",
                        "3. Internal Website",
                        "4. NYPScloud architecture",
                        "5. Security Features"},
                    prompt: "These are the overview of the features we offer. Click to learn more. ",
                    retry: "Please select the appropriate options",
                    promptStyle: PromptStyle.Auto
                );
            }
            
        }
    }
}