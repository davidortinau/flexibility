using System;
using Utils;
using Xamarin.Forms;

namespace Flexibility.Shared
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        void Handle_Clicked(object sender, System.EventArgs e)
        {
            if(string.IsNullOrEmpty(UserNameEntry.Text) || UserNameEntry.Text.Length < 5)
                VisualStateManager.GoToState(UserNameEntry, "Invalid");

            if (string.IsNullOrEmpty(PasswordEntry.Text) || PasswordEntry.Text.Length < 5)
                VisualStateManager.GoToState(PasswordEntry, "Invalid");

            DisplayAlert("Welcome to Visual State Manager", "", "Thanks!");
        }

        void Handle_TextChanged(object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            var strength = PasswordAdvisor.CheckStrength(e.NewTextValue);
            var strengthName = Enum.GetName(typeof(PasswordScore), strength);
            VisualStateManager.GoToState(StrengthIndicator, strengthName);
        }
    }
}
