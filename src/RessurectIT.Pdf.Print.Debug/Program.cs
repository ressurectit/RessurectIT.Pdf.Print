using RessurectIT.Pdf.Print;


//// The port number must match the port of the gRPC server.
//using var channel = GrpcChannel.ForAddress("http://127.0.0.1:8000");

//var client = new PrintService.PrintServiceClient(channel);
////var z = client.GetDefaultPrinter(new Empty());
////AvailablePrinters x = client.GetPrinters(new Empty());

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
////var reply = await client.SayHelloAsync(
////                  new HelloRequest { Name = "GreeterClient" });
////Console.WriteLine("Greeting: " + reply.Message);
//Console.WriteLine("Press any key to exit...");
//Console.ReadKey();
//var z = typeof(GrpcNodePrintServer).Assembly.GetManifestResourceNames();

using var server = new GrpcNodePrintServer();
var port = await server.Start();

// RessurectIT.Pdf.Print.index.js
// RessurectIT.Pdf.Print.print.js
// RessurectIT.Pdf.Print.proto.AvailablePrinters.js
// RessurectIT.Pdf.Print.proto.Empty.js
// RessurectIT.Pdf.Print.proto.pdfPrint.js
// RessurectIT.Pdf.Print.proto.pdfPrint.proto.js
// RessurectIT.Pdf.Print.proto.Printer.js
// RessurectIT.Pdf.Print.proto.PrintRequest.js
// RessurectIT.Pdf.Print.proto.PrintService.js
// RessurectIT.Pdf.Print.node_modules.zip


//var x = typeof(GrpcNodePrintServer).Assembly.GetManifestResourceNames();

//foreach (var s in z)
//{
//    Console.WriteLine(s);
//}

await Task.Delay(5000);

server.Stop();

await Task.Delay(1000);

port = await server.Start();

await Task.Delay(2500);

Console.WriteLine(port);

//Console.WriteLine("ok");
Console.ReadLine();
