namespace Xamarin.Forms.Media
{
    public class PathGeometryConverter : TypeConverter
    {
        public override object ConvertFromInvariantString(string value)
        {
            PathGeometry pathGeometry = new PathGeometry();

            PathFigureCollectionConverter.FillFigures(pathGeometry.Figures, value, true);

            return pathGeometry;
        }
    }
}
