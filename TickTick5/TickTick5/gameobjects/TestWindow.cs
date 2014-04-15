using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RamGecXNAControls;
using RamGecXNAControls.ExtendedControls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameManagement.gameobjects
{
    class TestWindow : GameEnvironment
    {
        Window myWindow;
        RamGecXNAControls.Button myButton;
        Label myLabel;
        
        public TestWindow()
        {
            myWindow = new Window(new Rectangle(10, 10, 300, 200), "Window Title");
            myButton = new RamGecXNAControls.Button(new Rectangle(50, 50, 0, 0), "My Button Text");
            myButton.Parent = myWindow;

            // assign OnClick event
            myButton.OnClick += (sender) => 
            {   
                myWindow.Visible = false;
            };

            // add Button to our Window
            myWindow.Controls.Add(myButton);

            // create the control (with AutoSize on)
            myLabel = new Label(new Rectangle(50, 50, 0, 0), "Hello World");

            // set some properties
            myLabel.TextColor = Color.Tomato;

            // add control to a Window
            myWindow.Controls.Add(myLabel);
            myWindow.Draw(spriteBatch);
        }
    }
}
