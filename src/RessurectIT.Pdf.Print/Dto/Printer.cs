namespace RessurectIT.Pdf.Print.Dto;

/// <summary>
/// Basic info about printer
/// </summary>
public class Printer
{
    #region public properties

    /// <summary>
    /// Id of printer
    /// </summary>
    public string? DeviceId
    {
        get;
        set;
    }

    /// <summary>
    /// Name of printer
    /// </summary>
    public string? Name
    {
        get;
        set;
    }
    #endregion
}