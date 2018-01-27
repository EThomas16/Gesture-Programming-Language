using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Gesture3
{
    /// <summary>
    /// Interaction logic for InputPage.xaml
    /// </summary>
    public partial class InputPage : Page
    {
        //Stores the active grid as a string for checking which grid is currently being used
        String activeGrid;
        //These static variables are used to set the current speech to text result
        static String speechResult = "";
        static String speechVal = "";
        static String speechSet = "";
        public List<ComboBoxItem> items = new List<ComboBoxItem>();
        //Global check for if the current statement should be indented in the code window
        public bool shouldIndent = false;

        //Basic constructor for the page
        public InputPage()
        {
            InitializeComponent();
        }

        public void init()
        {
            //Initialises the width and height of the buttons, as well as the scaling of the font
            double buttonWidth = Settings.windowWidth / 3;
            double buttonHeight = Settings.windowHeight / 8;
            double fs = Settings.fontScale;
            //Initialises all of the key buttons
            Formatting.buttonInit(btn_indent, Settings.windowWidth - (buttonWidth * 0.75), buttonHeight / 2, buttonWidth / 2, buttonHeight, 24 * fs, Brushes.White, Brushes.Black);
            Formatting.buttonInit(btn_confirm, (Settings.windowWidth / 2) - (buttonWidth), Settings.windowHeight - (buttonHeight * 2), buttonWidth, buttonHeight, 36 * fs, Brushes.Black, Brushes.White);
            Formatting.buttonInit(btn_decline, (Settings.windowWidth / 2), Settings.windowHeight - (buttonHeight * 2), buttonWidth, buttonHeight, 36 * fs, Brushes.Black, Brushes.White);

            //Print grid initialisation
            Formatting.textBoxInit(txt_print, (Settings.windowWidth / 2) - (buttonWidth), buttonHeight * 2, btn_decline.Width * 2, buttonHeight, 40 * fs, HorizontalAlignment.Left);
            Canvas.SetLeft(txt_printBorder, (Settings.windowWidth / 2) - (buttonWidth));
            Canvas.SetTop(txt_printBorder, buttonHeight * 2);
            lbl_printHelp.Content = "Say \"Start Text\" to \nenter content";
            Formatting.labelInit(lbl_printHelp, (Settings.windowWidth / 2) - (buttonWidth * 1.5), 0, btn_decline.Width * 2, buttonHeight * 2, 40 * fs, Brushes.White, Brushes.Black);

            cmb_useVars.Width = buttonWidth;
            cmb_useVars.Height = buttonHeight * 1.3;
            Canvas.SetLeft(cmb_useVars, Canvas.GetLeft(txt_print));
            Canvas.SetTop(cmb_useVars, Canvas.GetTop(txt_print) + (buttonHeight * 1.3));

            //Provides labels to tell the user how to set variable name and value
            lbl_varHelp1.Content = "Use\n\"Set\"";
            lbl_varHelp2.Content = "Use\n\"Value\"";
            //Initialises the text boxes and labels
            Formatting.textBoxInit(txt_varSet, buttonWidth / 1.7, (Settings.windowHeight / 2) - (buttonHeight), buttonWidth / 1.5, buttonHeight, 28 * fs);
            Canvas.SetLeft(txt_var1Border, buttonWidth / 1.7);
            Canvas.SetTop(txt_var1Border, (Settings.windowHeight / 2) - (buttonHeight));
            Formatting.labelInit(lbl_equals, (Settings.windowWidth / 2) - (buttonWidth / 5), (Settings.windowHeight / 2) - (buttonHeight), buttonWidth / 5, buttonHeight, 28 * fs, Brushes.White, Brushes.Black);
            Formatting.textBoxInit(txt_varVal, Canvas.GetLeft(lbl_equals) + lbl_equals.Width, (Settings.windowHeight / 2) - (buttonHeight), buttonWidth, buttonHeight, 28 * fs);
            Canvas.SetLeft(txt_var2Border, Canvas.GetLeft(lbl_equals) + lbl_equals.Width);
            Canvas.SetTop(txt_var2Border, (Settings.windowHeight / 2) - (buttonHeight));

            Formatting.labelInit(lbl_varHelp1, Canvas.GetLeft(txt_varSet), Canvas.GetTop(txt_varSet) - (txt_varSet.Height * 2), txt_varSet.Width, txt_varSet.Height * 2, 28 * fs, Brushes.White, Brushes.Black);
            Formatting.labelInit(lbl_varHelp2, Canvas.GetLeft(txt_varVal), Canvas.GetTop(txt_varVal) - (txt_varVal.Height * 2), txt_varVal.Width /2, txt_varVal.Height * 2, 28 * fs, Brushes.White, Brushes.Black, HorizontalAlignment.Left);

            //Condition grid initialisation
            cmb_useConds.Width = buttonWidth;
            cmb_useConds.Height = buttonHeight;
            Canvas.SetLeft(cmb_useConds, buttonWidth / 5);
            Canvas.SetTop(cmb_useConds, (Settings.windowHeight / 2) - (buttonHeight));
            Formatting.buttonInit(btn_equals, (Settings.windowWidth / 2) - (buttonWidth / 5), (Settings.windowHeight / 2) - (buttonHeight), buttonWidth / 5, buttonHeight, 28 * fs, Brushes.White, Brushes.Black);
            Formatting.buttonInit(btn_grtThan, (Settings.windowWidth / 2) - (buttonWidth / 5), Canvas.GetTop(btn_equals) - buttonHeight, buttonWidth / 5, buttonHeight, 28 * fs, Brushes.White, Brushes.Black);
            Formatting.buttonInit(btn_grtThanEq, (Settings.windowWidth / 2) - (buttonWidth / 5), Canvas.GetTop(btn_equals) - (buttonHeight * 2), buttonWidth / 5, buttonHeight, 28 * fs, Brushes.White, Brushes.Black);
            Formatting.buttonInit(btn_lessThanEq, (Settings.windowWidth / 2) - (buttonWidth / 5), Canvas.GetTop(btn_equals) + buttonHeight, buttonWidth / 5, buttonHeight, 28 * fs, Brushes.White, Brushes.Black);
            Formatting.buttonInit(btn_lessThan, (Settings.windowWidth / 2) - (buttonWidth / 5), Canvas.GetTop(btn_equals) + (buttonHeight * 2), buttonWidth / 5, buttonHeight, 28 * fs, Brushes.White, Brushes.Black);
            Formatting.textBoxInit(txt_condVal, buttonWidth, (Settings.windowHeight / 2) - (buttonHeight), buttonWidth, buttonHeight, 28 * fs);
            Canvas.SetLeft(txt_cond2Border, Canvas.GetLeft(btn_equals) + buttonWidth / 4);
            Canvas.SetTop(txt_cond2Border, (Settings.windowHeight / 2) - (buttonHeight));
            //Set thes the operator type
            btn_equals.Background = Brushes.LightGray;
            //Tells the user to use start text command to set values
            lbl_condHelp.Content = "Use\n\"Start Text\"";
            Formatting.labelInit(lbl_condHelp, Canvas.GetLeft(txt_cond2Border), Canvas.GetTop(txt_cond2Border) - (txt_cond2Border.Height), txt_cond2Border.Width, txt_cond2Border.Height * 2, 28 * fs, Brushes.White, Brushes.Black);

            //Expression grid initialisation
            cmb_useExp.Width = buttonWidth * 0.7;
            cmb_set1Exp.Width = buttonWidth * 0.7;
            cmb_set2Exp.Width = buttonWidth * 0.7;
            cmb_useExp.Height = buttonHeight;
            cmb_set1Exp.Height = buttonHeight;
            cmb_set2Exp.Height = buttonHeight;
            Canvas.SetLeft(cmb_useExp, buttonWidth / 8);
            Canvas.SetTop(cmb_useExp, (Settings.windowHeight / 2) - (buttonHeight));
            Formatting.labelInit(lbl_equals2, Canvas.GetLeft(cmb_useExp) + cmb_useExp.Width, Canvas.GetTop(cmb_useExp), buttonWidth * 0.2, buttonHeight, 28 * fs, Brushes.White, Brushes.Black);

            Canvas.SetLeft(cmb_set1Exp, Canvas.GetLeft(lbl_equals2) + buttonWidth * 0.2);
            Canvas.SetTop(cmb_set1Exp, (Settings.windowHeight / 2) - (buttonHeight));
            //Initialisation for the operator buttons
            Formatting.buttonInit(btn_add, Canvas.GetLeft(cmb_set1Exp) + cmb_set1Exp.Width * 1.1, (Settings.windowHeight / 2) - (buttonHeight), buttonWidth * 0.3, buttonHeight, 28 * fs, Brushes.Black, Brushes.White);
            Formatting.buttonInit(btn_mult, Canvas.GetLeft(cmb_set1Exp) + cmb_set1Exp.Width * 1.1, (Settings.windowHeight / 2) - (buttonHeight * 2), buttonWidth * 0.3, buttonHeight, 28 * fs, Brushes.Black, Brushes.White);
            Formatting.buttonInit(btn_div, Canvas.GetLeft(cmb_set1Exp) + cmb_set1Exp.Width * 1.1, (Settings.windowHeight / 2) - (buttonHeight * 3), buttonWidth * 0.3, buttonHeight, 28 * fs, Brushes.Black, Brushes.White);
            Formatting.buttonInit(btn_minus, Canvas.GetLeft(cmb_set1Exp) + cmb_set1Exp.Width * 1.1, (Settings.windowHeight / 2), buttonWidth * 0.3, buttonHeight, 28 * fs, Brushes.Black, Brushes.White);
            Canvas.SetLeft(cmb_set2Exp, Canvas.GetLeft(btn_add) + btn_add.Width * 1.1);
            Canvas.SetTop(cmb_set2Exp, (Settings.windowHeight / 2) - (buttonHeight));
            btn_add.Background = Brushes.LightGray;

        }

        public void openGrid(String name)
        {
            //Handles the opening of different input pages for each statement
            expGrid.Visibility = Visibility.Collapsed;
            condGrid.Visibility = Visibility.Collapsed;
            varGrid.Visibility = Visibility.Collapsed;
            printGrid.Visibility = Visibility.Collapsed;
            cmb_set1Exp.Visibility = Visibility.Collapsed;
            cmb_set2Exp.Visibility = Visibility.Collapsed;
            cmb_useConds.Visibility = Visibility.Collapsed;
            cmb_useExp.Visibility = Visibility.Collapsed;
            cmb_useVars.Visibility = Visibility.Collapsed;
            //Uses the current grid in a switch to customise the grid to the selected statement
            //And then alters the visibility of specific grid elements
            activeGrid = name;
            switch (name)
            {
                case "Expression (5)":
                    expGrid.Visibility = Visibility.Visible;
                    cmb_useExp.Visibility = Visibility.Visible;
                    cmb_set1Exp.Visibility = Visibility.Visible;
                    cmb_set2Exp.Visibility = Visibility.Visible;
                    break;
                case "Variable (4)":
                    varGrid.Visibility = Visibility.Visible;
                    cmb_useVars.Visibility = Visibility.Visible;
                    break;
                case "Print (2)":
                    printGrid.Visibility = Visibility.Visible;
                    cmb_useVars.Visibility = Visibility.Visible;
                    break;
                case "Loop (3)":
                case "If (1)":
                    cmb_useConds.Visibility = Visibility.Visible;
                    condGrid.Visibility = Visibility.Visible;
                    break;
            }
        }

        public String finalString()
        {
            //This method handles the appending of the input content to the code window in the correct format for Python
            String statement = "";
            if (shouldIndent)
            {
                //If the indent button is selected, adds a tab to the statement
                statement += "    ";
            }
            switch (activeGrid)
            {
                case "Expression (5)":
                    //Gets the current operator of the expression
                    String op = "";
                    if (btn_add.Background == Brushes.LightGray)
                    {
                        op = " + ";
                    }
                    else if (btn_minus.Background == Brushes.LightGray)
                    {
                        op = " - ";
                    }
                    else if (btn_mult.Background == Brushes.LightGray)
                    {
                        op = " * ";
                    }
                    else if (btn_div.Background == Brushes.LightGray)
                    {
                        op = " / ";
                    }
                    //Combines the combo box content and operators to create the expression
                    statement += ((ComboBoxItem)cmb_useExp.SelectedItem).Content.ToString() + " = " + ((ComboBoxItem)cmb_set1Exp.SelectedItem).Content.ToString() + op + ((ComboBoxItem)cmb_set2Exp.SelectedItem).Content.ToString();
                    break;
                case "Variable (4)":
                    //If the variable has been set...
                    if (speechSet != "" && speechVal != "")
                    {
                        //Sets the content of the text box as the spoken values
                        txt_varSet.Text = speechSet;
                        //And calls the typeCheck method to verify whether the variable's content is an integer, boolean or string
                        typeCheck(speechVal, txt_varVal);
                    }
                    Console.WriteLine("Speech set: " + txt_varSet.Text + " Speech val: " + txt_varVal.Text);
                    //Then adds the relevant information to the current statement
                    statement += txt_varSet.Text + " = " + txt_varVal.Text;
                    break;

                case "Print (2)":
                    //If the speech result is not an empty string...
                    if (speechResult != "")
                    {
                        //Checks the type of value given to categorise it
                        typeCheck(speechResult, txt_print);
                    }
                    //And then sets the statement to be a print statement
                    statement += "print(" + txt_print.Text;
                    Console.WriteLine("Print speech result is: " + txt_print.Text);
                    if (((ComboBoxItem)cmb_useVars.SelectedItem).Content.ToString() != "{No Var}")
                    {
                        //For appending any variables uses a comma
                        statement += ", " + ((ComboBoxItem)cmb_useVars.SelectedItem).Content.ToString();
                    }
                    //Finalises the print statement by closing the bracket
                    statement += ")";
                    break;

                case "Loop (3)":
                    //For loops and if statements, the operator is acquired from the buttons
                    op = getConditionalOperator();
                    if (speechResult != "")
                    {
                        typeCheck(speechResult, txt_condVal);
                    }
                    Console.WriteLine("Received result from speech, result is: " + txt_condVal.Text);
                    //Formats the statement appropriately to ensure it compiles
                    statement += "while " + ((ComboBoxItem)cmb_useConds.SelectedItem).Content.ToString() + op + txt_condVal.Text + ":";
                    break;

                case "If (1)":
                    op = getConditionalOperator();
                    if (speechResult != "")
                    {
                        typeCheck(speechResult, txt_condVal);                      
                    }
                    Console.WriteLine("Received result from speech, result is: " + txt_condVal.Text);
                    //And then passes it to be stored in the code window
                    statement += "if " + ((ComboBoxItem)cmb_useConds.SelectedItem).Content.ToString() + op + txt_condVal.Text + ":";

                    break;
            }
            return statement;
        }

        public void typeCheck(String speechResult, TextBox txt)
        {
            //This method is used to check the type of value being inputted by the user, taking the current text box and the result from speech
            switch (speechResult)
            {
                case "True":
                case "False":
                    //If the result is a boolean, set the text box's content as the pure boolean
                    txt.Text = speechResult;
                    break;

                default:
                    int speechNum;
                    //Checks if the result is an integer (digit)
                    Boolean isNumeric = int.TryParse(speechResult, out speechNum);
                    if (isNumeric)
                    {                       
                        //If it is a digit, then the value is converted to a string and passed without speech marks
                        txt.Text = speechNum.ToString();
                    }
                    else
                    {
                        //Otherwise, speech marks are added for formatting for strings
                        txt.Text = "\"" + speechResult + "\"";
                    }
                    break;
            }
        }

        public String getConditionalOperator()
        {
            //This method acquires the current conditional operator from the given buttons
            //Uses the colour of the button to check if it is active
            String temp = "";
            if (btn_equals.Background == Brushes.LightGray)
            {
                temp = " == ";
            }
            else if (btn_grtThan.Background == Brushes.LightGray)
            {
                temp = " > ";
            }
            else if (btn_grtThanEq.Background == Brushes.LightGray)
            {
                temp = " >=";
            }
            else if (btn_lessThanEq.Background == Brushes.LightGray)
            {
                temp = " <= ";
            }
            else if (btn_lessThan.Background == Brushes.LightGray)
            {
                temp = " < ";
            }else if (btn_add.Background == Brushes.LightGray)
            {
                temp = " + ";
            }
            else if (btn_div.Background == Brushes.LightGray)
            {
                temp = " / ";
            }
            else if (btn_minus.Background == Brushes.LightGray)
            {
                temp = " - ";
            }
            else if (btn_mult.Background == Brushes.LightGray)
            {
                temp = " * ";
            }
            return temp;
        }

        public void populateVariables(List<String> var)
        {
            //Used to populate the various combo boxes for selecting variables
            ComboBox[] cmbs = { cmb_useVars, cmb_useConds, cmb_useExp, cmb_set2Exp, cmb_set1Exp };
            foreach (ComboBox cmb in cmbs)
            {
                //For every combo box, it is set to visible and initialised with the correct sizing and font size
                if (cmb.Visibility == Visibility.Visible)
                {
                    cmb.FontSize = 28 * Settings.fontScale;
                    ComboBoxItem cmb_item = new ComboBoxItem();
                    cmb_item.Name = "";
                    cmb_item.Content = "{No Var}";
                    cmb_item.FontSize = 28 * Settings.fontScale;
                    cmb_item.Height = 80;
                    cmb.Items.Add(cmb_item);
                    cmb.SelectedItem = cmb_item;
                    foreach (String str in var)
                    {
                        //For every variable found a new combo box is made to add to the list
                        cmb_item = new ComboBoxItem();
                        cmb_item.Name = "";
                        //And the content is set as the current variable name
                        cmb_item.Content = str;
                        cmb_item.FontSize = 28 * Settings.fontScale;
                        cmb_item.Height = 80;
                        cmb.Items.Add(cmb_item);

                        items.Add(cmb_item);
                    }
                }
            }
        }

        public List<ComboBox> getComboBoxElements()
        {
            //Method used to get all of the combo boxes when required
            List<ComboBox> temp = new List<ComboBox>();
            //Takes the active grid and adds the combo boxes to the temporary list
            switch (activeGrid)
            {
                case "Expression (5)":
                    temp.Add(cmb_useExp);
                    temp.Add(cmb_set1Exp);
                    temp.Add(cmb_set2Exp);
                    break;
                case "Variable (4)":


                    break;
                case "Print (2)":
                    temp.Add(cmb_useVars);
                    break;
                case "Loop (3)":
                case "If (1)":
                    temp.Add(cmb_useConds);
                    break;
            }
            //Returns the temporary list of combo boxes to be used elsewhere
            return temp;
        }

        public List<Button> getInteractableElements()
        {
            //Performs the same function as the above method, to get the current active buttons
            List<Button> temp = new List<Button>();
            //Always adds confirm, decline and indent as they are on every input page
            temp.Add(btn_confirm);
            temp.Add(btn_decline);
            temp.Add(btn_indent);
            //With the current grid being checked...
            switch (activeGrid)
            {
                //... the correct buttons for each page are they added
                case "Expression (5)":
                    temp.Add(btn_add);
                    temp.Add(btn_mult);
                    temp.Add(btn_minus);
                    temp.Add(btn_div);
                    break;
                case "Variable (4)":
                case "Print (2)":
                    break;
                case "Loop (3)":
                case "If (1)":
                    temp.Add(btn_equals);
                    temp.Add(btn_grtThan);
                    temp.Add(btn_grtThanEq);
                    temp.Add(btn_lessThan);
                    temp.Add(btn_lessThanEq);
                    break;
            }
            //And returns a list of the currently active elements
            return temp;
        }

        public static void setSpeechResult(String result)
        {
            //Called from speech to set the current result for use in setting values of statements
            speechResult = result;
            Console.WriteLine("InputPage speech result is: " + speechResult);
        }

        public static void setVarResult(String varSet)
        {
            //Specifically used for setting the variable names in the input page
            speechSet = varSet;
            Console.WriteLine("Variable set: " + speechSet);
        }

        public static void valVarResult(String varVal)
        {
            //Specifically used for setting the variable values in the input page
            speechVal = varVal;
            Console.WriteLine("Variable value set: " + speechVal);
        }
    }
}
