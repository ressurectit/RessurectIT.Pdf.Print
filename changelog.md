# Changelog

## Version 2.0.1 (2022-02-02)

### Bug Fixes

- `RessurectIT.Pdf.Print.NodeServer`
    - added missing SourceLink for github
- `RessurectIT.Pdf.Print`
    - added missing SourceLink for github

## Version 2.0.0 (2022-01-31)

### BREAKING CHANGES

- project no longer using *gRPC* for communication with *Node*, instead `Jering.Javascript.NodeJS` is used
- `RessurectIT.Pdf.Print.NodeServer.Node`
    - `index.js` exports previous RPC methods as module methods
    - removed `AvailablePrinters` dto, now returns array of printers
    - new `print` method have two parameters same as `pdf-to-printer`
- `RessurectIT.Pdf.Print.NodeServer`
    - c# wrapper around nodejs server `NodePdfPrintServer`
        - extracts node files
        - configure node process
- `RessurectIT.Pdf.Print`
    - c# node client used as c# `PdfPrintServer` implementing `IPdfPrintServer`
        - removed `Running` property, removed `Start`, `Stop` methods

## Version 1.0.1 (2022-01-28)

### Bug Fixes

- `RessurectIT.Pdf.Print.NodeServer`
    - now correctly embeds all `.js` resources

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
