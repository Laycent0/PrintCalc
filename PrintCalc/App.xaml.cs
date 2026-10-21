using System.Windows;
using PrintCalc.Data;

namespace PrintCalc
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            Db.Init();              // сначала база
            base.OnStartup(e);      // потом окно
        }
    }
}
