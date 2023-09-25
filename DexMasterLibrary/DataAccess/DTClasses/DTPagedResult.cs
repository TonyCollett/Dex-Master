namespace DexMasterLibrary.DataAccess.DTClasses;

public class DTPagedResult<T>
{
    public IEnumerable<T> Items { get; set; }
    public int TotalPages { get; set; }
}
