using System.ComponentModel;

namespace Xamarin.Forms.Media
{
    public class ItemPropertyChangedEventArgs : PropertyChangedEventArgs
    {
        object item;

        public ItemPropertyChangedEventArgs(object item, string propertyName) : base(propertyName)
        {
            this.item = item;
        }

        public object Item
        {
            get { return item; }
        }
    }
}
