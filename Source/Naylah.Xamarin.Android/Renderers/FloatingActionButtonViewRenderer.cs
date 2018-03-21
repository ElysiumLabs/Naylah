namespace Naylah.Xamarin.Android.Renderers
{
    //public class FloatingActionButtonViewRenderer : ViewRenderer<FloatingActionButtonView, FrameLayout>
    //{
    //    private const int MARGIN_DIPS = 16;
    //    private const int FAB_HEIGHT_NORMAL = 56;
    //    private const int FAB_HEIGHT_MINI = 40;
    //    private const int FAB_FRAME_HEIGHT_WITH_PADDING = (MARGIN_DIPS * 2) + FAB_HEIGHT_NORMAL;
    //    private const int FAB_FRAME_WIDTH_WITH_PADDING = (MARGIN_DIPS * 2) + FAB_HEIGHT_NORMAL;
    //    private const int FAB_MINI_FRAME_HEIGHT_WITH_PADDING = (MARGIN_DIPS * 2) + FAB_HEIGHT_MINI;
    //    private const int FAB_MINI_FRAME_WIDTH_WITH_PADDING = (MARGIN_DIPS * 2) + FAB_HEIGHT_MINI;
    //    private readonly Context context;
    //    private readonly FloatingActionButton fab;

    //    public FloatingActionButtonViewRenderer()
    //    {
    //        context = Forms.Context;

    //        float d = context.Resources.DisplayMetrics.Density;
    //        var margin = (int)(MARGIN_DIPS * d); // margin in pixels

    //        fab = new FloatingActionButton(context);
    //        var lp = new FrameLayout.LayoutParams(LayoutParams.WrapContent, LayoutParams.WrapContent);
    //        lp.Gravity = GravityFlags.CenterVertical | GravityFlags.CenterHorizontal;
    //        lp.LeftMargin = margin;
    //        lp.TopMargin = margin;
    //        lp.BottomMargin = margin;
    //        lp.RightMargin = margin;
    //        fab.LayoutParameters = lp;
    //    }

    //    public void Show()
    //    {
    //        fab.Show();
    //    }

    //    public void Hide()
    //    {
    //        fab.Hide();
    //    }

    //    public void Fab_Click(object sender, EventArgs e)
    //    {
    //        var clicked = Element.Clicked;
    //        if (Element != null && clicked != null)
    //        {
    //            clicked(sender, e);
    //        }
    //    }

    //    protected override void OnElementChanged(ElementChangedEventArgs<FloatingActionButtonView> e)
    //    {
    //        base.OnElementChanged(e);

    //        if (e.OldElement != null || this.Element == null)
    //            return;

    //        if (e.OldElement != null)
    //            e.OldElement.PropertyChanged -= HandlePropertyChanged;

    //        if (this.Element != null)
    //        {
    //            //UpdateContent ();
    //            this.Element.PropertyChanged += HandlePropertyChanged;
    //        }

    //        Element.Show = Show;
    //        Element.Hide = Hide;

    //        SetFabImage(Element.Icon);
    //        SetFabSize(Element.Size);
    //        SetFabColor(Element.BackgroundColor);

    //        fab.Click += Fab_Click;

    //        var frame = new FrameLayout(context);
    //        frame.RemoveAllViews();
    //        frame.AddView(fab);

    //        SetNativeControl(frame);
    //    }

    //    private void HandlePropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    //    {
    //        if (e.PropertyName == "Content")
    //        {
    //            Tracker.UpdateLayout();
    //        }
    //        else if (e.PropertyName == FloatingActionButtonView.IconProperty.PropertyName)
    //        {
    //            SetFabImage(Element.Icon);
    //        }
    //        else if (e.PropertyName == FloatingActionButtonView.SizeProperty.PropertyName)
    //        {
    //            SetFabSize(Element.Size);
    //        }
    //        else if (e.PropertyName == FloatingActionButtonView.BackgroundColorProperty.PropertyName)
    //        {
    //            SetFabColor(Element.BackgroundColor);
    //        }
    //    }

    //    private void SetFabImage(string imageName)
    //    {
    //        if (!string.IsNullOrWhiteSpace(imageName))
    //        {
    //            try
    //            {
    //                var drawableNameWithoutExtension = Path.GetFileNameWithoutExtension(imageName);
    //                var resources = context.Resources;
    //                var imageResourceName = resources.GetIdentifier(drawableNameWithoutExtension, "drawable", context.PackageName);
    //                fab.SetImageBitmap(global::Android.Graphics.BitmapFactory.DecodeResource(context.Resources, imageResourceName));
    //            }
    //            catch (Exception ex)
    //            {
    //                throw new FileNotFoundException("There was no Android Drawable by that name.", ex);
    //            }
    //        }
    //    }

    //    private void SetFabColor(Color colorName)
    //    {
    //        fab.BackgroundTintList = ColorStateList.ValueOf(colorName.ToAndroid());
    //    }

    //    private void SetFabSize(FloatingActionButtonSize size)
    //    {
    //        if (size == FloatingActionButtonSize.Mini)
    //        {
    //            Element.WidthRequest = FAB_MINI_FRAME_WIDTH_WITH_PADDING;
    //            Element.HeightRequest = FAB_MINI_FRAME_HEIGHT_WITH_PADDING;
    //        }
    //        else
    //        {
    //            Element.WidthRequest = FAB_FRAME_WIDTH_WITH_PADDING;
    //            Element.HeightRequest = FAB_FRAME_HEIGHT_WITH_PADDING;
    //        }
    //    }
    //}
}