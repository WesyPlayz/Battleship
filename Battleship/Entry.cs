using Battleship;
using Battleship.AI;

/// <summary>
/// 
/// </summary>
public static class Entry 
{
    #region PUBLIC CONSTANT FIELDS

    public const int X = 10;
    public const int Y = 10;

    #endregion

    /// <summary>
    /// 
    /// </summary>
    public static void Main () 
    {
        Manager.Board_Size( X, Y );

        Manager.Add_Type( "Patrol_Boat", 2 );
        Manager.Add_Type( "Submarine", 3 );
        Manager.Add_Type( "Cruiser", 3 );
        Manager.Add_Type( "Interceptor", 4 );
        Manager.Add_Type( "Destroyer", 4 );
        Manager.Add_Type( "Carrier", 5 );

        Board p1 = new ( "Player" );
        BoardBot p2 = new ( "Bot" );
    }
}
