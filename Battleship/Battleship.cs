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
    #region PUBLIC   STATIC BOARD FUNCTIONS

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

    #endregion
    #region PUBLIC   STATIC BOAT FUNCTIONS

    /// <summary>
    /// 
    /// </summary>
    /// <param name = "name"></param>
    /// <param name = "length"></param>
    public static void Add_Type ( string name, int length ) 
    {
        if ( Types.ContainsKey( name ) ) return;

        Types[ name ] = length;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name = "name"></param>
    public static void Remove_Type ( string name ) 
    {
        if ( !Types.ContainsKey( name ) ) return;

        Types.Remove( name );
    }

    #endregion
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

    #region PUBLIC INSTANCE STATUS FUNCTIONS

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public bool Status () 
    {
        foreach ( ( Boat?, bool )[] column in this.Grid )
        {
            foreach ( ( Boat? boat, bool status ) in column ) if ( boat != null && !status ) return false;
        }
        return true;
    }

    #endregion
    #region PUBLIC INSTANCE DISPLAY FUNCTIONS

    /// <summary>
    /// 
    /// </summary>
    public void Display () 
    {
        char rIdx = 'A';
        int cIdx = 1;

        foreach ( ( Boat?, bool )[] column in Grid )
        {
            foreach ( ( Boat? boat, bool status ) in column )
            {
                Console.WriteLine( $"{ rIdx }{ cIdx } : { ( boat == null ? "EMTPY" : boat.Name ) }, { status }" );

                cIdx++;
            }
            cIdx = 1;
            rIdx++;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public void Display_Boats () 
    {
        char rIdx = 'A';
        int cIdx = 1;

        foreach ( ( Boat?, bool )[] column in Grid )
        {
            foreach ( ( Boat? boat, bool status ) in column )
            {
                if ( boat != null ) Console.WriteLine( $"{ rIdx }{ cIdx } : { boat.Name }, { status }" );

                cIdx++;
            }
            cIdx = 1;
            rIdx++;
        }
    }

    #endregion
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

    #region INTERNAL STATIC INITIALIZATION

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

    #endregion
}
