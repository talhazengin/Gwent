using Gwent.Core.Environment;

namespace Gwent.Core.Game
{
    /// <summary>
    /// Represents game input output mechanisms.
    /// </summary>
    public interface IGwentGameIO
    {
        void EndMessage();

        void StartMessage();

        void RandomlySelectedPlayerStartsTheGameMessage(int playerNumber);

        void CurrentGameStatusMessage(IPlayer activePlayer, IPlayer opponentPlayer);

        void InfoAboutRandomlyDrawnCardMessage(ICard drawnCard);

        /// <summary>
        /// Asks to player for attack card selection from player's hand.
        /// </summary>
        /// <returns>Returns selected card or null if not selected</returns>
        ICard AskToPlayerForAttackCardSelection(IPlayer player);

        void NewGwentGameTurnMessage(IPlayer activePlayer);

        void ClearScreenMessage();

        void ThereAreNotAnyPlayableCardMessage();

        void NotEnoughManaToPlayAnyCardMessage();

        void NotEnoughManaToPlaySelectedCardMessage();

        void YouWinMessage(IPlayer activePlayer, IPlayer opponentPlayer);

        void YouDefeatedMessage(IPlayer activePlayer, IPlayer opponentPlayer);

        void RandomlyDrawnCardCouldNotAddedMessage();
    }
}