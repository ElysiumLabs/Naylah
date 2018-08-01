using Xamarin.Forms;

namespace Naylah.Xamarin.Controls.Buttons
{
    /// <summary>
    /// Ainda nao esta pronto pra producao
    /// </summary>
    public class CircleContentView : ContentView
    {
        //public new static readonly BindableProperty BackgroundColorProperty = BindableProperty.Create(nameof(BackgroundColor), typeof(Color), typeof(CircleContentView), default(Color));

        //public new double BackgroundColor
        //{
        //    get { return (double)this.GetValue(BackgroundColorProperty); }
        //    set { this.SetValue(BackgroundColorProperty, value); }
        //}

        public static readonly BindableProperty StrokeProperty = BindableProperty.Create(nameof(StrokeThickness), typeof(double), typeof(CircleContentView), (double)2);

        public double StrokeThickness
        {
            get { return (double)this.GetValue(StrokeProperty); }
            set { this.SetValue(StrokeProperty, value); }
        }

        public static readonly BindableProperty StrokeColorProperty = BindableProperty.Create(nameof(StrokeColor), typeof(Color), typeof(CircleContentView), Color.Black);

        public Color StrokeColor
        {
            get { return (Color)this.GetValue(StrokeColorProperty); }
            set { this.SetValue(StrokeColorProperty, value); }
        }

        public static readonly BindableProperty FillColorProperty = BindableProperty.Create(nameof(FillColor), typeof(Color), typeof(CircleContentView), Color.Transparent);

        public Color FillColor
        {
            get { return (Color)this.GetValue(FillColorProperty); }
            set { this.SetValue(FillColorProperty, value); }
        }
    }
}