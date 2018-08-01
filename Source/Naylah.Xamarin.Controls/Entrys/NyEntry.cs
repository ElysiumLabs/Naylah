using System;
using Xamarin.Forms;

namespace Naylah.Xamarin.Controls.Entrys
{
    public class NyEntry : Entry
    {
        public NyEntry()
        {
        }

        private NyEntryStyle _entryStyle = NyEntryStyle.FloatLabeled;

        public NyEntryStyle EntryStyle
        {
            get { return _entryStyle; }
            set
            {
                if (_rendered)
                {
                    throw new Exception("We cannot change the type of control after rendered....");
                }
                else
                {
                    _entryStyle = value;
                }
            }
        }

        public bool _rendered;

        public Color RoundedBackgroundColor { get; set; } = Color.White;
        public Color RoundedBorderColor { get; set; } = Color.Accent;
        public float RoundedBorderRadius { get; set; } = 10;
        public Thickness RoundedPadding { get; set; } = 10;
    }

    public enum NyEntryStyle
    {
        Default,
        FloatLabeled,
        Rounded
    }
}