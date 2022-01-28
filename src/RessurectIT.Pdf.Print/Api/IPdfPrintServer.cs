using RessurectIT.Pdf.Print.Dto;

namespace RessurectIT.Pdf.Print.Api;

/// <summary>
/// Server used for printing PDF files
/// </summary>
public interface IPdfPrintServer
{
    #region properties

    /// <summary>
    /// Gets indication whether server is running
    /// </summary>
    bool Running
    {
        get;
    }
    #endregion


    #region methods

    /// <summary>
    /// Starts print server
    /// </summary>
    Task Start();

    /// <summary>
    /// Stops running print server
    /// </summary>
    Task Stop();

    /// <summary>
    /// Gets array of all available printers
    /// </summary>
    /// <returns>Array of all available printers</returns>
    /// <exception cref="InvalidOperationException">Occurs when called before starting of server</exception>
    AvailablePrintersDto GetPrinters();

    /// <summary>
    /// Gets default printer or null if no default printer
    /// </summary>
    /// <returns>default printer or null if no default printer</returns>
    /// <exception cref="InvalidOperationException">Occurs when called before starting of server</exception>
    PrinterDto? GetDefaultPrinter();

    /// <summary>
    /// Runs print of pdf document
    /// </summary>
    /// <param name="pdfPath">Path to pdf file to be printed</param>
    /// <param name="options">Options for printing</param>
    /// <exception cref="InvalidOperationException">Occurs when called before starting of server</exception>
    void Print(string pdfPath, PrintOptions? options);

    /// <summary>
    /// Runs print of pdf document with default options, on default printer
    /// </summary>
    /// <param name="pdfPath">Path to pdf file to be printed</param>
    /// <exception cref="InvalidOperationException">Occurs when called before starting of server</exception>
    void Print(string pdfPath);
    #endregion
}