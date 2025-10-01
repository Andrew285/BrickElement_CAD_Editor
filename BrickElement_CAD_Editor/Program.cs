using App.Tools;
using Core.Commands;
using Core.Models.Graphics.Cameras;
using Core.Models.Graphics.Rendering;
using Core.Models.Scene;
using Core.Services;
using UI.MainFormLayout;
using UI.MainFormLayout.MiddleViewLayout.PropertyViewLayout;
using UI.Utils.ConsoleLogging;

namespace BrickElement_CAD_Editor
{
    static class Program
    {
        [STAThread]
        static void Main()
        {

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //var services = new ServiceCollection();
            //services.AddSingleton<ICamera, PerspectiveCamera>();
            //services.AddSingleton<IScene, Scene>();
            //services.AddSingleton<IRenderer>(provider =>
            //{
            //    var mainForm = provider.GetRequiredService<IMainForm>();
            //    return new Renderer(mainForm.Control, mainForm.MainView.MiddleView.SceneView.Control);
            //});

            //services.AddSingleton<IMainForm, MainForm>();
            //services.AddSingleton<IMainView>(provider => new MainView(4, 1));
            //services.AddSingleton<MainViewPresenter>();
            //services.AddSingleton<MainFormPresenter>();
            //services.AddSingleton<MiddleViewPresenter>();
            //services.AddSingleton<IMiddleView>();
            //services.AddSingleton<SceneViewPresenter>();

            //var serviceProvider = services.BuildServiceProvider();
            //var mainFormPresenter = serviceProvider.GetRequiredService<MainFormPresenter>();
            //var mainViewPresenter = serviceProvider.GetRequiredService<MainViewPresenter>();


            // Create Console window
            ConsoleAllocator.Create();

            LanguageService languageService = LanguageService.GetInstance();
            languageService.ChangeLanguage(Language.UKRAINIAN);

            // Initialize MainForm
            IMainForm view = new MainForm();

            ICamera camera = new PerspectiveCamera();
            IRenderer renderer = new Renderer(view.Control, view.MainView.MiddleView.SceneView.Control);
            renderer.Camera = camera;

            IScene scene = new Scene();
            scene.Camera = camera;

            IPropertyView propertyView = view.MainView.MiddleView.PropertyView;

            CommandHistory commandHistory = new CommandHistory();
            ToolManager toolManager = new ToolManager(scene, commandHistory, renderer, propertyView);
            //toolManager.SetTool(new SelectionTool(scene, renderer, propertyView));

            new MainFormPresenter(view, renderer, scene, toolManager, languageService);

            Application.Run((MainForm)view.Control);
        }
    }
}
