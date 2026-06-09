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

        p1.Place( "Patrol_Boat" );
        p1.Place( "Submarine" );
        p1.Place( "Cruiser" );
        p1.Place( "Interceptor" );
        p1.Place( "Destroyer" );
        p1.Place( "Carrier" );

        p2.Place( "Patrol_Boat" );
        p2.Place( "Submarine" );
        p2.Place( "Cruiser" );
        p2.Place( "Interceptor" );
        p2.Place( "Destroyer" );
        p2.Place( "Carrier" );

        string winner = "IN PROGRESS";

        while ( winner.Equals( "IN PROGRESS" ) )
        {
            p2.Hit( p1 );

            if ( p1.Status() )
            {
                winner = "Bot";

                break;
            }
            Console.WriteLine();
            p1.Display_Boats();
            Console.WriteLine();
            p1.Hit( p2 );

            if ( p2.Status() )
            {
                winner = "Player";

                break;
            }
            Console.WriteLine();
        }
        Console.WriteLine( "\nPlayer Board\n" );
        p1.Display_Boats();
        
        Console.WriteLine( "\nBot Board\n" );
        p2.Display_Boats();

        Console.WriteLine( $"\nThe winner is the { winner }!" );
    }
}
