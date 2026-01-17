using System;
using System.Windows;
using System.Windows.Controls;
using MidiSampler.Models;
using MidiSampler.ViewModels;

namespace MidiSampler;

public partial class MainWindow : Window
{
    private readonly MainViewModel _viewModel;

    public MainWindow()
    {
        InitializeComponent();
        _viewModel = new MainViewModel();
        DataContext = _viewModel;
    }

    protected override void OnClosed(EventArgs e)
    {
        base.OnClosed(e);
        _viewModel.Cleanup();
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