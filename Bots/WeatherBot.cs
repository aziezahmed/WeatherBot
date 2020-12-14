using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Teams;
using Microsoft.Bot.Schema;

namespace WeatherBot.Bots
{
    public class WeatherBot : TeamsActivityHandler
    {
        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            turnContext.Activity.RemoveRecipientMention();

            var text = turnContext.Activity.Text.Trim().ToLower();

            var mention = new Mention
            {
                Mentioned = turnContext.Activity.From,
                Text = $"<at>{XmlConvert.EncodeName(turnContext.Activity.From.Name)}</at>"
            };

            var response = MessageFactory.Text($"Hi {mention.Text}, Here is your response");
            await turnContext.SendActivityAsync(response, cancellationToken);

            // Execute Base Code
            await base.OnMessageActivityAsync(turnContext, cancellationToken);
        }

        protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            var welcomeText = "Hello and welcome!";
            foreach (var member in membersAdded)
            {
                if (member.Id != turnContext.Activity.Recipient.Id)
                {
                    await turnContext.SendActivityAsync(MessageFactory.Text(welcomeText, welcomeText), cancellationToken);
                }
            }
        }
    }
}