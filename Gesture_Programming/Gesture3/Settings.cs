using System;

namespace Gesture3
{
    //Static class so that any other class or object can reference its values
    static class Settings
    {
        //The users settings that are changed in the settings page and used in a variety of other classes
        //The resource path stores the path to the resources folder, set in the main window, for use whenever files need to be read from or written to
        public static String resourcePath;
        //Stores the state of the hand tracking
        public static Boolean rightHandTracked = true;
        //The volume of the text to speech can be adjusted using speech commands
        public static int ttsVolume = 100;
        //The font scale and window sizing can also be adjusted in the settings menu
        public static double fontScale = 1.0;
        public static int windowWidth = 640;
        public static int windowHeight = 480;

        //The starting locations of the draggable elements in the CodeWindow class
        //Used to reset the locations when they are released
        public static double var_x;
        public static double var_y;
        public static double if_x;
        public static double if_y;
        public static double exp_x;
        public static double exp_y;
        public static double loop_x;
        public static double loop_y;
        public static double print_x;
        public static double print_y;
    }
}
