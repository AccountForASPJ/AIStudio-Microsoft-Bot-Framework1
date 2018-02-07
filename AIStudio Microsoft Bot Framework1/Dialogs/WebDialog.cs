using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;

namespace AIStudio_Microsoft_Bot_Framework1.Dialogs
{
    [Serializable]
    public class WebDialog : IDialog<Boolean>
    {
        public async Task StartAsync(IDialogContext context)
        {
            PromptDialog.Choice(
                context: context,
              resume: receivedProblem,
              options: new string[] { "1. How do I access internal website?",
                  "2. I don't have a GitHub account",
                  "3. How do I add a collaborator?"},
              prompt: "Which best describe your problem?",
              retry: "Please select the appropriate options",
              promptStyle: PromptStyle.Auto
                );
            //context.Wait(receivedProblem);
        }
        private async Task receivedProblem(IDialogContext context, IAwaitable<string> result)
        {
            try
            {
                var message = await result;

                switch (message[0])
                {
                    case '1':
                        // "1. How do I access internal website?",
                        await context.PostAsync("NYPSCloud internal website is accessible at https://internal.nypscloud.net. Please ensure that you are connected to the VPN and the certificates are installed in your computer.");
                        break;
                    case '2':
                        //"2. I don't have a GitHub account",
                        await context.PostAsync("You can follow the instructions at https://blogs.msdn.microsoft.com/benjaminperkins/2017/04/04/setting-up-and-using-github-in-visual-studio-2017/ for more info on setting up github repository.");
                        break;
                    case '3':
                        //"3. How do I add a collaborator?",
                        await context.PostAsync("Login to your GithHub -> Select the project -> Click on settings -> add collborator");
                        var reply = context.MakeMessage();
                        var imagePath = HostingEnvironment.MapPath("~/image/collab.png");

                        var imageData = Convert.ToBase64String(File.ReadAllBytes(imagePath));


                        var attach = new Attachment
                        {
                            Name = "collab.png",
                            ContentType = "image/png",
                            ContentUrl = $"data:image/png;base64,{imageData}"
                        };

                        reply.Attachments = new List<Attachment> { attach };
                        reply.Text = "Add collbaborator";
                        await context.PostAsync(reply);
                        break;

                    default:
                        await context.PostAsync("Sorry, I don't understand you.");
                        break;

                }
                context.Done(true);
            }
            catch
            {
                context.Done(false);
            }
        }
    }
}