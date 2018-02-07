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
    public class WebTutorialDialog: IDialog<bool>
    {
        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync("To host a website, you need to upload your project to github and share the repository to NYPSCloud. You can follow the instructions at https://blogs.msdn.microsoft.com/benjaminperkins/2017/04/04/setting-up-and-using-github-in-visual-studio-2017/ for more info on setting up github.");
            await context.PostAsync("Please access the NYPSCloud internal website at https://internal.nypscloud.net ");
            var reply = context.MakeMessage();
            var imagePath = HostingEnvironment.MapPath("~/image/authorize.png");

            var imageData = Convert.ToBase64String(File.ReadAllBytes(imagePath));


            var attach = new Attachment
            {
                Name = "authorize.png",
                ContentType = "image/png",
                ContentUrl = $"data:image/png;base64,{imageData}"
            };

            reply.Attachments = new List<Attachment> { attach};
            reply.Text = "Click on Authorize GitHub";
            await context.PostAsync(reply);
            PromptDialog.Confirm(context, StepTwo, "Please Confirm after you have complete the above step"
                , attempts: 0, promptStyle: PromptStyle.Auto);

        }
        public async Task StepTwo(IDialogContext context, IAwaitable<bool> result)
        {
            try
            {
                if (await result)
                {
                    await context.PostAsync("Login to your Github account.");
                    PromptDialog.Confirm(context, StepThree, "Please Confirm after you have complete the above step"
                        , attempts: 0, promptStyle: PromptStyle.Auto);
                }
                else
                {
                    await context.PostAsync("Please access the NYPSCloud internal website at https://internal.nypscloud.net ");
                    var reply = context.MakeMessage();
                    var imagePath = HostingEnvironment.MapPath("~/image/authorize.png");

                    var imageData = Convert.ToBase64String(File.ReadAllBytes(imagePath));


                    var attach = new Attachment
                    {
                        Name = "authorize.png",
                        ContentType = "image/png",
                        ContentUrl = $"data:image/png;base64,{imageData}"
                    };

                    reply.Attachments = new List<Attachment> { attach };
                    reply.Text = "Click on Authorize GitHub";
                    await context.PostAsync(reply);
                    PromptDialog.Confirm(context, StepTwo, "Please Confirm after you have complete the above step"
                        , attempts: 0, promptStyle: PromptStyle.Auto);
                }
            }
            catch
            {
                context.Done(false);
            }
        }
        public async Task StepThree(IDialogContext context, IAwaitable<bool> result)
        {
            try
            {
                if (await result)
                {
                    await context.PostAsync("Click on Create Website. Select your project and fill in the relevant information to host your website. Note: The url field is your website url.");
                    var reply = context.MakeMessage();
                    var imagePath = HostingEnvironment.MapPath("~/image/create.png");

                    var imageData = Convert.ToBase64String(File.ReadAllBytes(imagePath));


                    var attach = new Attachment
                    {
                        Name = "create.png",
                        ContentType = "image/png",
                        ContentUrl = $"data:image/png;base64,{imageData}"
                    };

                    reply.Attachments = new List<Attachment> { attach };
                    await context.PostAsync(reply);
                    PromptDialog.Confirm(context, StepFour, "Please Confirm after you have complete the above step"
                        , attempts: 0, promptStyle: PromptStyle.Auto);
                }
                else
                {
                    await context.PostAsync("Login to your Github account.");
                    PromptDialog.Confirm(context, StepThree, "Please Confirm after you have complete the above step"
                        , attempts: 0, promptStyle: PromptStyle.Auto);
                }
            }
            catch
            {
                context.Done(false);
            }
        }
        public async Task StepFour(IDialogContext context, IAwaitable<bool> result)
        {
            try
            {
                if (await result)
                {
                    await context.PostAsync("Wait for a while for your website to be hosted. You should be able to browse your website with the url you filled in.");
                    context.Done(true);
                }
                else
                {
                    await context.PostAsync("Click on Create Website. Select your project and fill in the relevant information to host your website. Note: The url field is your website url.");
                    var reply = context.MakeMessage();
                    var imagePath = HostingEnvironment.MapPath("~/image/create.png");

                    var imageData = Convert.ToBase64String(File.ReadAllBytes(imagePath));


                    var attach = new Attachment
                    {
                        Name = "create.png",
                        ContentType = "image/png",
                        ContentUrl = $"data:image/png;base64,{imageData}"
                    };

                    reply.Attachments = new List<Attachment> { attach };
                    await context.PostAsync(reply);
                    PromptDialog.Confirm(context, StepFour, "Please Confirm after you have complete the above step"
                        , attempts: 0, promptStyle: PromptStyle.Auto);
                }
            }
            catch
            {
                context.Done(false);
            }
        }
    }
}