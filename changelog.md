# Changelog

## Version 1.0.0 (2022-01-28)

### Features

- `RessurectIT.Pdf.Print.NodeServer.Node`
    - node js gRPC server
        - gRPC `PrintService`
            - available RPC method `Print` allows printing of PDF
            - available RPC method `GetPrinters` allows obtaining all available printers
            - available RPC method `GetDefaultPrinter` allows obtaining default printer
        - gRPC messages
            - `Empty` represents empty request or response
            - `Printer` basic info about printer
                - `deviceId` - id of printer
                - `name` - name of printer
            - `AvailablePrinters` holds all available printers
                - `printers` - array of all available printers
            - `PrintRequest` request storing information about what should be printed
                - `pdfPath` - path to PDF file
                - rest of parameters are from `pdf-to-printer` npm package
- `RessurectIT.Pdf.Print.NodeServer`
    - c# wrapper around nodejs server `GrpcNodePrintServer`
        - extracts gRPC node files
        - runs new `Node` process with gRPC server
        - obtains port of newly running gRPC server
- `RessurectIT.Pdf.Print`
    - c# gRPC client used as c# `PdfPrintServer` implementing `IPdfPrintServer`
        - allows starting of gRPC client and calling methods `Print`, `GetDefaultPrinter`, `GetPrinters`
    - added strongly typed classes and enums as DTO `AvailablePrintersDto`, `PrinterDto`, `PrintOptions`, `PrintOrientation`, `PrintPaperSize`, `PrintScale`, `PrintSide`, `PrintSubset`
