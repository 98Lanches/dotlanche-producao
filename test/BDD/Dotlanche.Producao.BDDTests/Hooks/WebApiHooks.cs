using Dotlanche.Producao.BDDTests.Setup;

namespace Dotlanche.Producao.BDDTests.Hooks
{
    [Binding]
    public class WebApiHooks
    {
        [BeforeTestRun]
        public static void BeforeFeature(DefaultTestRunContext testRunContext) 
        {
            var webApi = new WebApiFactory();
            testRunContext.TestRunContainer.RegisterInstanceAs(webApi, dispose: true);
        }
    }
}
