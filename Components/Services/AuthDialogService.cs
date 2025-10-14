namespace cse325_team7_project.Components.Services;

/// <summary>
/// Simple pub/sub coordinator so any component can request auth dialogs.
/// </summary>
public class AuthDialogService
{
    public event Action? LoginRequested;
    public event Action? RegistrationRequested;

    public void ShowLogin()
    {
        LoginRequested?.Invoke();
    }

    public void ShowRegistration()
    {
        RegistrationRequested?.Invoke();
    }
}
