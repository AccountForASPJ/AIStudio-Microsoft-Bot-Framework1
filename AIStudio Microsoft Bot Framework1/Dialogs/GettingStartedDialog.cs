using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;

namespace AIStudio_Microsoft_Bot_Framework1.Dialogs
{
    [Serializable]
    public class GettingStartedDialog : IDialog<Boolean>
    {
        public async Task StartAsync(IDialogContext context)
        {
              
            await context.PostAsync("I will now help you get started with NYPSCloud. There are 2 types of installation: ");
            PromptDialog.Choice(
                context: context,
              resume: Installation,
              options: new string[] { "1. Automatic Installation",
                  "2. Manual Installation",},
              prompt: "Please choose your installation type",
              retry: "Please select the appropriate options",
              attempts: 0,
              promptStyle: PromptStyle.Auto
                );
            
        }
        private async Task Installation(IDialogContext context, IAwaitable<string> result)
        {
            
            try {
                var message = await result;
                Debug.WriteLine("HI");
                switch (message[0])
                {
                    case '1':
                        await context.PostAsync("You have selected Automatic Installation");
                        await context.PostAsync("Please download the installer at https://www.nypscloud.net/Home/AutoInstall. It will guide you through the installation and help you connect to the VPN. You should see a 'Initialization Sequence Completed' message");
                        context.Done(true);
                        break;
                    case '2':
                        await context.PostAsync("You have selected Manual Installation");
                        PromptDialog.Confirm(context, Manual, "Have you installed OpenVPN on your computer?"
                    , attempts: 1, promptStyle: PromptStyle.Auto);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception e)
            {
                context.Done(false);
            }
        }
        private async Task HandOverTask(IDialogContext context, IAwaitable<bool> result)
        {
            context.Done(await result);
        }
        private async Task Manual(IDialogContext context, IAwaitable<bool> result)
        {
            try
            {
                if (await result)
                {
                    await context.PostAsync("Please download the certificates from  https://www.nypscloud.net/Home/GenerateKey");
                    PromptDialog.Confirm(context, InstallCert, "Please Confirm after you have complete the above step"
                , attempts: 1, promptStyle: PromptStyle.Auto);
                }
                else
                {
                    context.Call(new VPNTutorialDialog(), HandOverTask);
                }
            }
            catch
            {
                context.Done(false);
            }
        }
        private async Task InstallCert(IDialogContext context, IAwaitable<bool> result)
        {
            try
            {
                if (await result)
                {
                    await context.PostAsync(@"Please extract the all files inside the downloaded zip file into 'C:\Program Files\OpenVPN\config' directory");
                    PromptDialog.Confirm(context, InstallCert2, "Please Confirm after you have complete the above step"
                        , attempts: 1, promptStyle: PromptStyle.Auto);

                }
                else
                {
                    await context.PostAsync("Please download the certificates from  https://www.nypscloud.net/Home/GenerateKey");
                    PromptDialog.Confirm(context, InstallCert, "Please Confirm after you have complete the above step"
                , attempts: 1, promptStyle: PromptStyle.Auto);
                }
            }
            catch
            {
                context.Done(false);
            }
        }
        private async Task InstallCert2(IDialogContext context, IAwaitable<bool> result)
        {
            try {
                if (await result)
                {
                    await context.PostAsync("You will now need to install the NYPSCloud Certificate Authority and a personal Certificate issued by NYPScloud");
                    await context.PostAsync(@"To install NYPSCloud Certificate Authority, Right click the 'ca.crt' in 'C:\Program Files\OpenVPN\config' directory -> Select Install Certificate.");

                    var reply = context.MakeMessage();
                    var imagePath = HostingEnvironment.MapPath("~/image/wizard1.png");
                    var imagePath2 = HostingEnvironment.MapPath("~/image/wizard2.png");
                    var imagePath3 = HostingEnvironment.MapPath("~/image/wizard3.png");

                    var imageData = Convert.ToBase64String(File.ReadAllBytes(imagePath));
                    var imageData2 = Convert.ToBase64String(File.ReadAllBytes(imagePath2));
                    var imageData3 = Convert.ToBase64String(File.ReadAllBytes(imagePath3));

                    var attach = new Attachment
                    {
                        Name = "wizard1.png",
                        ContentType = "image/png",
                        ContentUrl = $"data:image/png;base64,{imageData}"
                    };
                    var attach2 = new Attachment
                    {
                        Name = "wizard2.png",
                        ContentType = "image/png",
                        ContentUrl = $"data:image/png;base64,{imageData2}"
                    };
                    var attach3 = new Attachment
                    {
                        Name = "wizard3.png",
                        ContentType = "image/png",
                        ContentUrl = $"data:image/png;base64,{imageData3}"
                    };
                    reply.Attachments = new List<Attachment> { attach, attach2, attach3 };
                    reply.Text = "Follow this wizard to install the CA cert.";
                    await context.PostAsync(reply);
                    PromptDialog.Confirm(context, InstallCert3, "Please Confirm after you have complete the above step"
                        , attempts: 1, promptStyle: PromptStyle.Auto);

                }
                else
                {
                    await context.PostAsync(@"Please extract the all files inside the downloaded zip file into 'C:\Program Files\OpenVPN\config' directory");
                    PromptDialog.Confirm(context, InstallCert2, "Please Confirm after you have complete the above step"
                        , attempts: 1, promptStyle: PromptStyle.Auto);
                }
            }
            catch
            {
                context.Done(false);
            }
        }
        private async Task InstallCert3(IDialogContext context, IAwaitable<bool> result)
        {
            try
            {
                if (await result)
                {
                    await context.PostAsync(@"To install your personal certificate, Right click '153741J.p12' in 'C:\Program Files\OpenVPN\config' directory -> Select Install PFX.");

                    var reply = context.MakeMessage();
                    var imagePath = HostingEnvironment.MapPath("~/image/wizard1.png");
                    var imagePath2 = HostingEnvironment.MapPath("~/image/personal1.png");

                    var imagePath3 = HostingEnvironment.MapPath("~/image/personal_pass.png");

                    var imagePath4 = HostingEnvironment.MapPath("~/image/personal3.png");
                    var imagePath5 = HostingEnvironment.MapPath("~/image/personal4.png");

                    var imageData = Convert.ToBase64String(File.ReadAllBytes(imagePath));
                    var imageData2 = Convert.ToBase64String(File.ReadAllBytes(imagePath2));
                    var imageData3 = Convert.ToBase64String(File.ReadAllBytes(imagePath3));
                    var imageData4 = Convert.ToBase64String(File.ReadAllBytes(imagePath4));
                    var imageData5 = Convert.ToBase64String(File.ReadAllBytes(imagePath5));

                    var attach = new Attachment
                    {
                        Name = "wizard1.png",
                        ContentType = "image/png",
                        ContentUrl = $"data:image/png;base64,{imageData}"
                    };
                    var attach2 = new Attachment
                    {
                        Name = "wizard2.png",
                        ContentType = "image/png",
                        ContentUrl = $"data:image/png;base64,{imageData2}"
                    };

                    reply.Attachments = new List<Attachment> { attach, attach2 };
                    reply.Text = "Follow this wizard to install the your personal cert.";
                    await context.PostAsync(reply);
                    var attach3 = new Attachment
                    {
                        Name = "wizard3.png",
                        ContentType = "image/png",
                        ContentUrl = $"data:image/png;base64,{imageData3}"
                    };
                    var attach4 = new Attachment
                    {
                        Name = "wizard4.png",
                        ContentType = "image/png",
                        ContentUrl = $"data:image/png;base64,{imageData4}"
                    };
                    var attach5 = new Attachment
                    {
                        Name = "wizard5.png",
                        ContentType = "image/png",
                        ContentUrl = $"data:image/png;base64,{imageData5}"
                    };
                    reply = context.MakeMessage();

                    reply.Attachments = new List<Attachment> { attach3, attach4, attach5 };
                    reply.Text = "The password is store at <adminNo>_pass.txt";
                    await context.PostAsync(reply);
                    PromptDialog.Confirm(context, InstallCert4, "Please Confirm after you have complete the above step"
                        , attempts: 1, promptStyle: PromptStyle.Auto);

                }
                else
                {
                    await context.PostAsync(@"Right click the 'ca.crt' in 'C:\Program Files\OpenVPN\config' directory -> Select Install Certificate.");

                    var reply = context.MakeMessage();
                    var imagePath = HostingEnvironment.MapPath("~/image/wizard1.png");
                    var imagePath2 = HostingEnvironment.MapPath("~/image/wizard2.png");
                    var imagePath3 = HostingEnvironment.MapPath("~/image/wizard3.png");

                    var imageData = Convert.ToBase64String(File.ReadAllBytes(imagePath));
                    var imageData2 = Convert.ToBase64String(File.ReadAllBytes(imagePath2));
                    var imageData3 = Convert.ToBase64String(File.ReadAllBytes(imagePath3));

                    var attach = new Attachment
                    {
                        Name = "wizard1.png",
                        ContentType = "image/png",
                        ContentUrl = $"data:image/png;base64,{imageData}"
                    };
                    var attach2 = new Attachment
                    {
                        Name = "wizard2.png",
                        ContentType = "image/png",
                        ContentUrl = $"data:image/png;base64,{imageData2}"
                    };
                    var attach3 = new Attachment
                    {
                        Name = "wizard3.png",
                        ContentType = "image/png",
                        ContentUrl = $"data:image/png;base64,{imageData3}"
                    };
                    reply.Attachments = new List<Attachment> { attach, attach2, attach3 };
                    reply.Text = "Follow this wizard to install the CA cert.";
                    await context.PostAsync(reply);
                    PromptDialog.Confirm(context, InstallCert3, "Please Confirm after you have complete the above step"
                        , attempts: 1, promptStyle: PromptStyle.Auto);
                }
            }
            catch
            {
                context.Done(false);
            }
            
        }
        private async Task InstallCert4(IDialogContext context, IAwaitable<bool> result)
        {
            try
            {
                if (await result)
                {
                    await context.PostAsync("Open a command prompt, press Win key -> Search for cmd -> Run command prompt, run the following command: \n\n        cd <OPENVPN_INSTALLATION_FOLDER>\n\n        bin\\openvpn.exe ..\\config\\client.ovpn");
                    PromptDialog.Confirm(context, InstallCert5, "Please Confirm after you have complete the above step"
                        , attempts: 1, promptStyle: PromptStyle.Auto);

                }
                else
                {
                    await context.PostAsync("Please ensure you follow the wizard properly.");
                    PromptDialog.Confirm(context, InstallCert4, "Please Confirm after you have complete the above step"
                , attempts: 1, promptStyle: PromptStyle.Auto);
                }
            }
            catch
            {
                context.Done(false);
            }
            
        }
        public async Task InstallCert5(IDialogContext context, IAwaitable<bool> result)
        {
            try
            {
                if (await result)
                {
                    await context.PostAsync("After the VPN finish connecting, you should see \"Initialization Sequence Completed\" message at the end of the output.");
                    PromptDialog.Confirm(context, InstallCert6, "Did you see the output?"
                        , attempts: 1, promptStyle: PromptStyle.Auto);

                }
                else
                {
                    await context.PostAsync("Open a command prompt, press Win key -> Search for cmd -> Run command prompt, run the following command: \n\n        cd <OPENVPN_INSTALLATION_FOLDER>\n\n        bin\\openvpn.exe ..\\config\\client.ovpn");
                    PromptDialog.Confirm(context, InstallCert5, "Please Confirm after you have complete the above step"
                        , attempts: 1, promptStyle: PromptStyle.Auto);
                }
            }
            catch
            {
                context.Done(false);
            }
            
        }
        public async Task InstallCert6(IDialogContext context, IAwaitable<bool> result)
        {
            try
            {
                if (await result)
                {
                    await context.PostAsync("You have successfully connected to the VPN. Try accessing the internal website");
                    context.Done(true);

                }
                else
                {
                    await context.PostAsync("Please contact the administrator.");
                    context.Done(false);

                }
            }
            catch
            {
                context.Done(false);
            }
            
        }
    }
}