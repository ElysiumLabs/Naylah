﻿namespace Naylah.Xamarin.iOS.Renderers
{
    //public class PickerRenderer : ViewRenderer<Picker, UITextField>
    //{
    //    private UIPickerView _picker;
    //    private UIColor _defaultTextColor;
    //    private bool _disposed;

    //    private IElementController ElementController => Element as IElementController;

    //    protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
    //    {
    //        if (e.OldElement != null)
    //            ((INotifyCollectionChanged)e.OldElement.Items).CollectionChanged -= RowsCollectionChanged;

    //        if (e.NewElement != null)
    //        {
    //            if (Control == null)
    //            {
    //                var entry = new NoCaretField { BorderStyle = UITextBorderStyle.RoundedRect };
    //                if (e.NewElement as BindablePicker != null)
    //                {
    //                    if (((BindablePicker)e.NewElement).FloatLabeledStyle)
    //                    {
    //                        entry.BorderStyle = UITextBorderStyle.None;
    //                    }
    //                }

    //                entry.EditingDidBegin += OnStarted;
    //                entry.EditingDidEnd += OnEnded;

    //                _picker = new UIPickerView();

    //                var width = UIScreen.MainScreen.Bounds.Width;
    //                var toolbar = new UIToolbar(new RectangleF(0, 0, width, 44)) { BarStyle = UIBarStyle.Default, Translucent = true };
    //                var spacer = new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace);
    //                var doneButton = new UIBarButtonItem(UIBarButtonSystemItem.Done, (o, a) =>
    //                {
    //                    var s = (PickerSource)_picker.Model;
    //                    if (s.SelectedIndex == -1 && Element.Items != null && Element.Items.Count > 0)
    //                        UpdatePickerSelectedIndex(0);
    //                    UpdatePickerFromModel(s);
    //                    entry.ResignFirstResponder();
    //                });

    //                toolbar.SetItems(new[] { spacer, doneButton }, false);

    //                entry.InputView = _picker;
    //                entry.InputAccessoryView = toolbar;

    //                _defaultTextColor = entry.TextColor;

    //                SetNativeControl(entry);
    //            }

    //            _picker.Model = new PickerSource(this);

    //            UpdatePicker();
    //            UpdateTextColor();

    //            ((INotifyCollectionChanged)e.NewElement.Items).CollectionChanged += RowsCollectionChanged;
    //        }

    //        base.OnElementChanged(e);
    //    }

    //    protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
    //    {
    //        base.OnElementPropertyChanged(sender, e);
    //        if (e.PropertyName == Picker.TitleProperty.PropertyName)
    //            UpdatePicker();
    //        if (e.PropertyName == Picker.SelectedIndexProperty.PropertyName)
    //            UpdatePicker();
    //        if (e.PropertyName == Picker.TextColorProperty.PropertyName || e.PropertyName == VisualElement.IsEnabledProperty.PropertyName)
    //            UpdateTextColor();
    //    }

    //    private void OnEnded(object sender, EventArgs eventArgs)
    //    {
    //        ElementController.SetValueFromRenderer(VisualElement.IsFocusedPropertyKey, false);
    //    }

    //    private void OnStarted(object sender, EventArgs eventArgs)
    //    {
    //        ElementController.SetValueFromRenderer(VisualElement.IsFocusedPropertyKey, true);
    //    }

    //    private void RowsCollectionChanged(object sender, EventArgs e)
    //    {
    //        UpdatePicker();
    //    }

    //    private void UpdatePicker()
    //    {
    //        var selectedIndex = Element.SelectedIndex;
    //        var items = Element.Items;
    //        Control.Placeholder = Element.Title;
    //        var oldText = Control.Text;
    //        Control.Text = selectedIndex == -1 || items == null ? "" : items[selectedIndex];
    //        UpdatePickerNativeSize(oldText);
    //        _picker.ReloadAllComponents();
    //        if (items == null || items.Count == 0)
    //            return;

    //        UpdatePickerSelectedIndex(selectedIndex);
    //    }

    //    private void UpdatePickerFromModel(PickerSource s)
    //    {
    //        if (Element != null)
    //        {
    //            var oldText = Control.Text;
    //            Control.Text = s.SelectedItem;
    //            UpdatePickerNativeSize(oldText);
    //            ElementController.SetValueFromRenderer(Picker.SelectedIndexProperty, s.SelectedIndex);
    //        }
    //    }

    //    private void UpdatePickerNativeSize(string oldText)
    //    {
    //        if (oldText != Control.Text)
    //            ((IVisualElementController)Element).NativeSizeChanged();
    //    }

    //    private void UpdatePickerSelectedIndex(int formsIndex)
    //    {
    //        var source = (PickerSource)_picker.Model;
    //        source.SelectedIndex = formsIndex;
    //        source.SelectedItem = formsIndex >= 0 ? Element.Items[formsIndex] : null;
    //        _picker.Select(Math.Max(formsIndex, 0), 0, true);
    //    }

    //    private void UpdateTextColor()
    //    {
    //        var textColor = Element.TextColor;

    //        if (textColor.IsDefault || !Element.IsEnabled)
    //            Control.TextColor = _defaultTextColor;
    //        else
    //            Control.TextColor = textColor.ToUIColor();
    //    }

    //    protected override void Dispose(bool disposing)
    //    {
    //        if (_disposed)
    //            return;

    //        _disposed = true;

    //        if (disposing)
    //        {
    //            _defaultTextColor = null;

    //            if (_picker != null)
    //            {
    //                if (_picker.Model != null)
    //                {
    //                    _picker.Model.Dispose();
    //                    _picker.Model = null;
    //                }

    //                _picker.RemoveFromSuperview();
    //                _picker.Dispose();
    //                _picker = null;
    //            }

    //            if (Control != null)
    //            {
    //                Control.EditingDidBegin -= OnStarted;
    //                Control.EditingDidEnd -= OnEnded;
    //            }

    //            if (Element != null)
    //                ((INotifyCollectionChanged)Element.Items).CollectionChanged -= RowsCollectionChanged;
    //        }

    //        base.Dispose(disposing);
    //    }

    //    private class PickerSource : UIPickerViewModel
    //    {
    //        private PickerRenderer _renderer;
    //        private bool _disposed;

    //        public PickerSource(PickerRenderer renderer)
    //        {
    //            _renderer = renderer;
    //        }

    //        public int SelectedIndex { get; internal set; }

    //        public string SelectedItem { get; internal set; }

    //        public override nint GetComponentCount(UIPickerView picker)
    //        {
    //            return 1;
    //        }

    //        public override nint GetRowsInComponent(UIPickerView pickerView, nint component)
    //        {
    //            return _renderer.Element.Items != null ? _renderer.Element.Items.Count : 0;
    //        }

    //        public override string GetTitle(UIPickerView picker, nint row, nint component)
    //        {
    //            return _renderer.Element.Items[(int)row];
    //        }

    //        public override void Selected(UIPickerView picker, nint row, nint component)
    //        {
    //            if (_renderer.Element.Items.Count == 0)
    //            {
    //                SelectedItem = null;
    //                SelectedIndex = -1;
    //            }
    //            else
    //            {
    //                SelectedItem = _renderer.Element.Items[(int)row];
    //                SelectedIndex = (int)row;
    //            }

    //            if (_renderer.Element.On<PlatformConfiguration.iOS>().UpdateMode() == UpdateMode.Immediately)
    //                _renderer.UpdatePickerFromModel(this);
    //        }

    //        protected override void Dispose(bool disposing)
    //        {
    //            if (_disposed)
    //                return;

    //            _disposed = true;

    //            if (disposing)
    //                _renderer = null;

    //            base.Dispose(disposing);
    //        }
    //    }
    //}

    //internal class NoCaretField : UITextField
    //{
    //    public NoCaretField() : base(new System.Drawing.RectangleF())
    //    {
    //    }

    //    public override CGRect GetCaretRectForPosition(UITextPosition position)
    //    {
    //        return base.GetCaretRectForPosition(position);
    //    }
    //}
}