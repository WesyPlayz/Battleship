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
}
