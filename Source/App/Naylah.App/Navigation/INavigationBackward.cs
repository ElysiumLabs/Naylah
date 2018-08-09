using System.Threading.Tasks;

namespace Naylah.App.Navigation
{
    public interface INavigationBackward
    {
        Task<INavigationResult> GoBackAsync(INavigationOptions options = null);

        bool CanGoBack { get; }
    }
}