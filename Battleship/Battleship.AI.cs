namespace Battleship.AI;

/// <summary>
/// 
/// </summary>
public sealed class BoardBot : Board 
{
    #region PRIVATE   INSTANCE FIELDS

    private readonly Random Generator = new ();

    private int First_Row = -1;
    private int First_Column = -1;

    private int Last_Row = -1;
    private int Last_Column = -1;

    private bool Bias = false;

    #endregion

    /// <summary>
    /// 
    /// </summary>
    /// <param name = "name"></param>
    public BoardBot ( string name ) : base ( name ) {}

    #region PUBLIC    OVERRIDE FUNCTIONS

    /// <summary>
    /// 
    /// </summary>
    /// <param name = "name"></param>
    public override void Place ( string name ) 
    {
        BotBoat boat = BotBoat.Create( name )!;
        bool valid = false;
        int row = -1;
        int column = -1;

        while ( !valid )
        {
            row = this.Generate_Row( this );
            column = this.Generate_Column( this, row );

            boat.Rotate();

            bool rotation = boat.Get_Rotation();

            if ( !this.Validate_Placement( rotation, boat.Length, row, column ) ) continue;
            
            valid = true;
        }
        this.Place( boat, row, column );
    }

    /// <summary>
    /// 
    /// </summary>
    public override void Hit ( Board board ) 
    {
        this.Bias = this.Generator.Next( 0, 2 ) == 0;

        int row = this.Generate_Row( board );
        int column = this.Generate_Column( board, row );

        while ( board.Grid[ row ][ column ].STATUS )
        {
            this.Bias = this.Generator.Next( 0, 2 ) == 0;

            row = this.Generate_Row( board );
            column = this.Generate_Column( board, row );
        }
        board.Grid[ row ][ column ].STATUS = true;

        if ( board.Grid[ row ][ column ].BOAT == null )
        {
            Console.WriteLine( $"{ this.Name } Missed { board.Name }'s boat!" ); 
            
            return;
        }
        else if ( board.Grid[ row ][ column ].BOAT!.Hit() )
        {
            this.First_Row = -1;
            this.First_Column = -1;

            this.Last_Row = -1;
            this.Last_Column = -1;

            Console.WriteLine( $"{ this.Name } Sunk { board.Name }'s { board.Grid[ row ][ column ].BOAT!.Name }!");

            return;
        }
        Console.WriteLine( $"{ this.Name } Hit { board.Name }'s boat!" );

        if ( this.First_Row == -1 ) this.First_Row = row;
        if ( this.First_Column == -1 ) this.First_Column = column;

        this.Last_Row = row != this.First_Row ? row : this.Last_Row;
        this.Last_Column = column != this.First_Column ? column : this.Last_Column;
    }

    #endregion
    #region PROTECTED OVERRIDE FUNCTIONS

    /// <summary>
    /// 
    /// </summary>
    /// <param name = "board"></param>
    /// <returns></returns>
    protected override int Generate_Row ( Board board )
    {
        int row = ( this.First_Row != -1 && this.Last_Row == -1 ) || this.Bias ? this.First_Row : this.Last_Row;

        if ( row == -1 ) return this.Generator.Next( 0, board.Grid.Length );

        return this.Generator.Next( Math.Max( 0, row - 1 ), Math.Min( board.Grid.Length - 1, row + 1 ) + 1 );
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name = "board"></param>
    /// <param name = "row"></param>
    /// <returns></returns>
    protected override int Generate_Column( Board board, int row )
    {
        int bRow = ( this.First_Row != -1 && this.Last_Row == -1 ) || this.Bias ? this.First_Row : this.Last_Row;
        int bColumn = ( this.First_Column != -1 && this.Last_Column == -1 ) || this.Bias ? this.First_Column : this.Last_Column;

        if ( bColumn == -1 ) return this.Generator.Next( 0, board.Grid[ row ].Length );

        if ( bRow - 1 == row || bRow + 1 == row ) return bColumn;

        int min = Math.Max( 0, bColumn - 1 );
        int max = Math.Min( board.Grid[ row ].Length - 1, bColumn + 1 );

        int result = bColumn;

        do result = this.Generator.Next( min, max + 1 ); while ( result == bColumn );

        return result;
    }

    #endregion
}

/// <summary>
/// 
/// </summary>
internal sealed class BotBoat : Boat
{
    #region PRIVATE  INSTANCE FIELDS

    private readonly Random Generator = new ();

    #endregion

    /// <summary>
    /// 
    /// </summary>
    /// <param name = "length"></param>
    internal BotBoat ( string name, int length ) : base ( name, length ) {}

    #region INTERNAL STATIC   INITIALIZATION

    /// <summary>
    /// 
    /// </summary>
    /// <param name = "name"></param>
    /// <returns></returns>
    internal static new BotBoat? Create ( string name ) 
    {
        if ( !Manager.Types.TryGetValue( name, out int dLength ) ) return null;

        return new ( name, dLength );
    }

    #endregion
    #region INTERNAL OVERRIDE FUNCTIONS

    /// <summary>
    /// 
    /// </summary>
    internal override void Rotate () 
    {
        this.Rotation = this.Generator.Next( 0, 1 ) == 1;
    }

    #endregion
}
