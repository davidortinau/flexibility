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

		protected override void OnAppearing()
		{
			base.OnAppearing();

			var stateGroup = new VisualStateGroup
			{
				Name = "StrengthStates",
				TargetType = typeof(Label)
			};

			stateGroup.States.Add(CreateState("Blank", "", Color.White));
			stateGroup.States.Add(CreateState("VeryWeak", "\uf023", Color.Red));
			stateGroup.States.Add(CreateState("Weak", "\uf023 \uf023", Color.Orange));
			stateGroup.States.Add(CreateState("Medium", "\uf023 \uf023 \uf023", Color.Yellow));
			stateGroup.States.Add(CreateState("String", "\uf023 \uf023 \uf023 \uf023", Color.Green));
			stateGroup.States.Add(CreateState("VeryStrong", "\uf023 \uf023 \uf023 \uf023 \uf023", Color.Green));

			VisualStateManager.SetVisualStateGroups(this.StrengthIndicator, new VisualStateGroupList { stateGroup });
		}

		static VisualState CreateState(string strength, string text, Color color)
		{
			var textSetter = new Setter { Value = text, Property = Label.TextProperty };
			var colorSetter = new Setter { Value = color, Property = Label.TextColorProperty };

			return new VisualState
			{
				Name = strength,
				TargetType = typeof(Label),
				Setters = { textSetter, colorSetter }
			};
		}

		void Handle_Clicked(object sender, System.EventArgs e)
		{
			var isValid = true;

			if (string.IsNullOrEmpty(UserNameEntry.Text) || UserNameEntry.Text.Length < 5)
			{
				VisualStateManager.GoToState(UserNameEntry, "Invalid");
				isValid = false;
			}

			if (string.IsNullOrEmpty(PasswordEntry.Text) || PasswordEntry.Text.Length < 5)
			{
				VisualStateManager.GoToState(PasswordEntry, "Invalid");
				isValid = false;
			}

			if (isValid)
				DisplayAlert("Welcome to Visual State Manager", "", "Thanks!");
		}

		void Handle_TextChanged(object sender, Xamarin.Forms.TextChangedEventArgs e)
		{
			var strength = PasswordAdvisor.CheckStrength(e.NewTextValue);
			var strengthName = Enum.GetName(typeof(PasswordScore), strength);
			VisualStateManager.GoToState(this.StrengthIndicator, strengthName);
		}

		private string strength;

		public string Strength
		{
			get => strength;
			set
			{
				strength = value;
				OnPropertyChanged(nameof(Strength));
			}
		}
	}
}
