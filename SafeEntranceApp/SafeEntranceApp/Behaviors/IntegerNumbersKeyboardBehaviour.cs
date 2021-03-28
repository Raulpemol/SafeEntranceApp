using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using System.Linq;

namespace SafeEntranceApp.Behaviors
{
    class IntegerNumbersKeyboardBehavior : Behavior<Entry>
    {
        protected override void OnAttachedTo(Entry entry)
        {
            entry.TextChanged += OnEntryTextChanged;
            base.OnAttachedTo(entry);
        }

        protected override void OnDetachingFrom(Entry entry)
        {
            entry.TextChanged -= OnEntryTextChanged;
            base.OnDetachingFrom(entry);
        }

        private static void OnEntryTextChanged(object sender, TextChangedEventArgs args)
        {
            Entry entry = sender as Entry;

            if (!string.IsNullOrWhiteSpace(args.NewTextValue))
            {
                if(entry.Text.Length <= 12)
                {
                    bool isValid = args.NewTextValue.ToCharArray().All(x => char.IsDigit(x));
                    entry.Text = isValid ? args.NewTextValue : args.OldTextValue;
                }
                else
                {
                    entry.Text = args.OldTextValue;
                }
            }
        }
    }
}
