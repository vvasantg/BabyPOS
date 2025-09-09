using BabyPOS_Web2.Application.DTOs;
using BabyPOS_Web2.Application.Services;
using System.ComponentModel;

namespace BabyPOS_Web2.Presentation.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private readonly IAuthService _authService;
        private string _username = string.Empty;
        private string _password = string.Empty;
        private string _errorMessage = string.Empty;
        private bool _isLoading = false;

        public LoginViewModel(IAuthService authService)
        {
            _authService = authService;
        }

        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged(nameof(Username));
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }

        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                _isLoading = value;
                OnPropertyChanged(nameof(IsLoading));
            }
        }

        public async Task<bool> LoginAsync()
        {
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
            {
                ErrorMessage = "Please enter both username and password.";
                return false;
            }

            IsLoading = true;
            ErrorMessage = string.Empty;

            try
            {
                var loginDto = new LoginDto
                {
                    Username = Username,
                    Password = Password
                };

                var result = await _authService.LoginAsync(loginDto);

                if (result.IsSuccess)
                {
                    ErrorMessage = string.Empty;
                    return true;
                }
                else
                {
                    ErrorMessage = result.ErrorMessage;
                    return false;
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Login failed: {ex.Message}";
                return false;
            }
            finally
            {
                IsLoading = false;
            }
        }

        public void ClearForm()
        {
            Username = string.Empty;
            Password = string.Empty;
            ErrorMessage = string.Empty;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
