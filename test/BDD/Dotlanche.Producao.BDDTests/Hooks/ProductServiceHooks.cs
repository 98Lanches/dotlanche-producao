using Dotlanche.Producao.BDDTests.Drivers;
using WireMock.Server;

namespace Dotlanche.Producao.BDDTests.Hooks
{
    [Binding]
    public class ProductServiceHooks
    {
        public const int ServerPort = 8082;
        private static WireMockServer? wiremockServer;

        [BeforeFeature]
        public static void StartMockedServer()
        {
            wiremockServer = WireMockServer.Start(ServerPort);
            ProdutoServiceDriver.SetWiremockServer(wiremockServer);
        }

        [AfterFeature]
        public static void StopMockedServer()
        {
            wiremockServer?.Stop();
            wiremockServer?.Dispose();
        }
    }
}