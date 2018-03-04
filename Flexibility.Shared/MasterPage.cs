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
            Detail = new Xamarin.Forms.NavigationPage(new LoginPage()){
                BarBackgroundColor = Color.FromHex("#16222a"),
                BarTextColor = Color.FromHex("#F1F1F1")
            };
            (Detail as Xamarin.Forms.NavigationPage).On<Xamarin.Forms.PlatformConfiguration.iOS>().SetPrefersLargeTitles(true);

            menuPage.Menu.ItemSelected += OnItemSelected;
        }

        private void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as MasterPageItem;
            if (item != null)
            {
                Detail = new Xamarin.Forms.NavigationPage((Xamarin.Forms.Page)Activator.CreateInstance(item.TargetType)){
                    BarBackgroundColor = Color.FromHex("#16222a"),
                    BarTextColor = Color.FromHex("#ccc")
                };
                (Detail as Xamarin.Forms.NavigationPage).On<Xamarin.Forms.PlatformConfiguration.iOS>().SetPrefersLargeTitles(true);
                menuPage.Menu.SelectedItem = null;
                IsPresented = false;
            }
        }
    }
}

