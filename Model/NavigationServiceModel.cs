using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
