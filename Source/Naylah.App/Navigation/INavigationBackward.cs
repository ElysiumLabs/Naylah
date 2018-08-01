using System.Threading.Tasks;

namespace Naylah.App.Navigation
{
    public interface INavigationBackward
    {
        Task<INavigationResult> GoBackAsync(INavigationOptions options = null);

        Task<bool> CanGoBack { get; }
    }
}