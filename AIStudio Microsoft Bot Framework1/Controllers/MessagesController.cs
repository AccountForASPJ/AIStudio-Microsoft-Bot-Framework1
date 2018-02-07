using AIStudio_Microsoft_Bot_Framework1.Dialogs;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Web.Http;

namespace AIStudio_Microsoft_Bot_Framework1
{
    [BotAuthentication]
    public class MessagesController : System.Web.Http.ApiController
    {

        private static bool HasILILoaded = false;
        private static string CurrentConversationID = "0";

        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            try
            {
                ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));

                if (activity.Type == ActivityTypes.Message)
                {
                    if (AISAdapter_MBF.Gateway.Current != null)
                    {
                        //This must be processed inside a worker thread as it hands off to multiple internal engine threads which
                        // continue to process. If we dont do this, our async function is still running after we exit this method.                 
                        HostingEnvironment.QueueBackgroundWorkItem(async cancellationToken =>
                        {
                                //Each new input requires a GUID reference
                            await Task.Run(() => AISAdapter_MBF.Gateway.Current.ProcessTextInputAsync(connector, activity, Guid.NewGuid()));
                            await Conversation.SendAsync(activity, () => new HelpBotDialog());
                        });
                    }
                    else
                    {
                        Activity reply = activity.CreateReply("There is an error with the engine");
                        await connector.Conversations.ReplyToActivityAsync(reply);
                        
                    }

                }
                else if (activity.Type == ActivityTypes.ConversationUpdate)
                {
                    IConversationUpdateActivity update = activity;
                    var client = new ConnectorClient(new Uri(activity.ServiceUrl), new MicrosoftAppCredentials());
                    if (update.MembersAdded != null && update.MembersAdded.Any())
                    {
                        foreach (var newMember in update.MembersAdded)
                        {
                            if (newMember.Id != activity.Recipient.Id)
                            {
                                var reply = activity.CreateReply();
                                reply.Text = $"Hi, I am SCloud Bot. How may I help you?";
                                await client.Conversations.ReplyToActivityAsync(reply);
                            }
                        }
                    }

                    bool LoadEngine = false;
                    if (activity.Conversation.Id != MessagesController.CurrentConversationID)
                    {
                        MessagesController.CurrentConversationID = activity.Conversation.Id;
                        LoadEngine = true;
                    }
                    else
                    {
                        if (!MessagesController.HasILILoaded)
                            LoadEngine = true;
                    }

                    if (LoadEngine)
                    {
                        MessagesController.HasILILoaded = true;
                        string executionPath = System.Web.HttpContext.Current.Server.MapPath("~/App_Data/");
                        if (AISAdapter_MBF.Gateway.Current != null)
                        {
                            //This must be processed inside a worker thread as it hands off to multiple internal engine threads which
                            // continue to process. If we dont do this, our async function is still running after we exit this method.                 
                            HostingEnvironment.QueueBackgroundWorkItem(async cancellationToken =>
                            {

                                AISAdapter_MBF.Gateway.Current.EngineRuntimeError += async delegate (Exception e)
                                {
                                    Activity reply = activity.CreateReply("Engine Runtime Error =>" + e.Message);
                                    await connector.Conversations.ReplyToActivityAsync(reply);
                                };

                                AISAdapter_MBF.Gateway.Current.EngineInitiateError += async delegate (Exception e)
                                {
                                    Activity reply = activity.CreateReply("Engine Initiate Error =>" + e.Message);
                                    await connector.Conversations.ReplyToActivityAsync(reply);
                                };

                                AISAdapter_MBF.Gateway.Current.DetachedOutputRecieved += async delegate (string output, string activePersona)
                                {
                                    Activity reply = activity.CreateReply(output);
                                    reply.From.Name = activePersona;

                                    await connector.Conversations.ReplyToActivityAsync(reply);
                                };

                                AISAdapter_MBF.Gateway.Current.UnresolvedOutputRecieved += async delegate (string output, string activePersona)
                                {
                                    Activity reply = activity.CreateReply("iLi Engine Message =>" + output);
                                    reply.From.Name = activePersona;

                                    await connector.Conversations.ReplyToActivityAsync(reply);
                                };

                                //In this example, the engine will restart everytime the emulatore is restarted
                                await AISAdapter_MBF.Gateway.Current.SetFileStorePathAsync(executionPath, true).ConfigureAwait(false);

                            });
                        }
                    }
                }
                else
                {
                    HandleSystemMessage(activity);
                }

                return new HttpResponseMessage(HttpStatusCode.OK);

            }
            catch (Exception ex)
            {
                throw;
            }
        }


        private Activity HandleSystemMessage(Activity message)
        {
            if (message.Type == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == ActivityTypes.ConversationUpdate)
            {
                // Handle conversation state changes, like members being added and removed
                // Use Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info
                // Not available in all channels
                var reply = message.CreateReply();
                reply.Text = message.Text;
                return reply;
            }
            else if (message.Type == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
            }
            else if (message.Type == ActivityTypes.Typing)
            {
                // Handle knowing tha the user is typing
            }
            else if (message.Type == ActivityTypes.Ping)
            {
            }

            return null;
        }
    }
    [Serializable]
    public class EchoDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);
        }

        public async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var message = await argument;
            await context.PostAsync("You said: " + message.Text);
            context.Wait(MessageReceivedAsync);
        }
    }
    [LuisModel("9d3d1efa-a3ac-418d-8465-b6fffcf73e8b", "0387b25251954066b3b661101b139893", domain: "westus.api.cognitive.microsoft.com")]
    [Serializable]
    public class HelpBotDialog : LuisDialog<object>
    {
        [LuisIntent("Greetings")]
        public async Task Greetings(IDialogContext context, LuisResult result)
        {   
            await context.PostAsync("Hi, I am SCloud bot. How may I help you?");
        }

        [LuisIntent("Started")]
        public async Task Started(IDialogContext context, LuisResult result)
        {
            context.Call(new GettingStartedDialog(), HandOverTask);
        }
        [LuisIntent("Problem")]
        public async Task Problem(IDialogContext context, LuisResult result)
        {
            String msg = "";
            Debug.WriteLine(result.Entities.Count);
            foreach (EntityRecommendation er in result.Entities)
            {
                msg += er.Entity;
                Debug.WriteLine("er: "+ msg);
            }
            if (msg.Contains("vpn") && msg.Contains("website"))
            {
                await context.PostAsync("You are having problems with your vpn and website\n\nWe will try to solve your VPN problem first. ");
                context.Call(new VPNDialog(), SolveWebProblem);
                
            }
            else if (msg.Contains("vpn"))
            {
                await context.PostAsync("You are having problems with your vpn");
                context.Call(new VPNDialog(), HandOverTask);

            }
            else if (msg.Contains("website"))
            {
                await context.PostAsync("You are having problems with your website");
                context.Call(new WebDialog(), HandOverTask);
            }
            else
            {
                await context.PostAsync("What kind of problem are you facing?");
            }
        }
        [LuisIntent("Help")]
        public async Task Help(IDialogContext context, LuisResult result)
        {
            String msg = "";
            foreach (EntityRecommendation er in result.Entities)
            {
                msg += er.Entity;
                Debug.WriteLine("er: " + msg);
            }
            if (msg.Contains("vpn") && msg.Contains("website"))
            {
                await context.PostAsync("What kind of help do you need?");

            }
            else if (msg.Contains("vpn"))
            {
                await context.PostAsync("I will now guide you through connecting to VPN");
                context.Call(new VPNTutorialDialog(), HandOverTask);

            }
            else if (msg.Contains("website"))
            {
                await context.PostAsync("I will guide you through website.");
                context.Call(new WebTutorialDialog(), HandOverTask);
            }
            else
            {
                await context.PostAsync("What kind of help do you need?");
            }
        }
        [LuisIntent("Feature")]
        public async Task Feature(IDialogContext context, LuisResult result)
        {
            context.Call(new FeatureDialog(), HandOverTask);
        }
        private async Task HandOverTask(IDialogContext context, IAwaitable<Boolean> result)
        {
            bool succ = await result;
            if (succ)
            {
                await context.PostAsync("Hi, I am SCloud bot. How may I help you?");
            }
            else
            {
                await context.PostAsync("Can repeat what you said?");
            }
            
        }

        private async Task SolveWebProblem(IDialogContext context, IAwaitable<Boolean> result)
        {
            await context.PostAsync("You still have a problem with your Website.");
            context.Call(new WebDialog(), HandOverTask);
        }
        [LuisIntent("")]
        public async Task None(IDialogContext context, LuisResult result)
        {

            string message = "Sorry I didn't understand what you just said. What do you want me to do?";
            await context.PostAsync(message);
            context.Wait(MessageReceived);
        }
    }
    

}