using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using SecureChat.Context;

namespace SecureChat.Views;

public partial class ChatWindow : Window
{
    public ChatWindow(UserContext user)
    {
        InitializeComponent();
    }
}