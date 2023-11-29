using System.Windows.Navigation;

namespace Dictionary.Model
{
    public class NavigationServiceModel
    {
        static public NavigationService NavigationService { get; set; }

        static public void SetNavigationService(NavigationService service)
        {
            NavigationService = service;
        }

    }
}
