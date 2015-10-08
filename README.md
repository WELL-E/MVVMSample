
## MVVM模式 ##

### 一、MVVM模式概述 ###

MVVM Pattern : Model\View\ViewModel

View：视图、UI界面

ViewModel：ViewModel是对Model的封装，通过一系列属性暴露Model的状态，提供给View进行显示

Model：数据模型

> 使用MVVM模式可以将代码逻辑和UI进行分离，因此开发团队可以关注创建健壮的ViewModel类，而设计团队可以关注设计界面友好的View。要融合两个团队输出只需要在View的xaml上进行正确的绑定即可。


### 二、演示程序 ###

下面通过一个Demo演示WPF中如何使用MVVM模式：使用WPF中的data template、commands、data binding、resource结合MVVM模式，创建一个简单、可测试、健壮的框架。

演示程序结构图如下：

![](http://i.imgur.com/G5byg4f.jpg)

1、演示程序
Demo界面如图所示：

![](http://i.imgur.com/YcK8PCT.jpg)

工作区用于显示视图

命令区分两部分，上部分为显示单视图命令，下部分为显示多视图命令

单视图：工作区始终只显示一个视图。

多视图：工作区可以显示多个视图，以TabControl控件的TabItem进行展示。可以通过previousview命令显示视图集合中的上一个视图，通过nextview显示视图集合中的下一个视图。

Demo的MainWindow.xaml文件中，使用单视图时，需要注释多视图；使用多视图时，需要注释单视图。代码如下：

	<!--single view-->
	<ContentPresenter Content="{Binding Path=WorkspaceSingle}"/>
	
	<!--multi view-->
	<ContentPresenter Content="{Binding Path=WorkspaceMulti}" ContentTemplate="{StaticResource WorkspacesTemplate}"/>

### 三、数据模型(Model)、视图（View） ###
 
为了使Demo更容易理解，程序中只使用了一个Model，Model中Name属性用于显示视图名称。

    public class InfoModel
    {
		//视图名称
        public string Name { get; set; }
    }

两个简单的视图：FirstView、SecondView，视图中控件显示当前的视图名称，如视图FirstView：

![](http://i.imgur.com/H0cx7GU.jpg)

	<StackPanel Background="Aqua">
		<TextBlock Text="{Binding Path=Name}" FontSize="20"/>
	</StackPanel>

	<StackPanel Background="Chartreuse">
		<TextBlock Text="{Binding Path=Name}" FontSize="20"/>
	</StackPanel>

在实际开发中，视图中可以布局其它控件，并进行正确的绑定，界面都能正常的显示。

显示多视图时，TabItem的Header能显示视图名称，如图所示：

![](http://i.imgur.com/TIO2Nhm.jpg)

视图名称DisplayName是基类ViewModelBase的属性

	/// <summary>
	/// 名称
	/// </summary>
	public virtual string DisplayName { get; protected set; }

子类在构造函数中给DisplayName赋值

	public class FirstViewModel : WorkspaceViewModel
	{
	    private const string DisplayViewName = "FirstView";
	
	    private readonly InfoModel _info;
	
	    public FirstViewModel()
	    {
			//视图名称
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

在View中绑定视图名称

	<DataTemplate x:Key="TabItemTemplate">
	    <DockPanel>
	        <ContentPresenter Content="{Binding Path=DisplayName}" VerticalAlignment="Center"/>
	    </DockPanel>
	</DataTemplate>


### 四、ViewModel类图 ###

大家一看代码就知道，整个ViewModel使用的是什么设计模式

![](http://i.imgur.com/YtY34N0.jpg)


### 五、View对应ViewModel ###

Demo的一个主要特点是数据延迟加载，即在需要数据时创建ViewModel

程序在启动时即为主界面加载数据MainWindowViewModel

	public partial class App : Application
	{
		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);
			MainWindow window = new MainWindow();
			var viewModel = new MainWindowViewModel();
			window.DataContext = viewModel;
			window.Show();
		}
	}

程序启动后，单击按钮时创建FirstViewModel或SecondViewModel

**创建单视图**

	/// <summary>
	/// 显示视图一
	/// </summary>
	private void ShowFirstView()
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
	private void ShowSecondView()
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

**创建多视图**

	/// <summary>
	/// 创建视图一，并显示
	/// </summary>
	private void CreateFirstView()
	{
		var model = new FirstViewModel();
		WorkspaceMulti.Add(model);
		ShowCurrentView(model);
	}
	
	/// <summary>
	/// 创建视图二
	/// </summary>
	private void CreateSecondView()
	{
		var model = new SecondViewModel();
		WorkspaceMulti.Add(model);
		ShowCurrentView(model);
	}

创建后，WPF自动为匹配的View Model寻找View来渲染。

	<DataTemplate DataType="{x:Type vm:FirstViewModel}">
		<vw:FirstView/>
	</DataTemplate>
	
	<DataTemplate DataType="{x:Type vm:SecondViewModel}">
		<vw:SecondView/>
	</DataTemplate>


### 六、总结 ###

MVVM模式是设计和开发WPF程序一种简单、有效的指导方针。它允许你创建数据、行为和展示强分离的程序，更容易控制软件开发中的混乱因素。
