using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestMVVMDemo.Models;

namespace TestMVVMDemo.ViewModels
{
    public class FirstViewModel : WorkspaceViewModel
    {
        private const string DisplayViewName = "FirstView";

        private readonly InfoModel _info;

        public FirstViewModel()
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
