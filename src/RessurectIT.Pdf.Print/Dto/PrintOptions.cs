using RessurectIT.Pdf.Print.Dto.Enum;

namespace RessurectIT.Pdf.Print.Dto;

/// <summary>
/// Request storing information about what should be printed
/// </summary>
public class PrintOptions
{
    #region public properties

    /// <summary>
    /// Send a file to the specified printer. Id of printer.
    /// </summary>
    public string? Printer
    {
        get;
        set;
    }

    //TODO: regex validator
    /// <summary>
    /// Specifies which pages to print in the PDF document. Supports ranges (1-5), supports comma separated pages (3,5) and combinations (1-3,5)
    /// </summary>
    public string? Pages
    {
        get;
        set;
    }

    /// <summary>
    /// Will print odd pages only when value is odd. Will print even pages only when even.
    /// </summary>
    public PrintSubset? Subset
    {
        get;
        set;
    }

    /// <summary>
    /// Can provide 90-degree rotation of contents (NOT the rotation of paper which must be pre-set by the choice of printer defaults).
    /// </summary>
    public PrintOrientation? Orientation
    {
        get;
        set;
    }

    /// <summary>
    /// Supported names noscale, shrink and fit.
    /// </summary>
    public PrintScale? Scale
    {
        get;
        set;
    }

    /// <summary>
    /// Prints the document in black and white. Default is false.
    /// </summary>
    public bool? Monochrome
    {
        get;
        set;
    }

    /// <summary>
    /// Supported names duplex, duplexshort, duplexlong and simplex.
    /// </summary>
    public PrintSide? Side
    {
        get;
        set;
    }

    /// <summary>
    /// Select tray to print to. Number or name.
    /// </summary>
    public string? Bin
    {
        get;
        set;
    }

    /// <summary>
    /// Specifies the paper size. Supported names A2, A3, A4, A5, A6, letter, legal, tabloid, statement.
    /// </summary>
    public PrintPaperSize? PaperSize
    {
        get;
        set;
    }

    /// <summary>
    /// Silences SumatraPDF's error messages.
    /// </summary>
    public bool? Silent
    {
        get;
        set;
    }

    /// <summary>
    /// Displays the Print dialog for all the files indicated on this command line.
    /// </summary>
    public bool? PrintDialog
    {
        get;
        set;
    }

    /// <summary>
    /// Specifies how many copies will be printed.
    /// </summary>
    public int? Copies
    {
        get;
        set;
    }
    #endregion
}

/// <summary>
/// Request storing information about what should be printed with updated enums to strings
/// </summary>
internal class PrintOptionsNode : PrintOptions
{
    #region public properties

    /// <summary>
    /// Will print odd pages only when value is odd. Will print even pages only when even.
    /// </summary>
    public new string? Subset
    {
        get;
        set;
    }

    /// <summary>
    /// Can provide 90-degree rotation of contents (NOT the rotation of paper which must be pre-set by the choice of printer defaults).
    /// </summary>
    public new string? Orientation
    {
        get;
        set;
    }

    /// <summary>
    /// Supported names noscale, shrink and fit.
    /// </summary>
    public new string? Scale
    {
        get;
        set;
    }

    /// <summary>
    /// Supported names duplex, duplexshort, duplexlong and simplex.
    /// </summary>
    public new string? Side
    {
        get;
        set;
    }

    /// <summary>
    /// Specifies the paper size. Supported names A2, A3, A4, A5, A6, letter, legal, tabloid, statement.
    /// </summary>
    public new string? PaperSize
    {
        get;
        set;
    }
    #endregion
}