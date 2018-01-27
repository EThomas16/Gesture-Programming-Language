using System.Windows.Controls;
using System.Windows.Media;
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace Gesture3
{
    //This class handles all the events of the kinect interactions
    class Event
    {
        //An instance of the the Kinect class
        Kinect kinect;
        //The total number of lines that the user has created
        int totalLines = 0;
        //All the statements that the user has created
        List<Button> statements = new List<Button>();
        //All the variables that the user has made
        List<Button> variables = new List<Button>();

        public Process cmd;

        //Constructor which saves the current instances of the kinect
        public Event(Kinect kinect)
        {
            this.kinect = kinect;
        }

        //Method that sets the position of the button parameter to the center of the hand circle
        public void buttonDragged(double hand_x, double hand_y, Button btn)
        {
            setButtonLocation(btn, hand_x - (btn.Width / 2), hand_y - (btn.Height / 2));
        }

        //Method that uses the coordinate parameters to relocate the button relative to the parent canvas
        public void setButtonLocation(Button current, double x, double y)
        {
            Canvas.SetLeft(current, x);
            Canvas.SetTop(current, y);
        }

        //Method to route the event handler based on which page is currently open
        public void clickHandler(Button btn)
        {
            //calls a method based on which window is currently open which is stored in the kinect class
            switch (kinect.currentWindow)
            {
                case "menu":
                    menuEvents(btn);
                    break;
                case "settings":
                    settingEvents(btn);
                    break;
                case "help":
                    helpEvents(btn);
                    break;
                case "main":
                    mainEvents(btn);
                    break;
                case "input":
                    inputEvents(btn);
                    break;
            }
        }

        //Method to handle the release of the draggable buttons
        public void buttonReleased(double x, double y, Button btn)
        {
            //Gets the bounding box of the code window
            Border border = kinect.code.codeBorder;
            double borderLeft = Canvas.GetLeft(border);
            double borderTop = Canvas.GetTop(border);
            double borderRight = borderLeft + border.Width;
            double borderBottom = borderTop + border.Height;

            //Checks if the hand is located within the code window
            if (y > borderTop && y < borderBottom && x > borderLeft && x < borderRight)
            {
                //Creates a new button/statement
                Button currentStatement = new Button();
                //Adds the new button the screen
                kinect.code.codeWindow.Children.Add(currentStatement);
                //Sets the grid location of the button
                Grid.SetRow(currentStatement, totalLines);
                Grid.SetColumn(currentStatement, 1);
                //Increments the number of lines that exist
                totalLines++;
                //Sets the values of the new button
                currentStatement.Name = "statement_" + totalLines;
                currentStatement.Height = 45;
                currentStatement.Width = 400;
                currentStatement.Content = "EMPTY";
                currentStatement.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Left;
                //Adds the button to the list of existing statements
                statements.Add(currentStatement);

                //Resets the location of all the draggable buttons
                defaultStatementLocations();

                //If the statement used was a variable, add the variable to its list
                if (btn.Content.ToString() == "Variable (4)")
                {
                    variables.Add(currentStatement);

                }
                //Open the inputs page to assign a name and value to the button
                kinect.openInput(btn.Content.ToString());
            }
            else //If the draggable button was not released over the code window, then reset their locations
            {
                defaultStatementLocations();
            }
        }

        //Method to reset the locations of all draggable buttons
        public void defaultStatementLocations()
        {
            //Sets the default location of each button that can be dragged using the coordinates stored in the Settings class
            setButtonLocation(kinect.code.btn_variable, Settings.var_x, Settings.var_y);
            setButtonLocation(kinect.code.btn_if, Settings.if_x, Settings.if_y);
            setButtonLocation(kinect.code.btn_print, Settings.print_x, Settings.print_y);
            setButtonLocation(kinect.code.btn_exp, Settings.exp_x, Settings.exp_y);
            setButtonLocation(kinect.code.btn_loop, Settings.loop_x, Settings.loop_y);
        }

        //Method to handle main menu page button presses
        public void menuEvents(Button btn)
        {
            //Get the button name that was pressed
            switch (btn.Content.ToString())
            {
                case "New":
                    //Open code page from the Kinect instance
                    kinect.openCode();
                    break;
                case "Open":
                    //Trigger the speech to text event to listen for the file name that the user wants to open
                    //This is the name of the event used in the Speech class
                    String result = "open page";
                    Boolean check = false;
                    kinect.speech.varResult = "open page";
                    //Call the method in the Speech class giving the above values as parameters
                    kinect.speech.speechToText(result, check);
                    break;
                case "Help":
                    //Open help page from the Kinect instance
                    kinect.openHelp();
                    break;
                case "Settings":
                    //Open settings page from the Kinect instance
                    kinect.openSettings();
                    break;
                case "Quit":
                    //Exit the program
                    kinect.quitProgram();
                    break;
            }
        }

        //Method to handle help page button presses
        public void helpEvents(Button btn)
        {
            //Get the button name that was pressed
            switch (btn.Content.ToString())
            {
                case "Tutorial": //Open the tutorial grid and hide the others
                    kinect.help.commandGrid.Visibility = System.Windows.Visibility.Collapsed;
                    kinect.help.gestureGrid.Visibility = System.Windows.Visibility.Collapsed;
                    kinect.help.btn_goLeft.Visibility = System.Windows.Visibility.Visible;
                    kinect.help.btn_goRight.Visibility = System.Windows.Visibility.Visible;
                    kinect.help.tutorialGif.Visibility = System.Windows.Visibility.Visible;
                    kinect.help.lbl_gifHeading.Visibility = System.Windows.Visibility.Visible;
                    break;
                case "Gestures": //Open the gesture grid and hide the others
                    kinect.help.commandGrid.Visibility = System.Windows.Visibility.Collapsed;
                    kinect.help.gestureGrid.Visibility = System.Windows.Visibility.Visible;
                    kinect.help.btn_goLeft.Visibility = System.Windows.Visibility.Collapsed;
                    kinect.help.btn_goRight.Visibility = System.Windows.Visibility.Collapsed;
                    kinect.help.tutorialGif.Visibility = System.Windows.Visibility.Collapsed;
                    kinect.help.lbl_gifHeading.Visibility = System.Windows.Visibility.Collapsed;
                    break;
                case "Voice Commands": //Open the voice command grid and hide the others
                    kinect.help.commandGrid.Visibility = System.Windows.Visibility.Visible;
                    kinect.help.gestureGrid.Visibility = System.Windows.Visibility.Collapsed;
                    kinect.help.btn_goLeft.Visibility = System.Windows.Visibility.Collapsed;
                    kinect.help.btn_goRight.Visibility = System.Windows.Visibility.Collapsed;
                    kinect.help.tutorialGif.Visibility = System.Windows.Visibility.Collapsed;
                    kinect.help.lbl_gifHeading.Visibility = System.Windows.Visibility.Collapsed;
                    break;
                case "<": //Navigate to the previous gif in the tuorial grid
                    kinect.help.previousGif();
                    break;
                case ">": //Navigate to the next gif in the tutorial grid
                    kinect.help.nextGif();
                    break;
                case "Back": //Go back to the main menu
                    kinect.openMenu();
                    break;
            }
        }

        //Method to handle settings page button presses
        public void settingEvents(Button btn)
        {
            //Has to be btn.Name rather than btn.Content because there are 2 sets of buttons name "normal" and "large"
            //Each of these buttons manipulates the settings in a specific way
            switch (btn.Name.ToString())
            {
                case "btn_Left":
                    //Switches the currently tracked hand to left
                    Settings.rightHandTracked = false;
                    //Remove the hand from the screen
                    kinect.removePerson(kinect.People[0]);
                    //Redraw the new hand
                    kinect.initialisePeople();
                    break;
                case "btn_Right":
                    //Switches the currently tracked hand to right
                    Settings.rightHandTracked = true;
                    //Remove the hand from the screen
                    kinect.removePerson(kinect.People[0]);
                    //Redraw the hand
                    kinect.initialisePeople();
                    break;
                case "btn_normText":
                    //Scales the font back down to the original scale
                    Settings.fontScale = 1.0;
                    break;
                case "btn_largeText":
                    //Upscales the font to increase readability
                    Settings.fontScale = 1.15;
                    break;
                case "btn_normScreen":
                    //Sets the screen size to be the default kinect camera resolution
                    Settings.windowHeight = 480;
                    Settings.windowWidth = 640;
                    kinect.frame.scaleSize();
                    break;
                case "btn_largeScreen":
                    //Sets the screen size to be 720p
                    Settings.windowHeight = 720;
                    Settings.windowWidth = 1280;
                    kinect.frame.scaleSize();
                    break;
                case "btn_back":
                    //Go back to the main menu
                    kinect.openMenu();
                    break;
                case "btn_apply":
                    //Settings options are applied automatically, makes users sure that settings are confirmed
                    kinect.frame.textToSpeech("Settings have been applied");
                    break;
            }
            //Reload the page with the new settings
            kinect.settings.init();
        }

        //Method to handle the code page events
        public void mainEvents(Button btn)
        {
            //Gets the name of the button to decide which event was triggered
            switch (btn.Content.ToString())
            {
                case "Back":
                    //Goes back to the main menu page
                    kinect.openMenu();
                    break;
                case "Save":
                    //Saves the current statements to a python file
                    Console.WriteLine("SAVING");
                    savePress();
                    break;
                case "Run":
                    //Runs the saved file
                    runPress();
                    break;
                case "Undo":
                    //Removes the last statement that was added
                    removeLastStatement();
                    break;
            }
        }

        //Method to handle the input page events
        public void inputEvents(Button btn)
        {
            //Sets the colour of the selected button
            Brush condIfColour = Brushes.LightGray;
            Brush bgChange = Brushes.Black;

            //Gets the button that was pressed
            switch (btn.Content.ToString())
            {
                case "Confirm":
                    //Call a method to add the statement to the code window
                    addStatement();
                    break;
                case "Decline": //Cancels the addition of a new statement
                    //Removes the new statement from the list
                    removeLastStatement();
                    //Go back to the code window
                    kinect.openCode();
                    break;
                case "Indent":
                    //If the current statement was going to be indented, set the button colour to black to show it will no longer be
                    if (kinect.input.shouldIndent)
                    {
                        btn.Background = Brushes.Black;
                    }
                    else
                    {
                        //Otherwise, set the colour to green, meaning the statement will now be indented
                        btn.Background = Brushes.DarkGreen;
                    }
                    //Inverts the shouldIndent value
                    kinect.input.shouldIndent = !kinect.input.shouldIndent;
                    break;
                case "+": //Sets the active operator to be + for the expressions grid
                    //Change the background colour
                    btn.Background = condIfColour;
                    kinect.input.btn_div.Background = bgChange;
                    kinect.input.btn_minus.Background = bgChange;
                    kinect.input.btn_mult.Background = bgChange;
                    break;
                case "-": //Sets the active operator to be - for the expressions grid
                    //Change the background colour
                    btn.Background = condIfColour;
                    kinect.input.btn_add.Background = bgChange;
                    kinect.input.btn_minus.Background = bgChange;
                    kinect.input.btn_mult.Background = bgChange;
                    break;
                case "/": //Sets the active operator to be / for the expressions grid
                    //Change the background colour
                    btn.Background = condIfColour;
                    kinect.input.btn_add.Background = bgChange;
                    kinect.input.btn_minus.Background = bgChange;
                    kinect.input.btn_mult.Background = bgChange;
                    break;
                case "*": //Sets the active operator to be * for the expressions grid
                    //Change the background colour
                    btn.Background = condIfColour;
                    kinect.input.btn_add.Background = bgChange;
                    kinect.input.btn_minus.Background = bgChange;
                    kinect.input.btn_div.Background = bgChange;
                    break;
                case "==": //Sets the remaining conditions to be that colour in the conditions grid
                    btn.Background = condIfColour;
                    kinect.input.btn_grtThan.Background = bgChange;
                    kinect.input.btn_grtThanEq.Background = bgChange;
                    kinect.input.btn_lessThan.Background = bgChange; ;
                    kinect.input.btn_lessThanEq.Background = bgChange;
                    break;
                case ">=":
                    btn.Background = condIfColour;
                    kinect.input.btn_lessThanEq.Background = bgChange;
                    kinect.input.btn_lessThan.Background = bgChange;
                    kinect.input.btn_equals.Background = bgChange;
                    kinect.input.btn_grtThan.Background = bgChange;
                    break;
                case "<=":
                    btn.Background = condIfColour;
                    kinect.input.btn_equals.Background = bgChange;
                    kinect.input.btn_lessThan.Background = bgChange;
                    kinect.input.btn_grtThan.Background = bgChange;
                    kinect.input.btn_grtThanEq.Background = bgChange;
                    break;
                case ">":
                    btn.Background = condIfColour;
                    kinect.input.btn_equals.Background = bgChange;
                    kinect.input.btn_grtThanEq.Background = bgChange;
                    kinect.input.btn_lessThan.Background = bgChange;
                    kinect.input.btn_lessThanEq.Background = bgChange;
                    break;
                case "<":
                    btn.Background = condIfColour;
                    kinect.input.btn_equals.Background = bgChange;
                    kinect.input.btn_lessThanEq.Background = bgChange;
                    kinect.input.btn_grtThanEq.Background = bgChange;
                    kinect.input.btn_grtThan.Background = bgChange;
                    break;
            }

        }

        //Method to populate the content of a new statement
        public void addStatement()
        {
            //Updates the text of the new statement to be a string returned from a function call
            statements[statements.Count - 1].Content = kinect.input.finalString();
            statements[statements.Count - 1].FontSize = 20 * Settings.fontScale;
            statements[statements.Count - 1].VerticalContentAlignment = System.Windows.VerticalAlignment.Top;
            Console.WriteLine("Statement: " + statements[statements.Count - 1].Content.ToString());
            //Go back to the code window
            Console.WriteLine("Opening code window...");
            kinect.openCode();
        }

        //Method to remove the last statement from the list
        public void removeLastStatement()
        {
            //Attempts to get the last statement from the list
            Button temp;
            try
            {
                temp = statements[statements.Count - 1];
            } catch(IndexOutOfRangeException e) //If there is no statement to remove, then stop running this method
            {
                Console.WriteLine("No statements exist");
                return;
            } catch (ArgumentOutOfRangeException arg_out)
            {
                Console.WriteLine("Invalid argument in event.cs: " + arg_out.Message + arg_out.Source);
                return;
            }

            //Removes the statement from the list and screen
            statements.Remove(temp);
            kinect.code.codeWindow.Children.Remove(temp);
            //Adds the line number as removing the button removes the line number
            kinect.code.addLineNum(statements.Count);
            //Decrements the total lines
            totalLines--;
            //Checks if the statement was a variable
            if (variables.Contains(temp))
            {
                //removes the variable from the list
                variables.Remove(temp);
                //If there are no variables, do not allow the user to do any condition checks or expressions
                if (variables.Count == 0)
                {
                    //kinect.code.btn_if.IsEnabled = false;
                    //kinect.code.btn_exp.IsEnabled = false;
                    //kinect.code.btn_loop.IsEnabled = false;
                }
            }

            //If there are no statements that exist, do not allow the user to remove anymore
            if (statements.Count == 0)
            {
                //disable button
            }
        }


        //Function to get all the variables from the list
        public List<String> getVariables()
        {
            List<String> temp = new List<string>();
            //Goes through each button in the list
            foreach (Button btn in variables)
            {
                String varName = btn.Content.ToString();
                //Only get the variable name
                varName = varName.Split(' ')[0];
                temp.Add(varName);
                Console.WriteLine(varName);
            }
            return temp;
        }

        //Method to save all the users statements into a python file using their project name
        public void savePress()
        {
            //Variable to hold the full python script
            String input = "";
            //Appends the import command and a try to the input to catch any runtime errors and display them to the user
            input += "try:\n";
            foreach (Button statement in statements)
            {
                //Loops through all of the buttons adding them to the file, ensuring to use proper Python indentation
                //Tabs are required for each line due to the try catch
                input += "    " + statement.Content.ToString() + "\n";
            }
            //Appends the exception in Python to the file, using the traceback module to print the specific error
            //Input is added at the end to hold the console to wait for a user input, to allow the users to see the result of their program
            //For Python 2, raw_input is used whereas for Python 3 input is used
            input += "except Exception as e:\n    print(e)\nraw_input (' ')";
            
            //Remove any spaces in the title for ease of accessing the file within a console command
            kinect.code.lbl_title.Content = kinect.code.lbl_title.Content.ToString().Replace(" ", "");
            //Sets the content of the title label to be the name of the file
            using (System.IO.FileStream fs = System.IO.File.Create(Settings.resourcePath + kinect.code.lbl_title.Content.ToString() + ".py"))
            {
                //Uses a file IO stream to save the current content of the input string as a Python file
                Byte[] info = new UTF8Encoding(true).GetBytes(input);
                //Writes the file of the given length
                fs.Write(info, 0, info.Length);
            }
        }

        //Method to run the program either created by the user or, if the default parameter is given a name, a premade script
        public void runPress(String filePath = "")
        {
            //This is the file name that will be given to the command window
            String cmdText = "";
            //Check if the default parameter has been given a proper name
            if (filePath == "")
            {
                //If the file path is empty, get the resource path and add the project name to it to make a python file by using the .py extension
                cmdText = Settings.resourcePath + kinect.code.lbl_title.Content + ".py";
            } else
            {
                //Otherwise, run a pre-made script with that name
                cmdText = Settings.resourcePath + filePath + ".py";
            }

            //Creates a new process and sets it to the cmd object
            cmd = new Process();
            //Use the projects own python install to ensure that any user can run it
            //Load the following command in the Process
            cmd.StartInfo = new ProcessStartInfo(Settings.resourcePath + @"/Python27/python.exe", cmdText)
            {
                //Use the following settings
                RedirectStandardOutput = false,
                UseShellExecute = false,
                CreateNoWindow = false
            };
            try
            {
                //Tries to start the process to run the python compiler
                Console.WriteLine("File path: " + cmdText);
                cmd.Start();
            }
            catch (Exception e)
            {
                //If that fails, an appropriate error is said using text to speech
                kinect.frame.textToSpeech("Error: project not found");
            }
        }
    }
}
