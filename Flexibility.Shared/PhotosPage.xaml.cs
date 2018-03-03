using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Monkeys.ViewModels;
using System.IO;

namespace Flexibility.Shared
{
    public partial class PhotosPage : ContentPage
    {
        MonkeysViewModel vm;

        public PhotosPage()
        {
            InitializeComponent();
            BindingContext = vm = new MonkeysViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

    }
}
