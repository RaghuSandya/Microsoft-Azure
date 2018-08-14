using Microsoft.Azure;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using System;
using System.Threading.Tasks;

namespace AdaptiveSpeak.Models
{
    [Serializable]
    public class Members
    {
        [Prompt("May I know your good name?")]
        public string Name { get; set; }

        [Prompt("What is your contact number?")]
        [Numeric(6000000000, 9999999999)]
        public double PhoneNumber { get; set; }

        [Prompt("What is your address?")]
        public string Address { get; set; }

        public static IForm<Members> BuildForm() => new FormBuilder<Members>()
            .Message("Wipro voice based Registration")
            .Field(nameof(Name))
            .Field(nameof(PhoneNumber))
            .Field(nameof(Address))
            .Confirm(async (state) =>
            {
                return new PromptAttribute("Hi {Name}, Please review your selection. Phone Number: {PhoneNumber}, Address: {Address}. Do you want to continue? {||}");
            })
            .OnCompletion(saveDetails)
            .Build();

        private static Task saveDetails(IDialogContext context, Members state)
        {
            context.SayAsync(text: state.Name, speak: state.Name);
            return Task.CompletedTask;
        }
    }
}