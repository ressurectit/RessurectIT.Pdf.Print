using Grpc.Net.Client;
using RessurectIT.Pdf.Print.Api;
using RessurectIT.Pdf.Print.Dto;

namespace RessurectIT.Pdf.Print.Services;

//TODO: handle multiple stop calls
//TODO: handle null default printer, also in JS

/// <summary>
/// Class that represents pdf print server
/// </summary>
public class PdfPrintServer : IPdfPrintServer, IDisposable
{
    #region private fields

    /// <summary>
    /// Instance of gRPC node print server
    /// </summary>
    private readonly GrpcNodePrintServer _printServer;

    /// <summary>
    /// Instance of gRPC channel
    /// </summary>
    private GrpcChannel? _grpcChannel;

    /// <summary>
    /// Instance of gRPC client
    /// </summary>
    private PrintService.PrintServiceClient? _client;
    #endregion


    #region public properties - Implementation of IPdfPrintServer

    /// <inheritdoc />
    public bool Running
    {
        get;
        private set;
    }
    #endregion


    #region constructors

    /// <summary>
    /// Creates instance of <see cref="PdfPrintServer"/>
    /// </summary>
    public PdfPrintServer()
    {
        _printServer = new GrpcNodePrintServer();
    }

    /// <summary>
    /// Creates instance of <see cref="PdfPrintServer"/>
    /// </summary>
    /// <param name="nodeJsPath">Path to 'node' that will be used for running gRPC server</param>
    public PdfPrintServer(string nodeJsPath)
    {
        _printServer = new GrpcNodePrintServer(nodeJsPath);
    }
    #endregion


    #region public methods - Implementation of IPdfPrintServer

    /// <inheritdoc />
    public async Task Start()
    {
        int port = await _printServer.Start();

        _grpcChannel = GrpcChannel.ForAddress($"http://127.0.0.1:{port}");
        _client = new PrintService.PrintServiceClient(_grpcChannel);

        Running = true;
    }

    /// <inheritdoc />
    public async Task Stop()
    {
        Running = false;

        if (_grpcChannel != null)
        {
            await _grpcChannel.ShutdownAsync();
            _grpcChannel.Dispose();
        }

        _client = null;
        _printServer.Stop();
    }

    /// <inheritdoc />
    public AvailablePrintersDto GetPrinters()
    {
        if (!Running)
        {
            throw new InvalidOperationException("GetPrinters must be called after server was started");
        }

        AvailablePrinters? printers = _client?.GetPrinters(new Empty());

        if (printers == null)
        {
            return new AvailablePrintersDto();
        }

        return new AvailablePrintersDto
        {
            Printers = printers.Printers.Select(itm => new PrinterDto
            {
                DeviceId = itm.DeviceId,
                Name = itm.Name
            }).ToArray()
        };
    }

    /// <inheritdoc />
    public PrinterDto? GetDefaultPrinter()
    {
        if (!Running)
        {
            throw new InvalidOperationException("GetDefaultPrinter must be called after server was started");
        }

        Printer? printer = _client?.GetDefaultPrinter(new Empty());

        if (printer == null)
        {
            return null;
        }

        return new PrinterDto
        {
            DeviceId = printer.DeviceId,
            Name = printer.Name
        };
    }

    /// <inheritdoc />
    public void Print(string pdfPath, PrintOptions? options)
    {
        if (!Running)
        {
            throw new InvalidOperationException("Print must be called after server was started");
        }

        PrintRequest request = new PrintRequest
        {
            PdfPath = pdfPath
        };

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

        _client?.Print(request);
    }

    /// <inheritdoc />
    public void Print(string pdfPath)
    {
        if (!Running)
        {
            throw new InvalidOperationException("Print must be called after server was started");
        }

        Print(pdfPath, null);
    }
    #endregion


    #region public methods - Implementation of IDisposable

    /// <inheritdoc />
    public void Dispose()
    {
        _printServer.Dispose();
        _grpcChannel?.Dispose();
    }
    #endregion
}
