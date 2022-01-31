using Jering.Javascript.NodeJS;
using RessurectIT.Pdf.Print.Api;
using RessurectIT.Pdf.Print.Dto;

namespace RessurectIT.Pdf.Print.Services;

/// <summary>
/// Class that represents pdf print server
/// </summary>
public class PdfPrintServer : IPdfPrintServer, IDisposable
{
    #region private fields

    /// <summary>
    /// Instance of gRPC node print server
    /// </summary>
    private readonly NodePdfPrintServer _printServer;

    /// <summary>
    /// Path to index.js file
    /// </summary>
    private readonly string _indexPath;
    #endregion


    #region constructors

    /// <summary>
    /// Creates instance of <see cref="PdfPrintServer"/>
    /// </summary>
    public PdfPrintServer()
    {
        _printServer = new NodePdfPrintServer();
        _indexPath = Path.Combine(_printServer.NodeServerFilesDir, "index.js");
    }

    /// <summary>
    /// Creates instance of <see cref="PdfPrintServer"/>
    /// </summary>
    /// <param name="nodeJsPath">Path to 'node' that will be used for running gRPC server</param>
    public PdfPrintServer(string nodeJsPath)
    {
        _printServer = new NodePdfPrintServer(nodeJsPath);
        _indexPath = Path.Combine(_printServer.NodeServerFilesDir, "index.js");
    }
    #endregion


    #region public methods - Implementation of IPdfPrintServer

    /// <inheritdoc />
    public Printer[] GetPrinters()
    {
        return StaticNodeJSService.InvokeFromFileAsync<Printer[]>(_indexPath, "getPrinters").Result ?? Array.Empty<Printer>();
    }

    /// <inheritdoc />
    public Printer? GetDefaultPrinter()
    {
        return StaticNodeJSService.InvokeFromFileAsync<Printer>(_indexPath, "getDefaultPrinter").Result;
    }

    /// <inheritdoc />
    public void Print(string pdfPath, PrintOptions? options)
    {
        PrintOptionsNode request = new PrintOptionsNode();

        if (options != null)
        {
            if (options.Printer != null)
            {
                request.Printer = options.Printer;
            }

            if (options.Pages != null)
            {
                request.Pages = options.Pages;
            }

            if (options.Subset != null)
            {
                request.Subset = options.Subset.ToString().ToLower();
            }

            if (options.Orientation != null)
            {
                request.Orientation = options.Orientation.ToString().ToLower();
            }

            if (options.Scale != null)
            {
                request.Scale = options.Scale.ToString().ToLower();
            }

            if (options.Monochrome != null)
            {
                request.Monochrome = options.Monochrome.Value;
            }

            if (options.Side != null)
            {
                request.Side = options.Side.ToString().ToLower();
            }

            if (options.Bin != null)
            {
                request.Bin = options.Bin;
            }

            if (options.PaperSize != null)
            {
                request.PaperSize = options.PaperSize.ToString().ToLower();
            }

            if (options.Silent != null)
            {
                request.Silent = options.Silent.Value;
            }

            if (options.PrintDialog != null)
            {
                request.PrintDialog = options.PrintDialog.Value;
            }

            if (options.Copies != null)
            {
                request.Copies = options.Copies.Value;
            }
        }

        StaticNodeJSService.InvokeFromFileAsync(_indexPath, "print", new object[]{pdfPath, request}).Wait();
    }

    /// <inheritdoc />
    public void Print(string pdfPath)
    {
        Print(pdfPath, null);
    }
    #endregion


    #region public methods - Implementation of IDisposable

    /// <inheritdoc />
    public void Dispose()
    {
        _printServer.Dispose();
    }
    #endregion
}
