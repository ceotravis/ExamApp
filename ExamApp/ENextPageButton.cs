using System.ComponentModel;

namespace ExamApp {
    public class ENextPageButton : Button {

        private TabControl _parentTabControl;


        /// <summary>
        /// The TabControl that will be navigated
        /// </summary>
        [Browsable(true)]
        [Description("The TabControl that will be navigated. This cannot be (null)")]
        public TabControl ParentTabControl {
            get { return _parentTabControl; }
            set { _parentTabControl = value; }
        }

        /// <summary>
        /// Custom default settings used for the Next Page button
        /// This is also where the onClick method is registered
        /// </summary>
        public ENextPageButton() {
            _parentTabControl = ParentTabControl;
            Click += new EventHandler(ENextPageButton_Click);
            Text = "Nästa sida";
            Anchor = AnchorStyles.None;
            Size = new Size(100, 37);
            AutoSize = true;
        }

        /// <summary>
        /// Navigates the next in the _parentTabControl when the button is pressed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <exception cref="ArgumentNullException">_parentTabControl needs to be set to the parent control</exception>
        private void ENextPageButton_Click(object? sender, EventArgs? e) {
            if (_parentTabControl == null) {
                throw new ArgumentNullException("_parentTabControl", "_parentTabControl is null. Did you set ParentTabControl in the desinger?");
            }

            if (_parentTabControl.SelectedIndex < _parentTabControl.TabCount -1 ) {
                _parentTabControl.SelectedIndex++;
            }
        }
    }
}

