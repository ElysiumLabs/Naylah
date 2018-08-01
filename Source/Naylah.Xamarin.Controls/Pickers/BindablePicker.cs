using System;
using System.Collections;
using Xamarin.Forms;

namespace Naylah.Xamarin.Controls.Pickers
{
    public class BindablePicker : Picker
    {
        public bool FloatLabeledStyle { get; set; }

        //public PickerSyncronizationType SyncronizationType { get; set; } = PickerSyncronizationType.SelectedOrDone;

        public new static BindableProperty ItemsSourceProperty =
            BindableProperty.Create(nameof(ItemsSource), typeof(IList), typeof(BindablePicker), default(IList), propertyChanged: OnItemsSourceChanged);


        public BindablePicker()
        {
            this.SelectedIndexChanged += OnSelectedIndexChanged;
        }

        public new IList ItemsSource
        {
            get { return (IList)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }



        public Func<object, string> SourceItemLabelConverter { get; set; }

        private static void OnItemsSourceChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var oldValueList = oldValue as IList;
            var newValueList = newValue as IList;

            var picker = bindable as BindablePicker;
            picker.Items.Clear();
            if (newValue != null)
            {
                foreach (var item in newValueList)
                {
                    if (picker.SourceItemLabelConverter != null)
                    {
                        picker.Items.Add(picker.SourceItemLabelConverter(item));
                    }
                    else
                    {
                        picker.Items.Add(item.ToString());
                    }
                }
            }
        }

        private void OnSelectedIndexChanged(object sender, EventArgs eventArgs)
        {
            this.SelectedItem = (SelectedIndex < 0 || SelectedIndex > Items.Count - 1) ? null : ItemsSource[SelectedIndex];
        }

        private static void OnSelectedItemChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            var picker = bindable as BindablePicker;

            if (picker == null) return;

            try
            {
                picker.SelectedIndexChanged -= picker.OnSelectedIndexChanged;

                if (newvalue != null)
                {
                    var title = (picker.SourceItemLabelConverter != null) ? picker.SourceItemLabelConverter(newvalue) : newvalue.ToString();
                    picker.SelectedIndex = picker.Items.IndexOf(title);
                }
                else
                {
                    picker.SelectedIndex = -1;
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                picker.SelectedIndexChanged += picker.OnSelectedIndexChanged;
            }
        }

        public enum PickerSyncronizationType
        {
            Scrolling,
            SelectedOrDone
        }
    }
}