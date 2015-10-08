using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Data;
using TestMVVMDemo.Command;

namespace TestMVVMDemo.ViewModels
{
    public class MainWindowViewModel : WorkspaceViewModel
    {
        private const string DisplayViewName = "MainWindowView";

        /// <summary>
        /// 工作区（单视图）
        /// </summary>
        private WorkspaceViewModel _workspaceSingle;

        /// <summary>
        /// 历史视图（单视图）
        /// </summary>
        private ObservableCollection<WorkspaceViewModel> _workspaceStory; 

        /// <summary>
        /// 工作区（多视图）
        /// </summary>
        private ObservableCollection<WorkspaceViewModel> _workspaceMulti; 

        /// <summary>
        /// 显示视图一命令
        /// </summary>
        public DelegateCommand ShowFirstViewCmd { get; set; }

        /// <summary>
        /// 显示视图二命令
        /// </summary>
        public DelegateCommand ShowSecondViewCmd { get; set; }

        /// <summary>
        /// 创建视图一
        /// </summary>
        public DelegateCommand CreateFirstViewCmd { get; set; }

        /// <summary>
        /// 创建视图二
        /// </summary>
        public DelegateCommand CreateSecondViewCmd { get; set; }

        /// <summary>
        /// 显示上一个视图
        /// </summary>
        public DelegateCommand ShowPreviousViewCmd { get; set; }

        /// <summary>
        /// 显示下一个视图
        /// </summary>
        public DelegateCommand ShowNextViewCmd { get; set; }

        public MainWindowViewModel()
        {
            base.DisplayName = DisplayViewName;

            //单视图命令
            ShowFirstViewCmd = new DelegateCommand();
            ShowFirstViewCmd.ExecuteCommand = new Action<object>(ShowFirstView);

            ShowSecondViewCmd = new DelegateCommand();
            ShowSecondViewCmd.ExecuteCommand = new Action<object>(ShowSecondView);

            //多视图命令
            CreateFirstViewCmd = new DelegateCommand();
            CreateFirstViewCmd.ExecuteCommand = new Action<object>(CreateFirstView);

            CreateSecondViewCmd = new DelegateCommand();
            CreateSecondViewCmd.ExecuteCommand = new Action<object>(CreateSecondView);

            ShowPreviousViewCmd = new DelegateCommand();
            ShowPreviousViewCmd.ExecuteCommand = new Action<object>(ShowPreviousView);

            ShowNextViewCmd = new DelegateCommand();
            ShowNextViewCmd.ExecuteCommand = new Action<object>(ShowNextView);
        }

        /// <summary>
        /// 工作区显示单视图
        /// </summary>
        public WorkspaceViewModel WorkspaceSingle
        {
            get
            {
                if (_workspaceSingle == null)
                {
                    _workspaceSingle = new WorkspaceViewModel();
                }

                return _workspaceSingle;
            }
            set
            {
                _workspaceSingle = value;
                OnPropertyChanged("WorkspaceSingle");
            }
        }

        /// <summary>
        /// 工作区显示多视图
        /// </summary>
        public ObservableCollection<WorkspaceViewModel> WorkspaceMulti
        {
            get
            {
                if (_workspaceMulti == null)
                {
                    _workspaceMulti = new ObservableCollection<WorkspaceViewModel>();
                }
                return _workspaceMulti;
            }
        }


        /// <summary>
        /// 显示视图一
        /// </summary>
        private void ShowFirstView(object obj)
        {
            if (_workspaceStory == null)
            {
                _workspaceStory = new ObservableCollection<WorkspaceViewModel>();
            }

            var model =
                this._workspaceStory.FirstOrDefault(vm => vm is FirstViewModel)
                    as FirstViewModel;

            if (model == null)
            {
                model = new FirstViewModel();
                _workspaceStory.Add(model);
            }

            WorkspaceSingle = model;
        }

        /// <summary>
        /// 显示视图二命令
        /// </summary>
        private void ShowSecondView(object obj)
        {
            if (_workspaceStory == null)
            {
                _workspaceStory = new ObservableCollection<WorkspaceViewModel>();
            }

            var model =
                this._workspaceStory.FirstOrDefault(vm => vm is SecondViewModel)
                    as SecondViewModel;

            if (model == null)
            {
                model = new SecondViewModel();
                _workspaceStory.Add(model);
            }

            WorkspaceSingle = model;
        }

        /// <summary>
        /// 创建视图一，并显示
        /// </summary>
        private void CreateFirstView(object obj)
        {
            var model = new FirstViewModel();
            WorkspaceMulti.Add(model);
            ShowCurrentView(model);
        }

        /// <summary>
        /// 创建视图二
        /// </summary>
        private void CreateSecondView(object obj)
        {
            var model = new SecondViewModel();
            WorkspaceMulti.Add(model);
            ShowCurrentView(model);
        }

        /// <summary>
        /// 显示上一个视图
        /// </summary>
        private void ShowPreviousView(object obj)
        {
            ICollectionView collectionView = CollectionViewSource.GetDefaultView(this.WorkspaceMulti);
            if (collectionView != null)
            {
                collectionView.MoveCurrentToPrevious();
            }
        }

        /// <summary>
        /// 显示下一个视图
        /// </summary>
        private void ShowNextView(object obj)
        {
            ICollectionView collectionView = CollectionViewSource.GetDefaultView(this.WorkspaceMulti);
            if (collectionView != null)
            {
                collectionView.MoveCurrentToNext();
            }
        }

        /// <summary>
        /// 显示当前默认视图
        /// </summary>
        /// <param name="workspace"></param>
        private void ShowCurrentView(WorkspaceViewModel workspace)
        {
            Debug.Assert(this.WorkspaceMulti.Contains(workspace));

            ICollectionView collectionView = CollectionViewSource.GetDefaultView(this.WorkspaceMulti);
            if (collectionView != null)
            {
                collectionView.MoveCurrentTo(workspace);
            }
        }
    }
}
