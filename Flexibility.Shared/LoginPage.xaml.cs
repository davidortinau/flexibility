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
        }
    }
}
