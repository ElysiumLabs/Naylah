using System.Threading.Tasks;

namespace Naylah.App.Navigation
{
    public interface INavigationFoward
    {
        Task<INavigationResult> GoFowardAsync(INavigationOptions options = null);

        Task<bool> CanGoBack { get; }
    }
}