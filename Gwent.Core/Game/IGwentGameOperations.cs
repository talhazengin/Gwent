using System.Collections.Generic;

using Gwent.Core.Environment;

namespace Gwent.Core.Game
{
    /// <summary>
    /// Represents logical game operations.
    /// </summary>
    public interface IGwentGameOperations
    {
        (IPlayer activePlayer, IPlayer opponentPlayer) SelectActiveAndOpponentPlayersRandomly(IPlayer player1, IPlayer player2);

        (IPlayer activePlayer, IPlayer opponentPlayer) SwitchActiveAndOpponentPlayers(IPlayer activePlayer, IPlayer opponentPlayer);

        void RefillPlayerMana(IPlayer player);

        ICard RandomlyDrawOneCardFromPlayerDeck(IPlayer player);

        /// <summary>
        /// Applies bleeding out special rule to player.
        /// </summary>
        /// <returns>Info about "Is game continuous?"</returns>
        bool BleedingOut(IPlayer player);

        /// <summary>
        /// Adds drawn card to the hand of the player.
        /// </summary>
        /// <returns>If hand overloaded then returns true otherwise returns false</returns>
        bool AddDrawnCardToTheHand(IPlayer player, ICard drawnCard);

        bool AreThereAnyPlayableCardInHand(IEnumerable<ICard> hand);

        bool IsThereEnoughManaForPlayability(IEnumerable<ICard> hand, byte mana);

        bool IsThereEnoughManaForSelectedCard(byte cardManaCost, byte mana);

        /// <summary>
        /// Attacks to opponent player with selected card.
        /// </summary>
        /// <returns>Info about "Is game continuous?"</returns>
        bool AttackTheOpponent(ICard selectedAttackCard, IPlayer activePlayer, IPlayer opponentPlayer);

        void RemoveUsedCardFromHand(IList<ICard> hand, ICard usedCard);
    }
}