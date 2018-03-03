using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Flexibility.Shared
{
    

    public partial class MenuPage : ContentPage
    {
        public ListView Menu
        {
            get
            {
                return listView;
            }
        }

        public MenuPage()
        {
            InitializeComponent();
        }
    }
}
