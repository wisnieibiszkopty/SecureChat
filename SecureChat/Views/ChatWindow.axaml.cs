using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using SecureChat.Context;
using SecureChat.Models;

namespace SecureChat.Views;

public partial class ChatWindow : Window
{
    private readonly UserContext _currentUser;
    private string _message;
 
    public string? SelectedRecipient { get; set; }
    public List<UserContext> Recipients { get; set; } = new();
    
    public ChatWindow(UserContext user)
    {
        InitializeComponent();
        _currentUser = user;
        Title = user.Username;
        
        LoadRecipients();

        AppContext.OnMessageReceived += HandleIncomingMessage;
        AppContext.OnUserRegistered += AddRecipientDynamic;

        DataContext = this;
    }

    private void LoadRecipients()
    {
        Recipients.Clear();
        var users = AppContext.Users.Where(u => u.Username != _currentUser.Username);
        foreach (var user in users)
        {
            Recipients.Add(user);
        }
    }
    
    public void SendMessage(object? sender, RoutedEventArgs e)
    {
        
    }

    private void HandleIncomingMessage(ChatMessage message)
    {
        
    }

    private void AddRecipientDynamic(UserContext newUser)
    {
        if (newUser.Username == _currentUser.Username)
        {
            return;
        }

        if (!Recipients.Select(r => r.Username).Contains(newUser.Username))
        {
            // TODO new user cannot be selected
            Recipients.Add(newUser);
            SelectedRecipient = newUser.Username;
        }
    }

    private void AppendToChat(string text)
    {
        
    }
    
    protected override void OnClosing(WindowClosingEventArgs e)
    {
        base.OnClosing(e);
        AppContext.OnMessageReceived -= HandleIncomingMessage;
        AppContext.OnUserRegistered -= AddRecipientDynamic;
    }
}