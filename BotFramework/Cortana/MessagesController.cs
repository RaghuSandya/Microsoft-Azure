using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using AdaptiveSpeak.Models;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Connector;

namespace AdaptiveSpeak
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        internal static IDialog<Members> MakeRootDialog()
        {
            return Chain.From(() => FormDialog.FromForm(Members.BuildForm));
        }

        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            if (activity.Type == ActivityTypes.Message)
            {
                await Conversation.SendAsync(activity, MakeRootDialog);
            }
            else if (activity.Type == ActivityTypes.ConversationUpdate)
            {
                IConversationUpdateActivity iConversationUpdated = activity as IConversationUpdateActivity;
                if (iConversationUpdated != null)
                {
                    ConnectorClient connector = new ConnectorClient(new System.Uri(activity.ServiceUrl));

                    foreach (var member in iConversationUpdated.MembersAdded ?? System.Array.Empty<ChannelAccount>())
                    {
                        if (member.Id == iConversationUpdated.Recipient.Id)
                        {
                            var reply = ((Activity)iConversationUpdated).CreateReply();
                            reply.Speak = "Welcome to wipro voice based application";
                            reply.Text = "Welcome to wipro voice based application";
                            await connector.Conversations.ReplyToActivityAsync(reply);
                        }
                    }
                }
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }
    }
}