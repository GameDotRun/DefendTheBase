using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

// Aaron Tighe 2016
// designed for in game menus, buttons etc.

namespace RPGEx
{

    public static class UiManager
    {
        public static List<Ui> UiScreens = new List<Ui>();

        public static void Add(Ui uiScreen)
        {
            UiScreens.Add(uiScreen);
        }

        public static void Draw(SpriteBatch sb)
        {
            foreach (Ui uiScreen in UiScreens)
            {
                uiScreen.Draw(sb);
            }
        }
    }

    /// <summary>
    ///  Ui class providing arrangement and other useful funtions.
    /// </summary>
    public class Ui
    {
        //Elements added to these lists can be manipulated. Nothing implemented yet.
        //Hoho list in list should be fun :P
        public List<List<UiButton>> ButtonList;
        public List<List<UiTextBox>> TextBoxList;
        public List<List<UiTextString>> StringList;
        public List<UiTabs> TabsList;
        public List<UiStatusBars> UiStatusBarList;

        public Vector2 WindowDimensions;

        public Ui(int WindowWidth, int WindowHeight)
        {
            WindowDimensions = new Vector2(WindowWidth, WindowHeight);

            ButtonList = new List<List<UiButton>>();
            TextBoxList = new List<List<UiTextBox>>();
            StringList = new List<List<UiTextString>>();
            TabsList = new List<UiTabs>();
            UiStatusBarList = new List<UiStatusBars>();

            UiManager.Add(this);
        }

        /// <summary>
        ///  Add elements to the Ui Control
        ///  cannot be removed
        /// </summary>
        public void Add<T>(ref List<T> UiElement)
        {
            if (UiElement is List<UiButton>)
                ButtonList.Add(UiElement as List<UiButton>);

            else if (UiElement is List<UiTextBox>)
                TextBoxList.Add(UiElement as  List<UiTextBox>);

            else if (UiElement is List<UiTextString>)
                StringList.Add(UiElement as List<UiTextString>);
        }

        public void Add<T>(ref T UiElement)
        {
            if (UiElement is UiTabs)
                TabsList.Add(UiElement as UiTabs);
            if (UiElement is UiStatusBars)
                UiStatusBarList.Add(UiElement as UiStatusBars);
        }

        public void Draw(SpriteBatch sb)
        {
            foreach (List<UiButton> List in ButtonList)
            {
                foreach (UiButton Button in List)
                {
                    Button.Draw(sb);
                }
            }

            foreach (List<UiTextString> List in StringList)
            {
                foreach (UiTextString String in List)
                {
                    String.DrawString(sb, Vector2.Zero);
                }
            }

            foreach (List<UiTextBox> List in TextBoxList)
            {
                foreach (UiTextBox TextBox in List)
                {
                    TextBox.Draw(sb);
                }
            }

            foreach (UiStatusBars Bars in UiStatusBarList)
            {
                Bars.Draw(sb);
            }

            foreach (UiTabs Tabs in TabsList)
            {
                Tabs.Draw(sb);
            }
        }  
    }

    /// <summary>
    /// Simple timer, could be useful.
    /// </summary>
    public class UiTimer
    {
        bool active = false;
        float target;
        float elasped = 0;

        public UiTimer(float TargetTime)
        {
            target = TargetTime;
            elasped = target;
        }

        public bool TimeReached()
        {
            if (active)
            {
                if (elasped <= 0)
                {
                    active = false;
                    elasped = target;

                    return true;
                }

                else return false;
            }

            else return true;
        }

        public void TimerUpdate(GameTime gt)
        {
            if(active)
                elasped -= gt.ElapsedGameTime.Milliseconds;
        }

        public void ActivateTimer()
        {
            active = true;    
        }

        public bool GetActive
        {
            get { return active; }
        } 
    }

    /// <summary>
    /// Creates a loading bar, health bar etc.
    /// </summary>
    public class UiStatusBars 
    {
        Texture2D BackgroundTex, ForegroundTex;
        Color BackgroundCol, ForegroundCol;
        Vector2 SizeOfBar, Location;
        float Total, Current;


        public UiStatusBars(GraphicsDevice graphicDev, float totalData, Vector2 size, Vector2 location, Color backCol, Color foreCol)
        {
            Total = totalData;
            SizeOfBar = size;
            BackgroundCol = backCol;
            ForegroundCol = foreCol;
            Location = location;

            BackgroundTex = new Texture2D(graphicDev, 1, 1, false, SurfaceFormat.Color);
            BackgroundTex.SetData(new Color[] { backCol });

            ForegroundTex = new Texture2D(graphicDev, 1, 1, false, SurfaceFormat.Color);
            ForegroundTex.SetData(new Color[] { foreCol });
        }

        public UiStatusBars(float totalData, Vector2 size, Vector2 location, Texture2D backTex, Texture2D foreTex)
        {
            Total = totalData;
            SizeOfBar = size;
            Location = location;
            BackgroundTex = backTex;
            ForegroundTex = foreTex;
        }

        public void Update(float currentData)
        {
            Current = currentData;
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(BackgroundTex, new Rectangle((int)Location.X, (int)Location.Y, (int)SizeOfBar.X, (int)SizeOfBar.Y), Color.White);
            sb.Draw(ForegroundTex, new Rectangle((int)Location.X, (int)Location.Y, (int)SizeOfBar.X * (int)Current / (int)Total, (int)SizeOfBar.Y), Color.White);
        }
    }

    /// <summary>
    ///  A class for tab pages.
    ///  Needs some refining
    /// </summary>
    public class UiTabs
    {
        internal List<UiTab> tabList;
        private int currentSelection;

        /// <summary>
        /// Create Tabs with a singular colour.
        /// The tabs will scale to the given text.
        /// </summary>
        public UiTabs(GraphicsDevice graphicDev, SpriteFont Font, int Pages, Vector2 TabDrawLocation, string[] TabName, Color TabColour)
        {
            tabList = new List<UiTab>();

            string TabID = "Tab";

            for (int i = 0; i < Pages; i++)
                tabList.Add(new UiTab(graphicDev, Font, i, TabID + i.ToString(), TabName[i], TabColour));

            //Set the Draw Locations for tabs.
            SetDrawLocations(TabDrawLocation, Pages);
        }

        /// <summary>
        /// Create Tabs with a texture.
        /// The tabs will scale to the given text.
        /// </summary>
        public UiTabs(GraphicsDevice graphicDev, SpriteFont Font, int Pages, Vector2 TabDrawLocation, string[] TabName, Texture2D TabTexture)
        {
            tabList = new List<UiTab>();

            string TabID = "Tab";

            for (int i = 0; i < Pages; i++)
                tabList.Add(new UiTab(graphicDev, Font, i, TabID + i.ToString(), TabName[i], TabTexture));

            //Set the Draw Locations for tabs.
            SetDrawLocations(TabDrawLocation, Pages);
        }

        /// <summary>
        /// Create Tabs with a texture.
        /// The tabs will scale to the given TabSize.
        /// Give empty tabnames for no text
        /// </summary>
        public UiTabs(GraphicsDevice graphicDev, SpriteFont Font, int Pages, Vector2 TabDrawLocation, string[] TabName, Texture2D TabTexture, Texture2D tabEffect, Vector2 TabSize)
        {
            tabList = new List<UiTab>();

            string TabID = "Tab";

            for (int i = 0; i < Pages; i++)
                tabList.Add(new UiTab(Font, i, TabID + i.ToString(), TabName[i], TabTexture, tabEffect, TabSize));

            //Set the Draw Locations for tabs.
            SetDrawLocations(TabDrawLocation, Pages);
        }

        /// <summary>
        /// Create Tabs with a Colour.
        /// The tabs will scale to the given TabSize.
        /// Give empty tabnames for no text
        /// </summary>
        public UiTabs(GraphicsDevice graphicDev, SpriteFont Font, int Pages, Vector2 TabDrawLocation, string[] TabName, Color TabColor, Vector2 TabSize)
        {
            tabList = new List<UiTab>();

            string TabID = "Tab";

            for (int i = 0; i < Pages; i++)
                tabList.Add(new UiTab(graphicDev, Font, i, TabID + i.ToString(), TabName[i], TabColor, TabSize));

            //Set the Draw Locations for tabs.
            SetDrawLocations(TabDrawLocation, Pages);
        }


        public void Update()
        {
            foreach (UiTab Tab in tabList)
            {
                if (Tab.tabNumber == currentSelection)
                    Tab.tabButton.isTabSelected = true;
                else Tab.tabButton.isTabSelected = false;

                if (Tab.tabButton.GetButtonState == UiButton.UiButtonStates.Button_Down)
                {
                    currentSelection = Tab.tabNumber;
                    SwitchPageListeners();
                }
            }

            tabList[currentSelection].tabButton.isTabSelected = true;
        }

        /// <summary>
        /// Add an element to a tab.
        /// </summary>
        public void Add<T>(T UiElement, int TabPage)
        {
            tabList[TabPage].Add(ref UiElement);
            SwitchPageListeners();
        }

        // draw the tab  and the currently selected tabs items
        public void Draw(SpriteBatch sb)
        {
            foreach (UiTab Tab in tabList)
            {
                /*if (Tab.tabNumber == currentSelection)
                    Tab.tabButton.TextBoxColour =;
                else*/
                Tab.Draw(sb);

            }

            foreach (UiButton button in tabList[currentSelection].ButtonList)
                button.DrawButton(sb);

            foreach (UiTextBox textBox in tabList[currentSelection].TextBoxList)
                textBox.Draw(sb);

            foreach (UiTextString TextString in tabList[currentSelection].StringList)
                TextString.DrawString(sb, Vector2.Zero);
        }

        private void SwitchPageListeners()
        {
            foreach (UiTab Tab in tabList)
            {
                if (Tab.tabNumber != currentSelection)
                {
                    foreach (UiButton Button in Tab.ButtonList)
                    {
                        Button.SetButtonState = UiButton.UiButtonStates.Button_Up;
                        UiButtonMessenger.RemoveButton(Button.GetButtonID);
                    }
                }

                else if (Tab.tabNumber == currentSelection)
                {
                    foreach (UiButton Button in Tab.ButtonList)
                    {
                        UiButtonMessenger.RegisterButton(Button);
                    }
                }
            }
        }

        private void SetDrawLocations(Vector2 TabDrawLocation, int Pages)
        {
            for (int i = 0; i < Pages; i++)
            {
                int tempInt;

                if (tabList[i].tabButton.ScaleBox)
                {
                    if (i == 0)
                    {
                        tabList[i].tabLocation = new Vector2(TabDrawLocation.X, TabDrawLocation.Y);
                        tabList[i].tabButton.TextBox = new Rectangle((int)tabList[i].tabLocation.X, (int)tabList[i].tabLocation.Y, (int)tabList[i].tabButton.StringPXSize.X, (int)tabList[i].tabButton.StringPXSize.Y);
                    }

                    else
                    {
                        tempInt = tabList[i - 1].tabButton.GetRectangleEdgeRight;
                        tabList[i].tabLocation = new Vector2(tempInt, TabDrawLocation.Y);
                        tabList[i].tabButton.TextBox = new Rectangle((int)tabList[i].tabLocation.X, (int)tabList[i].tabLocation.Y, (int)tabList[i].tabButton.StringPXSize.X, (int)tabList[i].tabButton.StringPXSize.Y);
                    }
                }

                if (!tabList[i].tabButton.ScaleBox)
                {
                    if (i == 0)
                    {
                        tabList[i].tabLocation = new Vector2(TabDrawLocation.X, TabDrawLocation.Y);
                        tabList[i].tabButton.TextBox = new Rectangle((int)tabList[i].tabLocation.X, (int)tabList[i].tabLocation.Y, (int)tabList[i].tabButton.TextBox.Width, (int)tabList[i].tabButton.TextBox.Height);
                    }

                    else
                    {
                        tempInt = tabList[i - 1].tabButton.GetRectangleEdgeRight;
                        tabList[i].tabLocation = new Vector2(tempInt, TabDrawLocation.Y);
                        tabList[i].tabButton.TextBox = new Rectangle((int)tabList[i].tabLocation.X, (int)tabList[i].tabLocation.Y, (int)tabList[i].tabButton.TextBox.Width, (int)tabList[i].tabButton.TextBox.Height);
                    }
                }

                tabList[i].tabButton.StringPosition = tabList[i].tabLocation;

                tabList[i].LocationResetter();
            }
        }
    }

    internal class UiTab
    {
        //List of items the tab contains, will be fed to the tabs own draw method
        public UiButton tabButton;
        public List<UiButton> ButtonList;
        public List<UiTextBox> TextBoxList;
        public List<UiTextString> StringList;

        public int tabNumber;
        public Vector2 tabLocation;

        public UiTab(GraphicsDevice graphicDev, SpriteFont Font, int TabNumber, string TabID, string TabName, Color TabColour)
        {
            tabNumber = TabNumber;

            //create Button for Tab
            tabButton = new UiButton(graphicDev, Font, Vector2.Zero, Vector2.Zero, TabColour, TabID, true);
            UiButtonMessenger.RegisterButton(tabButton);
            tabButton.ScaleBox = true;
            tabButton.isTabButton = true;
            Initilise(TabName);
        }

        public UiTab(GraphicsDevice graphicDev, SpriteFont Font, int TabNumber, string TabID, string TabName, Texture2D TabTexture)
        {
            tabNumber = TabNumber;

            //create Button for Tab
            tabButton = new UiButton(graphicDev, Font, Vector2.Zero, Vector2.Zero, TabTexture, TabID, true);
            UiButtonMessenger.RegisterButton(tabButton);
            tabButton.ScaleBox = true;
            tabButton.isTabButton = true;
            Initilise(TabName);
        }

        public UiTab(GraphicsDevice graphicDev, SpriteFont Font, int TabNumber, string TabID, string TabName, Texture2D TabTexture, Vector2 TabSize)
        {
            tabNumber = TabNumber;

            //create Button for Tab
            tabButton = new UiButton(graphicDev, Font, Vector2.Zero, TabSize, TabTexture, TabID, true);
            UiButtonMessenger.RegisterButton(tabButton);
            tabButton.ScaleBox = false;
            tabButton.isTabButton = true;
            Initilise(TabName);
        }

        public UiTab(GraphicsDevice graphicDev, SpriteFont Font, int TabNumber, string TabID, string TabName, Color TabColor, Vector2 TabSize)
        {
            tabNumber = TabNumber;

            //create Button for Tab
            tabButton = new UiButton(graphicDev, Font, Vector2.Zero, TabSize, TabColor, TabID, true);
            UiButtonMessenger.RegisterButton(tabButton);
            tabButton.ScaleBox = false;
            tabButton.isTabButton = true;
            Initilise(TabName);
        }

        public UiTab(SpriteFont Font, int TabNumber, string TabID, string TabName, Texture2D tabTex, Texture2D TabEffect, Vector2 TabSize)
        {
            tabNumber = TabNumber;

            //create Button for Tab
            tabButton = new UiButton(Font, Vector2.Zero, TabSize, tabTex, TabEffect, TabID, true);
            UiButtonMessenger.RegisterButton(tabButton);
            tabButton.ScaleBox = false;
            tabButton.isTabButton = true;
            Initilise(TabName);
        }

        public void Add<T>(ref T UiElement)
        {
            if (UiElement is UiButton)
                ButtonList.Add(UiElement as UiButton);

            else if (UiElement is UiTextBox)
                TextBoxList.Add(UiElement as UiTextBox);

            else if (UiElement is UiTextString)
                StringList.Add(UiElement as UiTextString);
        }

        internal void Draw(SpriteBatch sb)
        {
            tabButton.DrawButton(sb);
        }

        internal void LocationResetter()
        {
            tabButton.TextBoxLocation = tabLocation;
        }

        private void InitiliseLists()
        {
            ButtonList = new List<UiButton>();
            TextBoxList = new List<UiTextBox>();
            StringList = new List<UiTextString>();
        }

        private void Initilise(string TabName)
        {
            tabButton.StringText = TabName;
            tabButton.GetStringSizePX();
            InitiliseLists();
        }
    }

    /// <summary>
    ///  Handles button updates.
    /// </summary>
    static public class UiButtonMessenger
    {
        static public List<UiButton> ButtonListeners;
        static public string ButtonPressedId;

        static public void InitiliseListenerList()
        {
            ButtonListeners = new List<UiButton>();
        }

        /// <summary>
        /// Get Button updates that have been assigned to the Listener
        /// </summary>
        static public void ButtonResponder(MouseState mouseState, MouseState old)
        {
            foreach (UiButton button in ButtonListeners)
            {
                button.Update(mouseState, old);
                ButtonPressedId = ReturnButtonPressed();
            }
        }

        /// <summary>
        /// Register a button to the ListenerList. Allows the button to recieve and send updates.
        /// It wont allow the same button to be registered twice
        /// </summary>
        static public void RegisterButton(UiButton Button)
        {
            if (!ButtonListeners.Exists(item => string.Compare(item.GetButtonID, Button.GetButtonID, 0) == 0))
                ButtonListeners.Add(Button);
        }

        /// <summary>
        /// Remove a button from the listeners
        /// WARNING - The button will maintain its current state once removed, until re-added
        /// manual resetting of button state maybe required
        /// </summary>
        static public void RemoveButton(string ButtonID)
        {
            int index = ButtonListeners.FindIndex(item => string.Compare(item.GetButtonID, ButtonID, 0) == 0);

            if (index >= 0)
                ButtonListeners.RemoveAt(index);
        }

        static public bool CheckButtonIsDown(string ButtonName)
        {
            foreach (UiButton Button in ButtonListeners)
            {
                if (Button.GetButtonID == ButtonName)
                    if (Button.IsButtonDown())
                        return true;
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        static string ReturnButtonPressed()
        {
            foreach (UiButton Button in ButtonListeners)
            {
                if (Button.IsButtonDown())
                    return Button.GetButtonID;
            }

            return null;       
        }
        
    }

    /// <summary>
    /// Simple button with various editable variables.
    /// </summary>
    public class UiButton : UiTextBox
    {
        private string buttonID;
        private bool buttonEffects = true;
        private float highlight = 1;
        private int sizeChange = 0;

        internal bool isTabButton;
        internal bool isTabSelected;

        Texture2D effectTexture;

        bool DisplayEffect = false;

        public enum UiButtonStates
        {
            Button_Down,
            Button_Up
        }

        UiButtonStates buttonState;

        public UiButton(GraphicsDevice graphicDev, SpriteFont Font, Vector2 Location, Vector2 Size, Texture2D Texture, string ButtonID, bool ButtonEffects)
            : base(Font, graphicDev)
        {
            TextBoxLocation = Location;
            TextBoxTexture = Texture;
            TextBoxSize = Size;
            buttonID = ButtonID;
            buttonEffects = ButtonEffects;
            StringPosition = Location;
            InitiliseButton();
        }

        public UiButton(GraphicsDevice graphicDev, SpriteFont Font, Vector2 Location, Vector2 Size, Color BoxColour, string ButtonID, bool ButtonEffects)
            : base(Font, graphicDev)
        {
            TextBoxColour = BoxColour;
            TextBoxTexture.SetData(new Color[] { BoxColour });
            TextBoxLocation = Location;
            TextBoxSize = Size;
            buttonID = ButtonID;
            buttonEffects = ButtonEffects;
            StringPosition = Location;
            InitiliseButton();
        }

        public UiButton(SpriteFont Font, Vector2 Location, Vector2 Size, Texture2D Texture, Texture2D EffectTexture, string ButtonID, bool ButtonEffects)
            : base(Font, "", Location, Color.White, Texture, false)
        {
            effectTexture = EffectTexture;
            TextBoxLocation = Location;
            TextBoxTexture = Texture;
            TextBoxSize = Size;
            buttonID = ButtonID;
            buttonEffects = ButtonEffects;
            StringPosition = Location;
            InitiliseButton();
        }

        public void Update(MouseState mouseState, MouseState old)
        {
            if (TextBox.Contains(new Point(mouseState.X, mouseState.Y)))
            {
                if (buttonEffects)
                    DisplayEffect = true;

                if (mouseState.LeftButton == ButtonState.Pressed && mouseState.LeftButton != old.LeftButton)
                {
                    buttonState = UiButtonStates.Button_Down;
                }

                else if (!isTabButton)
                    buttonState = UiButtonStates.Button_Up;
            }

            else
            {
                DisplayEffect = false;
                buttonState = UiButtonStates.Button_Up;
            }

            if (isTabButton && isTabSelected)
            {
                sizeChange = 5;
                DisplayEffect = true;
            }

            else if (IsButtonDown() && buttonEffects)
                sizeChange = 5;

            else
            {
                sizeChange = 0;
            }
        }

        public void DrawButton(SpriteBatch sb)
        {
            if (ScaleBox)
                sb.Draw(TextBoxTexture, new Rectangle((int)TextBox.X, (int)TextBox.Y + sizeChange / 2, (int)StringPXSize.X, (int)StringPXSize.Y), null,
                    TextBoxColour * highlight, TextBoxRotation, Vector2.Zero, SpriteEffects.None, 1);
            else
                sb.Draw(TextBoxTexture, new Rectangle((int)TextBox.X, (int)TextBox.Y + sizeChange / 2, (int)TextBoxSize.X, (int)TextBoxSize.Y), null,
                    TextBoxColour * highlight, TextBoxRotation, Vector2.Zero, SpriteEffects.None, 0);

            if(DisplayEffect)
                sb.Draw(effectTexture, new Rectangle((int)TextBox.X, (int)TextBox.Y + sizeChange / 2, (int)TextBoxSize.X, (int)TextBoxSize.Y), null,
                    TextBoxColour * highlight, TextBoxRotation, Vector2.Zero, SpriteEffects.None, 0);

            DrawString(sb, StringOffset);
        }

        private void InitiliseButton()
        {
            TextBoxRectangleSet();
            GetStringSizePX();
        }

        public bool IsButtonDown()
        {
            if (buttonState == UiButtonStates.Button_Down)
                return true;

            else return false;
        }

        public UiButtonStates GetButtonState
        {
            get { return buttonState; }
        }

        public UiButtonStates SetButtonState
        {
            set { buttonState = value; }
        }

        public string GetButtonID
        {
            get { return buttonID; }
        }

        public int GetRectangleEdgeLeft
        {
            get { return TextBox.Left; }
        }

        public int GetRectangleEdgeRight
        {
            get { return TextBox.Right; }
        }

        public void SetStringPos()
        {
            StringPosition = TextBoxLocation;
        
        }
    }

    /// <summary>
    /// A Simple Box with Optional text.
    /// Without Text will just be a useless coloured rectangle... At least until this class is done
    /// </summary>
    public class UiTextBox : UiTextString // Not currently what i want. Will be changed to an editable text box in future (Take more time)
    {
        private Vector2 stringOffset = new Vector2(0,0);
        private Vector2 txtBoxlocation, txtBoxSize;
        private Rectangle txtBox;
        private Texture2D txtBoxTex;
        private Color txtBoxCol;
        private float txtBoxRotation, txtBoxScale;
        private bool scaleBox = false;

        private bool lineWrap = false;

        public UiTextBox(GraphicsDevice graphicDev)
            : base() // DEFAULT CONSTRUCTOR
        {
            TextBoxTextureDefSet(graphicDev);
            Initilise();
        }

        /// <summary>
        /// Constructs a box with no Text.
        /// </summary>
        public UiTextBox(SpriteFont Font, GraphicsDevice graphicDev)
            : base(Font)
        {
            TextBoxTextureDefSet(graphicDev);
            Initilise();
        }

        /// <summary>
        /// Constructs a box with text and choice of location. 
        /// Optional: Box can autoscale to text size
        /// </summary>
        public UiTextBox(SpriteFont Font, GraphicsDevice graphicDev, string Text, Vector2 Location, bool ScaleBoxToText)
            : base(Font)
        {
            Initilise();
            TextBoxTextureDefSet(graphicDev);
            txtBoxlocation = Location;
            scaleBox = ScaleBoxToText;

            StringText = Text;
            GetStringSizePX();
        }

        /// <summary>
        /// Constructs a box with text and choice of location. 
        /// Box Colour and Text Colour can be chosen. 
        /// Optional: Box can autoscale to text size
        /// </summary>
        public UiTextBox(SpriteFont Font, GraphicsDevice graphicDev, string Text, Vector2 Location, Color TextColour, Color BoxColour, bool ScaleBoxToText)
            : base(Font)
        {
            Initilise();
            TextBoxTextureDefSet(graphicDev);
            txtBoxTex.SetData(new Color[] { BoxColour });
            txtBoxlocation = Location;
            TextBoxRectangleSet();
            scaleBox = ScaleBoxToText;

            StringColour = TextColour;
            StringText = Text;
            GetStringSizePX();

        }

        /// <summary>
        /// Constructs a box with text and choice of location.
        /// Text Colour can be Chosen.
        /// Box can have a texture.
        /// Optional: Box can autoscale to text size
        /// </summary>
        public UiTextBox(SpriteFont Font, string Text, Vector2 Location, Color TextColour, Texture2D BoxTex, bool ScaleBoxToText)
            : base(Font) //
        {
            Initilise();
            txtBoxCol = Color.White;
            txtBoxTex = BoxTex;
            txtBoxlocation = Location;
            TextBoxRectangleSet();
            scaleBox = ScaleBoxToText;

            StringColour = TextColour;
            StringText = Text;
            GetStringSizePX();
        }

        public void Draw(SpriteBatch sb)
        {
            GetStringSizePX();
            TextBoxRectangleSet();

            if (scaleBox)
            {
                sb.Draw(txtBoxTex, new Rectangle((int)txtBoxlocation.X, (int)txtBoxlocation.Y, (int)StringPXSize.X + 5, (int)StringPXSize.Y), null, txtBoxCol, txtBoxRotation, Vector2.Zero, SpriteEffects.None, 1);
                sb.DrawString(StringFont, StringText, txtBoxlocation + stringOffset + new Vector2(2,0), StringColour, txtBoxRotation, Vector2.Zero, StringScale, SpriteEffects.None, 1);
            }
            else
            {
                sb.Draw(txtBoxTex, txtBox, null, txtBoxCol, txtBoxRotation, Vector2.Zero, SpriteEffects.None, 1);
                sb.DrawString(StringFont, StringText, txtBoxlocation + stringOffset, StringColour, txtBoxRotation, Vector2.Zero, StringScale, SpriteEffects.None, 1);
            }
            
            
        }

        private void Initilise()
        {
            txtBoxlocation = new Vector2(0, 0);
            txtBoxSize = new Vector2(40, 20);
            TextBoxRectangleSet();
            txtBoxScale = 1f;
            txtBoxRotation = 0f;

            StringText = "";
            StringPosition = txtBoxlocation;
        }

        public void LineWrapper()
        {
            TextBoxRectangleSet();

            string[] tokenized = StringText.Split(' ','\r');

            List<string> TempStringList = new List<string>();

            int CurrentToken = 0;
            int Prevlength;

            TempStringList.Add("");
            foreach (string stringToken in tokenized)
            {
               
                Prevlength = TempStringList[CurrentToken].Length - 1;

                TempStringList[CurrentToken] += stringToken + " ";

                int length = TempStringList[CurrentToken].Length - 1;

                if(GetNewStringSizePX(TempStringList[CurrentToken]).X > TextBox.Width - 5)
                {
                    string tempStrLine = TempStringList[CurrentToken].Remove(Prevlength,  length - Prevlength);
                    TempStringList[CurrentToken] = tempStrLine + "\n";
                    TempStringList.Add(stringToken + " ");
                    CurrentToken++;
                }
            }

            StringText = "";

            foreach (string String in TempStringList)
            {
                StringText += String;
            }
        }

        private Vector2 GetNewStringSizePX(string text)
        {
            if (text != null)
            {
                Vector2 stringsize = StringFont.MeasureString(text);
                return stringsize;
            }

            else return Vector2.Zero;
        
        }


        /// <summary>
        /// Get/set TextBoxLocation
        /// </summary>
        public Vector2 TextBoxLocation
        {
            get { return txtBoxlocation; }
            set { txtBoxlocation = value; }
        }

        /// <summary>
        /// Get/set TextBoxSize
        /// </summary>
        public Vector2 TextBoxSize
        {
            get { return txtBoxSize; }
            set { txtBoxSize = value; }
        }

        public Vector2 StringOffset
        {
            get { return stringOffset; }
            set { stringOffset = value; }
        }

        /// <summary>
        /// Get/set TextBoxTexture
        /// </summary>
        public Texture2D TextBoxTexture
        {
            get { return txtBoxTex; }
            set { txtBoxTex = value; }
        }

        /// <summary>
        /// Get/set TextBoxColour
        /// </summary>
        public Color TextBoxColour
        {
            get { return txtBoxCol; }
            set { txtBoxCol = value; }
        }

        /// <summary>
        /// Get/set TextBoxRotation
        /// </summary>
        public float TextBoxRotation
        {
            get { return txtBoxRotation; }
            set { txtBoxRotation = value; }
        }

        /// <summary>
        /// Get/set TextBoxScale
        /// </summary>
        public float TextBoxScale
        {
            get { return txtBoxScale; }
            set { txtBoxScale = value; }
        }

        /// <summary>
        /// Get/set TextBox
        /// </summary>
        public Rectangle TextBox
        {
            get { return txtBox; }
            set { txtBox = value; }
        }

        /// <summary>
        /// Get/set TextBoxScale
        /// </summary>
        public bool ScaleBox
        {
            get { return scaleBox; }
            set { scaleBox = value; }
        }

        public bool LineWrap
        {
            get { return lineWrap; }
            set { lineWrap = value; }
        }


        public void TextBoxRectangleSet()
        {
            txtBox = new Rectangle((int)txtBoxlocation.X, (int)txtBoxlocation.Y, (int)txtBoxSize.X, (int)txtBoxSize.Y);
        }

        private void TextBoxTextureDefSet(GraphicsDevice graphicDev)
        {
            txtBoxTex = new Texture2D(graphicDev, 1, 1, false, SurfaceFormat.Color);
            txtBoxTex.SetData(new Color[] { Color.Blue });
            txtBoxCol = Color.White;
        }

    }

    /// <summary>
    /// Provides a easy string manager. Allows manupulation of the string drawn on screen easier with less mucking about.
    /// Can get string size in pixels.
    /// Can rotate and scale.
    /// Can also change string Colour
    /// </summary>
    public class UiTextString
    {
        private Vector2 txtStringPos;
        private SpriteFont txtFont;
        private Vector2 txtSizePX;
        private Color txtCol;
        private string txtStr;
        private float txtRotation, txtScale;

        public UiTextString() // default constructor
        {
        }

        /// <summary>
        /// Creates empty text string, location (0,0) 
        /// </summary>
        public UiTextString(SpriteFont TextFont)
        {
            Initilise(TextFont);
            GetStringSizePX();
        }

        /// <summary>
        /// Creates text String with specified text. Location (0,0)
        /// </summary>
        public UiTextString(SpriteFont TextFont, string Text)
        {
            Initilise(TextFont);
            txtStr = Text;
            GetStringSizePX();
        }

        /// <summary>
        /// Creates text string with specified text. Location = stringPos 
        /// </summary>
        public UiTextString(SpriteFont TextFont, string Text, Vector2 StringPos, Color Colour)
        {
            Initilise(TextFont);
            txtStr = Text;
            txtStringPos = StringPos;
            txtCol = Colour;
            GetStringSizePX();
        }

        public void DrawString(SpriteBatch sb, Vector2 offset)
        {
            sb.DrawString(txtFont, txtStr, txtStringPos + offset, txtCol, txtRotation, Vector2.Zero, txtScale, SpriteEffects.None, 1);
        }


        private void Initilise(SpriteFont TextFont)
        {
            txtFont = TextFont;
            txtStr = "";
            txtStringPos = Vector2.Zero;
            txtCol = Color.Black;
            txtScale = 1f;
            txtRotation = 0f;
        }

        /// <summary>
        /// Get/Set String Text
        /// </summary>
        public string StringText
        {
            get { return txtStr; }
            set { txtStr = value; }
        }

        /// <summary>
        /// Get StringPXSize
        /// </summary>
        public Vector2 StringPXSize
        {
            get { return txtSizePX; }
        }

        /// <summary>
        /// Get/Set String Colour
        /// </summary>
        public Color StringColour
        {
            get { return txtCol; }
            set { txtCol = value; }
        }

        /// <summary>
        /// Get/Set String Position
        /// </summary>
        public Vector2 StringPosition
        {
            get { return txtStringPos; }
            set { txtStringPos = value; }
        }

        /// <summary>
        /// Get/Set String Rotation
        /// </summary>
        public float StringRotation
        {
            get { return txtRotation; }
            set { txtRotation = value; }
        }

        /// <summary>
        /// Get/Set StringScale
        /// </summary>
        public float StringScale
        {
            get { return txtScale; }
            set { txtScale = value; }
        }

        /// <summary>
        /// Get/Set stringfont
        /// </summary>
        public SpriteFont StringFont
        {
            get { return txtFont; }
            set { txtFont = value; }
        }

        /// <summary>
        /// Get string size as a vector2 in pixels.
        /// </summary>
        public void GetStringSizePX()
        {
            if (txtStr != null)
                txtSizePX = txtFont.MeasureString(txtStr);
        }

    }
}
