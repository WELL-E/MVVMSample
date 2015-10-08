using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestMVVMDemo.Command;
using TestMVVMDemo.ViewModels;
using System.Windows.Data;

namespace UnitTest
{
    [TestClass]
    public class MainWindowViewModelTests
    {
        public MainWindowViewModelTests()
        {
        }

        private TestContext _testContextInstance;
        public TestContext TestContext
        {
            get
            {
                return _testContextInstance;
            }
            set
            {
                _testContextInstance = value;
            }
        }

        [TestMethod]
        public void TestShowFirstView()
        {
            MainWindowViewModel target = new MainWindowViewModel();
            target.ShowFirstViewCmd = new DelegateCommand();
            target.ShowFirstViewCmd.ExecuteCommand = o =>
            {
                target.WorkspaceSingle = new FirstViewModel();
            };
            target.ShowFirstViewCmd.Execute(null);
            Assert.IsTrue(target.WorkspaceSingle is FirstViewModel, "Invalid current item.");
        }

        [TestMethod]
        public void TestShowSecondView()
        {
            MainWindowViewModel target = new MainWindowViewModel();
            target.ShowFirstViewCmd = new DelegateCommand();
            target.ShowFirstViewCmd.ExecuteCommand = o =>
            {
                target.WorkspaceSingle = new SecondViewModel();
            };
            target.ShowFirstViewCmd.Execute(null);
            Assert.IsTrue(target.WorkspaceSingle is SecondViewModel, "Invalid current item.");
        }

        [TestMethod]
        public void TestCreateFirstView()
        {
            MainWindowViewModel target = new MainWindowViewModel();
            target.ShowFirstViewCmd = new DelegateCommand();
            target.ShowFirstViewCmd.ExecuteCommand = o =>
            {
                target.WorkspaceMulti.Add(new FirstViewModel());
            };
            target.ShowFirstViewCmd.Execute(null);

            var collectionView = CollectionViewSource.GetDefaultView(target.WorkspaceMulti);
            Assert.IsTrue(collectionView.CurrentItem is FirstViewModel, "Invalid current item.");
        }

        [TestMethod]
        public void TestCreateSecondView()
        {
            MainWindowViewModel target = new MainWindowViewModel();
            target.ShowFirstViewCmd = new DelegateCommand();
            target.ShowFirstViewCmd.ExecuteCommand = o =>
            {
                target.WorkspaceMulti.Add(new SecondViewModel());
            };
            target.ShowFirstViewCmd.Execute(null);

            var collectionView = CollectionViewSource.GetDefaultView(target.WorkspaceMulti);
            Assert.IsTrue(collectionView.CurrentItem is SecondViewModel, "Invalid current item.");
        }

    }
}
