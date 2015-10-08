using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using TestMVVMDemo.Models;

namespace TestMVVMDemo.ViewModels
{
    public class SecondViewModel : WorkspaceViewModel
    {
        private const string DisplayViewName = "SecondView";

        private readonly InfoModel _info;
        public SecondViewModel()
        {
            base.DisplayName = DisplayViewName;

            if (_info == null)
            {
                _info = new InfoModel();
            }
            _info.Name = DisplayViewName;
        }

        public string Name
        {
            get { return _info.Name; }
        }
    }
}
