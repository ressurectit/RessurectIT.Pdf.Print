using Grpc.Net.Client;

namespace RessurectIT.Pdf.Print.Services;

/// <summary>
/// Class that represents pdf print server
/// </summary>
public class PdfPrintServer
{
    public void Test()
    {
        using var channel = GrpcChannel.ForAddress("http://127.0.0.1:8000");

        var client = new PrintService.PrintServiceClient(channel);
        ////var z = client.GetDefaultPrinter(new Empty());
        ////AvailablePrinters x = client.GetPrinters(new Empty());
    }
}



//try
//{
//    client.Print(new PrintRequest
//    {
//        PdfPath = "zrusenieSluzieb.pdf",
//        Copies = 2
//    });
//}
//catch(Exception e)
//{
//}