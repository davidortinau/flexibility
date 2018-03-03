using System;
using Flexibility.Shared.Models;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace Flexibility.Shared
{
    public class MasterPage : MasterDetailPage
    {
        MenuPage menuPage;

        public MasterPage()
        {
            menuPage = new MenuPage();
            Master = menuPage;
            Detail = new Xamarin.Forms.NavigationPage(new LoginPage());
            (Detail as Xamarin.Forms.NavigationPage).On<Xamarin.Forms.PlatformConfiguration.iOS>().SetPrefersLargeTitles(true);

            menuPage.Menu.ItemSelected += OnItemSelected;
        }

        private void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as MasterPageItem;
            if (item != null)
            {
                Detail = new Xamarin.Forms.NavigationPage((Xamarin.Forms.Page)Activator.CreateInstance(item.TargetType));
                (Detail as Xamarin.Forms.NavigationPage).On<Xamarin.Forms.PlatformConfiguration.iOS>().SetPrefersLargeTitles(true);
                menuPage.Menu.SelectedItem = null;
                IsPresented = false;
            }
        }
    }
}

