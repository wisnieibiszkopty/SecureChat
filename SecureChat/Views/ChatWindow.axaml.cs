using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Avalonia.Controls;
using Avalonia.Interactivity;
using SecureChat.Context;
using SecureChat.Models;
using AppContext = SecureChat.Context.AppContext;

namespace SecureChat.Views;

public partial class ChatWindow : Window
{
    private readonly UserContext _currentUser;
 
    public UserContext? SelectedRecipient { get; set; }
    public ObservableCollection<UserContext> Recipients { get; set; } = new();
    
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
        string? message = Message.Text;
        if (message == null)
        {
            new ToastWindow("Message cannot be empty!").Show();
            return;
        }
        
        if (SelectedRecipient == null)
        {
            new ToastWindow("Recipient is not selected").Show();
            return;
        }

        var recipient = AppContext.Users.FirstOrDefault(u => u.Username == SelectedRecipient.Username);
        if (recipient == null)
        {
            new ToastWindow("Recipient does not exists!").Show();
            return;
        }

        var ciphertext = AppContext.CryptoService.EncryptAsymmetric(
            message,
            recipient.EncryptionKeys.PublicKey,
            _currentUser.EncryptionKeys.PrivateKey
        );
        
        var signature = AppContext.PkiService.SignData(
            Encoding.UTF8.GetBytes(message),
            _currentUser.SigningKeys.PrivateKey
        );
        
        var chatMessage = new ChatMessage
        {
            Sender = _currentUser.Username,
            Recipient = SelectedRecipient.Username,
            Ciphertext = ciphertext,
            Signature = signature
        };
        
        AppContext.SendMessage(chatMessage);
        
        Message.Text = "";
    }

    private void HandleIncomingMessage(ChatMessage message)
    {
        if (message.Recipient != _currentUser.Username)
        {
            return;
        }
        
        Debug.WriteLine(message.Ciphertext);
        Debug.WriteLine(message.Recipient);
    }

    private void AddRecipientDynamic(UserContext newUser)
    {
        if (newUser.Username == _currentUser.Username)
        {
            return;
        }

        if (!Recipients.Select(r => r.Username).Contains(newUser.Username))
        {
            Recipients.Add(newUser);
            SelectedRecipient = newUser;
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