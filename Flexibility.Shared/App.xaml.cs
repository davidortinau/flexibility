using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Flexibility.Shared;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Flexibility
{
	public partial class App : Application
	{
		public App()
		{
			InitializeComponent();

			MainPage = new MasterPage();

			//MainPage = new LoginPage();
			//MainPage = new PhotosPage();
			//MainPage = new ProfilePage();
			//MainPage = new FoodPage();
		}

		protected override void OnStart()
		{
			// Handle when your app starts
		}

		protected override void OnSleep()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume()
		{
			// Handle when your app resumes
		}
	}
}
