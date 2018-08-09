using System.Threading.Tasks;

namespace Naylah.App.Navigation
{
    public interface INavigationRefresh
    {
        Task<object> Refresh(object parameter);
    }
}