using RessurectIT.Pdf.Print.Dto;

namespace RessurectIT.Pdf.Print.Api;

/// <summary>
/// Server used for printing PDF files
/// </summary>
public interface IPdfPrintServer
{
    #region methods

    /// <summary>
    /// Gets array of all available printers
    /// </summary>
    /// <returns>Array of all available printers</returns>
    Printer[] GetPrinters();

    /// <summary>
    /// Gets default printer or null if no default printer
    /// </summary>
    /// <returns>default printer or null if no default printer</returns>
    Printer? GetDefaultPrinter();

    /// <summary>
    /// Runs print of pdf document
    /// </summary>
    /// <param name="pdfPath">Path to pdf file to be printed</param>
    /// <param name="options">Options for printing</param>
    void Print(string pdfPath, PrintOptions? options);

    /// <summary>
    /// Runs print of pdf document with default options, on default printer
    /// </summary>
    /// <param name="pdfPath">Path to pdf file to be printed</param>
    void Print(string pdfPath);
    #endregion
}