using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace AIStudio_Microsoft_Bot_Framework1.Dialogs
{
    [Serializable]
    public class WebDialog : IDialog<Boolean>:
    {
        public async Task StartAsync(IDialogContext context)
        {
            PromptDialog.Choice(
                context: context,
              resume: receivedProblem,
              options: new string[] { "1. I don't have the VPN cert.",
                  "5. I can't access internal website"},
              prompt: "Which best describe your problem?",
              retry: "Please select the appropriate options",
              promptStyle: PromptStyle.Auto
                );
            //context.Wait(receivedProblem);
        }
        private async Task receivedProblem(IDialogContext context, IAwaitable<string> result)
        {
            context.Done(true);
        }
    }
}