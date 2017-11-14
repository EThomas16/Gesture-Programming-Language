SpeechRecognitionEngine recognizer = new SpeechRecognitionEngine();
Grammar dictationGrammar = new DictationGrammar();
recognizer.LoadGrammar(dictationGrammar);
try
{
  recognizer.SetInputToDefaultAudioDevice();
  RecognitionResult result = recognizer.Recognize();
  if (result != null)
      MessageBox.Show(result.Text);
}
catch (InvalidOperationException exception)
{
  MessageBox.Show(exception.Message, exception.Source);
}
finally
{
  recognizer.UnloadAllGrammars();
}
