using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;

namespace tranquvis.Utils.WinFormDesign
{
    public class CustomDesignForm : Form
    {
        private Brush _windowBrush;
        private Brush _contentBrush;
        private Color _windowBackColor;
        private Color _contentBackColor;

        private Image _logoSrc;
        private int _logoHeight;
        private int _logoWidth;

        private Button _buttonMinimize;
        private Button _buttonClose;
        private int _buttonMinimizePos = 3;
        private int _buttonClosePos = 1;

        private Padding _contentPadding;

        /// <summary>
        /// create Form with custom design
        /// </summary>
        public CustomDesignForm()
        {
            FormControlWidth = CaptionBarHeight + FormBorderWidth;
            FormControlHeight = CaptionBarHeight + FormBorderWidth;

            FormBorderStyle = FormBorderStyle.None;
            DoubleBuffered = true;
            SetStyle(ControlStyles.ResizeRedraw, true); // this is to avoid visual artifacts

            //default settings
            WindowBackColor = Color.Gray;
            ContentBackColor = Color.White;
            FormControlHoverColor = Color.DimGray;
            ContentPadding = new Padding(5,5,5,5);
        }

        /// <summary>
        /// Override/Extend this method to init layout properties. <para />
        /// (this will be called after initalizeComponent)
        /// </summary>
        public virtual void InitCustomLayout()
        {
            UpdateFormControls();
        }

        /// <summary>
        /// is the same as ContentBackColor. <para />
        /// Setting has no effect! (necessary for VS Form-Designer)
        /// </summary>
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("use ContentBackColor instead", false)]
        public new Color BackColor
        {
            get { return ContentBackColor; }
            set { }
        }

        /// <summary>
        /// background color of window frame
        /// </summary>
        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
        public Color WindowBackColor
        {
            get { return _windowBackColor; }
            set
            {
                _windowBackColor = value;
                _windowBrush = new SolidBrush(_windowBackColor);
            }
        }

        /// <summary>
        /// background color of the content area
        /// </summary>
        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
        public Color ContentBackColor
        {
            get { return _contentBackColor; }
            set
            {
                _contentBackColor = value;
                _contentBrush = new SolidBrush(_contentBackColor);
            }
        }

        /// <summary>
        /// padding relative to the window border <para />
        /// Use ContentPadding instead, because the padding depends on CaptionBarHeight and FormBorderWidth.
        /// (necessary for VS Form-Designer)
        /// </summary>
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public new Padding Padding
        {
            get { return base.Padding; }
            set { base.Padding = value; }
        }

        /// <summary>
        /// additional padding relative to the content area
        /// </summary>
        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
        public Padding ContentPadding
        {
            get { return _contentPadding; }
            set
            {
                _contentPadding = value;
                Padding = new Padding(
                    FormBorderWidth + _contentPadding.Left, 
                    CaptionBarHeight + FormBorderWidth + _contentPadding.Top, 
                    FormBorderWidth + _contentPadding.Right, 
                    FormBorderWidth + _contentPadding.Bottom
                    );
            }
        }

        /// <summary>
        /// height of caption bar
        /// note taht the caption bar is under the top form border
        /// </summary>
        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
        public int CaptionBarHeight { get; set; } = 30;

        /// <summary>
        /// width of form border
        /// </summary>
        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
        public int FormBorderWidth { get; set; } = 5;

        /// <summary>
        /// width of form controls like the minimize button
        /// </summary>
        [Browsable(true), EditorBrowsable(EditorBrowsableState.Advanced)]
        public int FormControlWidth { get; set; }
        /// <summary>
        /// height of form controls like the minimize button
        /// </summary>
        [Browsable(true), EditorBrowsable(EditorBrowsableState.Advanced)]
        public int FormControlHeight { get; set; }

        /// <summary>
        /// source of icon in caption-bar or null if no icon should be displayed
        /// </summary>
        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
        public Image IconSrc
        {
            get { return _logoSrc; }
            set
            {
                _logoSrc = value;
                if (_logoSrc == null)
                    return;
                _logoHeight = CaptionBarHeight - 2 * IconMarginY;
                _logoWidth = (int)((float)_logoSrc.Width / _logoSrc.Height * _logoHeight);
            }
        }

        /// <summary>
        /// horizontal margin of the icon
        /// </summary>
        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
        public int IconMarginX { get; set; } = 2;

        /// <summary>
        /// vertical margin of the icon
        /// </summary>
        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
        public int IconMarginY { get; set; } = 4;

        /// <summary>
        /// form border style <para />
        /// Is always set to none! (necessary for VS Form-Designer)
        /// </summary>
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public new FormBorderStyle FormBorderStyle
        {
            get { return base.FormBorderStyle; }
            set { base.FormBorderStyle = FormBorderStyle.None; }
        }

        /// <summary>
        /// hover color of the form controls
        /// </summary>
        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
        public Color FormControlHoverColor { get; set; }


        /// <summary>
        /// (necessary for initialising custom layout in VS Designer) Do not call this method outside Designer!
        /// </summary>
        /// <param name="performLayout"></param>
        public new void ResumeLayout(bool performLayout)
        {
            //init custom layout after InitializeComponent
            InitCustomLayout();
            base.ResumeLayout(performLayout);
        }

        #region form controls

        /// <summary>
        /// get button template for form control
        /// override this to change button style
        /// </summary>
        /// <param name="pos">position index of form control from right to left (starting at 1)</param>
        /// <returns>button template</returns>
        public virtual Button FormControlButtonTempl
        {
            get
            {
                Button b = new Button();
                b.Anchor = AnchorStyles.Top | AnchorStyles.Right;
                b.FlatAppearance.BorderSize = 0;
                b.FlatAppearance.MouseOverBackColor = FormControlHoverColor;
                b.FlatStyle = FlatStyle.Flat;
                b.ForeColor = Color.DodgerBlue;
                b.BackColor = WindowBackColor;
                b.ImeMode = ImeMode.NoControl;
                b.Margin = new Padding(0);
                b.UseVisualStyleBackColor = true;
                return b;
            }
        }

        /// <summary>
        /// add form controls for minimizing and closing form 
        /// or update them if they have been already created
        /// </summary>
        public void UpdateFormControls()
        {
            if(_buttonMinimize == null)
            {
                _buttonMinimize = FormControlButtonTempl;
                _buttonMinimize.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Bold);
                _buttonMinimize.Text = "_";
                _buttonMinimize.Click += delegate { WindowState = FormWindowState.Minimized; };

                Controls.Add(_buttonMinimize);
            }
            _buttonMinimize.Size = new Size(FormControlWidth, FormControlHeight);
            _buttonMinimize.Location = new Point(ClientSize.Width - FormControlWidth * _buttonMinimizePos, 0);

            if (_buttonClose == null)
            {

                _buttonClose = FormControlButtonTempl;
                _buttonClose.Font = new Font("Microsoft Sans Serif", 12F);
                _buttonClose.Text = "x";
                _buttonClose.Click += delegate { Close(); };

                Controls.Add(_buttonClose);
            }
            _buttonClose.Size = new Size(FormControlWidth, FormControlHeight);
            _buttonClose.Location = new Point(ClientSize.Width - FormControlWidth * _buttonClosePos, 0);
        }

        #endregion

        #region window management
        protected override void OnPaint(PaintEventArgs e)
        {
            //draw window borders
            e.Graphics.FillRectangle(_windowBrush, BorderTopR);
            e.Graphics.FillRectangle(_windowBrush, BorderLeftR);
            e.Graphics.FillRectangle(_windowBrush, BorderRightR);
            e.Graphics.FillRectangle(_windowBrush, BorderBottomR);

            //draw caption-bar
            e.Graphics.FillRectangle(_windowBrush, CaptionBarR);

            //draw logo
            if(IconSrc != null)
                e.Graphics.DrawImage(IconSrc, IconMarginX, IconMarginY, _logoWidth, _logoHeight);

            //draw content areas
            e.Graphics.FillRectangle(_contentBrush, ContentR);
        }

        Rectangle ContentR { get { return new Rectangle(FormBorderWidth, CaptionBarHeight + FormBorderWidth, ClientSize.Width - 2 * FormBorderWidth, ClientSize.Height - CaptionBarHeight - FormBorderWidth*2); } }

        Rectangle CaptionBarR { get { return new Rectangle(FormBorderWidth, FormBorderWidth, ClientSize.Width - 2* FormBorderWidth, CaptionBarHeight); } }

        Rectangle BorderTopR { get { return new Rectangle(0, 0, ClientSize.Width, FormBorderWidth); } }
        Rectangle BorderLeftR { get { return new Rectangle(0, 0, FormBorderWidth, ClientSize.Height); } }
        Rectangle BorderBottomR { get { return new Rectangle(0, ClientSize.Height - FormBorderWidth, ClientSize.Width, FormBorderWidth); } }
        Rectangle BorderRightR { get { return new Rectangle(ClientSize.Width - FormBorderWidth, 0, FormBorderWidth, ClientSize.Height); } }

        Rectangle CornerTopLeftR { get { return new Rectangle(0, 0, FormBorderWidth, FormBorderWidth); } }
        Rectangle CornerTopRightR { get { return new Rectangle(ClientSize.Width - FormBorderWidth, 0, FormBorderWidth, FormBorderWidth); } }
        Rectangle CornerBottomLeftR { get { return new Rectangle(0, ClientSize.Height - FormBorderWidth, FormBorderWidth, FormBorderWidth); } }
        Rectangle CornerBottomRightR { get { return new Rectangle(ClientSize.Width - FormBorderWidth, ClientSize.Height - FormBorderWidth, FormBorderWidth, FormBorderWidth); } }
        
        private const int
            HT_CAPTION = 2,
            HT_LEFT = 10,
            HT_RIGHT = 11,
            HT_TOP = 12,
            HT_TOPLEFT = 13,
            HT_TOPRIGHT = 14,
            HT_BOTTOM = 15,
            HT_BOTTOMLEFT = 16,
            HT_BOTTOMRIGHT = 17;

        /// <summary>
        /// make windows resizeable and moveable
        /// </summary>
        /// <param name="message"></param>
        protected override void WndProc(ref Message message)
        {
            base.WndProc(ref message);

            if (message.Msg == 0x84)
            {
                Point cursor = PointToClient(Cursor.Position);

                if (CaptionBarR.Contains(cursor)) message.Result = (IntPtr)HT_CAPTION;

                else if (CornerTopLeftR.Contains(cursor)) message.Result = (IntPtr)HT_TOPLEFT;
                else if (CornerTopRightR.Contains(cursor)) message.Result = (IntPtr)HT_TOPRIGHT;
                else if (CornerBottomLeftR.Contains(cursor)) message.Result = (IntPtr)HT_BOTTOMLEFT;
                else if (CornerBottomRightR.Contains(cursor)) message.Result = (IntPtr)HT_BOTTOMRIGHT;

                else if (BorderTopR.Contains(cursor)) message.Result = (IntPtr)HT_TOP;
                else if (BorderLeftR.Contains(cursor)) message.Result = (IntPtr)HT_LEFT;
                else if (BorderRightR.Contains(cursor)) message.Result = (IntPtr)HT_RIGHT;
                else if (BorderBottomR.Contains(cursor)) message.Result = (IntPtr)HT_BOTTOM;
            }
        }
        #endregion
    }
}
