using System.Windows;
using System.Windows.Controls;
using MidiSampler.Models;
using MidiSampler.ViewModels;

namespace MidiSampler;

public partial class PadEditorWindow : Window
{
    private readonly MainViewModel _viewModel;

    public PadEditorWindow(MainViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        DataContext = viewModel;
    }

    private void SelectAudio_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.DataContext is PadMapping padMapping)
        {
            _viewModel.SelectAudioFileForPadCommand.Execute(padMapping);
        }
    }

    private void LearnMidi_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.DataContext is PadMapping padMapping)
        {
            _viewModel.StartLearningMidiNoteCommand.Execute(padMapping);
        }
    }
}
