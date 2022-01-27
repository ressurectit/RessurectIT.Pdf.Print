namespace RessurectIT.Pdf.Print.Dto;

/// <summary>
/// Holds all available printers
/// </summary>
public class AvailablePrintersDto
{
    #region public properties

    /// <summary>
    /// Array of all available printers
    /// </summary>
    public PrinterDto[] Printers
    {
        get;
        set;
    } = Array.Empty<PrinterDto>();
    #endregion
}