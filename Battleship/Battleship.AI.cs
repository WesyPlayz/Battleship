namespace Battleship.AI;

/// <summary>
/// 
/// </summary>
public sealed class BoardBot : Board 
{
    #region PRIVATE INSTANCE FIELDS

    private readonly Random Generator = new ();

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

    #endregion
    #region PROTECTED OVERRIDE FUNCTIONS

    /// <summary>
    /// 
    /// </summary>
    /// <param name = "board"></param>
    /// <returns></returns>
    protected override int Generate_Row ( Board board )
    {
        return 0;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name = "board"></param>
    /// <param name = "row"></param>
    /// <returns></returns>
    protected override int Generate_Column( Board board, int row )
    {
        return 0;
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
