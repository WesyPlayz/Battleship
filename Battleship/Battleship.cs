namespace Battleship;

/// <summary>
/// 
/// </summary>
public static class Manager
{
    #region INTERNAL STATIC FIELDS

    /// <summary>
    /// 
    /// </summary>
    internal static Dictionary < string, int > Types = [];

    internal static int Rows;
    internal static int Columns;

    #endregion

    /// <summary>
    /// 
    /// </summary>
    /// <param name = "rows"></param>
    /// <param name = "columns"></param>
    public static void Board_Size ( int rows, int columns ) 
    {
        Rows = rows;
        Columns = columns;
    }
}

/// <summary>
/// 
/// </summary>
public class Board
{
    #region INTERNAL INSTANCE FIELDS

    /// <summary>
    /// 
    /// </summary>
    internal readonly ( Boat? BOAT, bool STATUS )[][] Grid;

    internal readonly string Name;

    #endregion

    /// <summary>
    /// 
    /// </summary>
    /// <param name = "name"></param>
    public Board ( string name ) 
    {
        this.Grid = new ( Boat? BOAT, bool STATUS )[ Manager.Rows ][];
        this.Name = name;

        for ( int idx = 0; idx < Manager.Rows; idx++ ) this.Grid[ idx ] = new ( Boat? BOAT, bool STATUS)[ Manager.Columns ];
    }
}

/// <summary>
/// 
/// </summary>
/// <param name = "name"></param>
internal class Boat ( string name, int length )
{
    internal readonly string Name = name;

    protected bool Rotation;

    /// <summary>
    /// 
    /// </summary>
    internal int Length { get; private set; } = length;

    /// <summary>
    /// 
    /// </summary>
    /// <param name = "name"></param>
    /// <returns></returns>
    internal static Boat? Create ( string name ) 
    {
        if ( !Manager.Types.TryGetValue( name, out int dLength ) ) return null;

        return new ( name, dLength );
    }
}
