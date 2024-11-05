using Dotlanche.Producao.Application.Ports;
using Dotlanche.Producao.Domain.Entities;
using QRCoder;

namespace Dotlanche.Producao.Checkout.Adapters
{
    public class FakeCheckoutQrCodeProvider : IQrCodeProvider
    {
        public string RequestQrCode(RegistroProducao producao)
        {
            Thread.Sleep(TimeSpan.FromSeconds(3));

            const string fakeCode = "DecodedQRCode";

            var qrGenerator = new QRCodeGenerator();
            var qrCodeData = qrGenerator.CreateQrCode(fakeCode, QRCodeGenerator.ECCLevel.Q);
            var qrCode = new Base64QRCode(qrCodeData);
            var qrCodeImage = qrCode.GetGraphic(10);

            return qrCodeImage;
        }
    }
}