using RessurectIT.Pdf.Print.Dto;
using RessurectIT.Pdf.Print.Services;

using PdfPrintServer server = new PdfPrintServer();

//TODO: installer msi with own windows service as http server for printing

await server.Start();

server.Print("D:\\LV 227.pdf",
             new PrintOptions
             {
                 Printer = "Microsoft Print to PDF",
                 Pages = "2,3"
             });

Console.ReadLine(); 
