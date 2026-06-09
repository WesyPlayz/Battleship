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
    #region INTERNAL  INSTANCE FIELDS

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

    #region PUBLIC    VIRTUAL  FUNCTIONS

    /// <summary>
    /// 
    /// </summary>
    /// <param name = "name"></param>
    public virtual void Place ( string name ) 
    {
        Console.WriteLine( $"Placing the { name }\n" );

        Boat boat = Boat.Create( name )!;
        bool valid = false;
        int row = -1;
        int column = -1;

        while ( !valid )
        {
            row = this.Generate_Row( this );
            column = this.Generate_Column( this, row );

            boat.Rotate();

            bool rotation = boat.Get_Rotation();

            if ( !this.Validate_Placement( rotation, boat.Length, row, column ) )
            {
                Console.WriteLine( $"Unable to place { name } here." );
                
                continue;
            }
            valid = true;
        }
        this.Place( boat, row, column );

        Console.WriteLine();
    }

    /// <summary>
    /// 
    /// </summary>
    public virtual void Hit ( Board board ) 
    {
        int row = this.Generate_Row( board );
        int column = this.Generate_Column( board, row );

        while ( board.Grid[ row ][ column ].STATUS )
        {
            Console.WriteLine( "You already hit this coordinate" );

            row = this.Generate_Row( board );
            column = this.Generate_Column( board, row );
        }
        board.Grid[ row ][ column ].STATUS = true;

        Console.WriteLine(
            board.Grid[ row ][ column ].BOAT == null ? $"{ this.Name } Missed { board.Name }'s boat!" :
            !board.Grid[ row ][ column ].BOAT!.Hit() ? $"{ this.Name } Hit { board.Name }'s boat!" :
            $"{ this.Name } Sunk { board.Name }'s { board.Grid[ row ][ column ].BOAT!.Name }!"
        );
    }

    #endregion
    #region PROTECTED VIRTUAL  GENERATORS

    /// <summary>
    /// 
    /// </summary>
    /// <param name = "board"></param>
    /// <returns></returns>
    protected virtual int Generate_Row ( Board board ) 
    {
        Console.Write( $"Please enter a row : ( A-{ ( char )( 'A' + ( board.Grid.Length - 1 ) ) } )" );

        string? rInput = Console.ReadLine();

        if ( rInput == null || !char.TryParse( rInput, out char row ) )
        {
            Console.WriteLine( "Invalid Input, must be of type < char >." );

            return this.Generate_Row( board );
        }
        row = char.ToUpper( row );

        if ( row < 'A' || row > 'A' + ( board.Grid.Length - 1 ) )
        {
            Console.WriteLine( $"Out of range, must fall between A and { ( char )( 'A' + ( board.Grid.Length - 1 ) ) }." );

            return this.Generate_Row( board );
        }
        return row - 'A';
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name = "board"></param>
    /// <param name = "row"></param>
    /// <returns></returns>
    protected virtual int Generate_Column ( Board board, int row ) 
    {
        Console.Write( $"Please enter a column : ( 1-{ board.Grid[ row ].Length } )" );

        string? cInput = Console.ReadLine();

        if ( string.IsNullOrWhiteSpace( cInput ) || !int.TryParse( cInput, out int column ) )
        {
            Console.WriteLine( "Invalid Input, must be of type < int >." );

            return this.Generate_Column( board, row);
        }
        else if ( column < 1 || column > board.Grid[ row ].Length )
        {
            Console.WriteLine( $"Out of range, must fall between 1 and { board.Grid[ row ].Length }." );

            return this.Generate_Column( board, row);
        }
        return column - 1;
    }

    #endregion
    #region PROTECTED INSTANCE FUNCTIONS

    /// <summary>
    /// 
    /// </summary>
    /// <param name = "rotation"></param>
    /// <param name = "length"></param>
    /// <param name = "row"></param>
    /// <param name = "column"></param>
    /// <returns></returns>
    protected bool Validate_Placement ( bool rotation, int length, int row, int column ) 
    {
        int veritcal = rotation ? 1 : 0;
        int horizontal = veritcal == 0 ? 1 : 0;

        int lRow = row + veritcal * ( length - 1 );
        int lColumn = column + horizontal * ( length - 1 );

        if (
            row < 0 ||
            column < 0 ||
            lRow >= this.Grid.Length ||
            lColumn >= this.Grid[ row ].Length
        ) return false;

        for ( int idx = 0; idx < length; idx++ )
        {
            int cRow = row + veritcal * idx;
            int cCol = column + horizontal * idx;

            if ( this.Grid[ cRow ][ cCol ].BOAT != null ) return false;
        }
        return true;
    }

    #endregion
    #region INTERNAL  INSTANCE FUNCTIONS

    /// <summary>
    /// 
    /// </summary>
    /// <param name = "boat"></param>
    /// <param name = "row"></param>
    /// <param name = "column"></param>
    internal void Place ( Boat boat, int row, int column ) 
    {
        int veritcal = boat.Get_Rotation() ? 1 : 0;
        int horizontal = veritcal == 0 ? 1 : 0;

        for ( int idx = 0; idx < boat.Length; idx++ )
        {
            int cRow = row + veritcal * idx;
            int cCol = column + horizontal * idx;

            this.Grid[ cRow][ cCol ].BOAT = boat;
        }
    }

    #endregion
    #region PUBLIC    INSTANCE STATUS FUNCTIONS

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
    #region PUBLIC    INSTANCE DISPLAY FUNCTIONS

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
    #region INTERNAL  INSTANCE FIELDS

    internal readonly string Name = name;

    #endregion
    #region PROTECTED INSTANCE FIELDS

    protected bool Rotation;

    #endregion
    #region INTERNAL  INSTANCE PROPERTIES

    /// <summary>
    /// 
    /// </summary>
    internal int Length { get; private set; } = length;

    #endregion
    #region INTERNAL  STATIC   INITIALIZATION

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
    #region INTERNAL  VIRTUAL  FUNCTIONS

    /// <summary>
    /// 
    /// </summary>
    internal virtual void Rotate () 
    {
        Console.Write( "Do you wish to rotate this boat? ( Y / N ) " );

        string? rInput = Console.ReadLine();

        if ( string.IsNullOrWhiteSpace( rInput ) )
        {
            Console.WriteLine( "Invalid Input, must be of type < string >." );

            this.Rotate();
        }
        rInput = rInput!.ToUpper();

        if ( rInput != "Y" && rInput != "N" )
        {
            Console.WriteLine( "Invalid Input, must be Y or N." );

            this.Rotate();
        }
        this.Rotation = rInput == "Y";
    }

    #endregion
    #region INTERNAL  INSTANCE FUNCTIONS

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    internal bool Get_Rotation () => this.Rotation;

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    internal bool Hit () 
    {
        this.Length = Math.Max( 0, this.Length - 1 );

        if ( this.Length <= 0 ) return true;

        return false;
    }

    #endregion
}
