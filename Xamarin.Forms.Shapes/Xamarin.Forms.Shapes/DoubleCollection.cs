using System.Collections.ObjectModel;

namespace Xamarin.Forms.Media
{
    [TypeConverter(typeof(DoubleCollectionConverter))]
    public sealed class DoubleCollection : ObservableCollection<double>
    {
        public float[] ToFloatArray()
        {
            float[] array = new float[Count];
           
            for (int i = 0; i < Count; i++)
            {
                array[i] = (float)Items[i];
            }

            return array;
        }
    }
}
