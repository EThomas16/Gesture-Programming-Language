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
using System.Diagnostics;
using System.Speech.Recognition;

namespace Test_Wpf {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
            this.Visibility = Visibility.Collapsed;
        }

        private void pythonCompiler() {
            String file_name = @"Insert output file path here";
            if (!System.IO.File.Exists(file_name)) {
                using (System.IO.FileStream fs = System.IO.File.Create(file_name)) {
                    Byte[] info = new UTF8Encoding(true).GetBytes("print(\"hello\")\nprint(\"new line\")\ninput(\"waiting\")");
                    fs.Write(info, 0, info.Length);
                }
            }


            String cmdText = "Insert output file path here";
            Process cmd = new Process();
            //Change this to correct directory
            cmd.StartInfo = new ProcessStartInfo(@"Insert Python path here", cmdText) {
                RedirectStandardOutput = false,
                UseShellExecute = false,
                CreateNoWindow = false
            };
            cmd.Start();
        }

        private void Python_Compile_Handler(object sender, RoutedEventArgs e) {
            pythonCompiler();
        }

        public void noDictionary() {
            SpeechRecognitionEngine recognizer = new SpeechRecognitionEngine();
            Grammar dictationGrammar = new DictationGrammar();
            recognizer.LoadGrammar(dictationGrammar);
            recognizer.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(speech_recognized);
            recognizer.SetInputToDefaultAudioDevice();
        }

        private void speech_recognized(object sender, SpeechRecognizedEventArgs e) {
            Debug.WriteLine("Speech recognition triggered");
            if (e.Result.Text != null) {
                MessageBox.Show(e.Result.Text);
            }

        }

        private void No_Dictionary_Handler(object sender, RoutedEventArgs e) {
            noDictionary();
        }

        private void Window_Switch(object sender, RoutedEventArgs e) {
            this.Visibility = Visibility.Hidden;
            TestWindow window = new TestWindow(this);
            window.Visibility = Visibility.Visible;
        }

    }
}
