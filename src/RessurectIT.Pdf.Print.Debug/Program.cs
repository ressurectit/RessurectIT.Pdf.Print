using Grpc.Net.Client;

// The port number must match the port of the gRPC server.
using var channel = GrpcChannel.ForAddress("http://127.0.0.1:8000");

var client = new PrintService.PrintServiceClient(channel);
//var z = client.GetDefaultPrinter(new Empty());
//AvailablePrinters x = client.GetPrinters(new Empty());

try
{
    client.Print(new PrintRequest
    {
        PdfPath = "zrusenieSluzieb.pdf",
        Copies = 2
    });
}
catch(Exception e)
{
}
//var reply = await client.SayHelloAsync(
//                  new HelloRequest { Name = "GreeterClient" });
//Console.WriteLine("Greeting: " + reply.Message);
Console.WriteLine("Press any key to exit...");
Console.ReadKey();