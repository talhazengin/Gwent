using System;
using System.Collections.Generic;
using System.Linq;

using Gwent.Core.Environment;
using Gwent.Core.Game;

namespace Gwent.Game
{
    public class GwentGameOperations : IGwentGameOperations
    {
        private readonly Random _randomGenerator = new Random();

        public (IPlayer activePlayer, IPlayer opponentPlayer) SelectActiveAndOpponentPlayersRandomly(IPlayer player1, IPlayer player2)
        {
            return _randomGenerator.Next(2) == 0 ? (player1, player2) : (player2, player1);
        }

        public (IPlayer activePlayer, IPlayer opponentPlayer) SwitchActiveAndOpponentPlayers(IPlayer activePlayer, IPlayer opponentPlayer)
        {
            return (opponentPlayer, activePlayer);
        }

        public void RefillPlayerMana(IPlayer player)
        {
            // Rearrange mana slot of the player.
            if (player.ManaSlots < 10)
            {
                player.ManaSlots++;
            }

            // Refill mana of the player.
            player.Mana = player.ManaSlots;
        }

        public ICard RandomlyDrawOneCardFromPlayerDeck(IPlayer player)
        {
            int cardsCount = player.Deck.Cards.Count;

            if (cardsCount == 0)
            {
                return null;
            }

            int drawnCardIndex = _randomGenerator.Next(cardsCount);

            ICard drawnCard = player.Deck.Cards[drawnCardIndex];

            // Remove drawn card from the player's deck.
            player.Deck.Cards.RemoveAt(drawnCardIndex);

            return drawnCard;
        }

        public bool BleedingOut(IPlayer player)
        {
            // Active player loses the game.
            if (player.Health <= 1)
            {
                player.Health = 0;
                return false;
            }

            player.Health--;

            return true;
        }

        public bool AddDrawnCardToTheHand(IPlayer player, ICard drawnCard)
        {
            // Overload rule applied.
            if (player.Hand.Count >= 5)
            {
                return true;
            }

            // Add drawn card to the player's hand.
            player.Hand.Add(drawnCard);

            return false;
        }

        public bool AreThereAnyPlayableCardInHand(IEnumerable<ICard> hand)
        {
            return hand.Any();
        }

        public bool IsThereEnoughManaForPlayability(IEnumerable<ICard> hand, byte mana)
        {
            return hand.Any(card => mana >= card.ManaCost);
        }

        public bool IsThereEnoughManaForSelectedCard(byte cardManaCost, byte mana)
        {
            return mana >= cardManaCost;
        }

        public bool AttackTheOpponent(ICard selectedAttackCard, IPlayer activePlayer, IPlayer opponentPlayer)
        {
            if (selectedAttackCard.ManaCost > activePlayer.Mana)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(selectedAttackCard),
                    "Selected card's mana cost can not bigger than active player's mana");
            }

            // Calculate new mana, mana should not lower than the mana cost.
            activePlayer.Mana -= selectedAttackCard.ManaCost;

            // Opponent player loses the game.
            if (selectedAttackCard.Damage >= opponentPlayer.Health)
            {
                opponentPlayer.Health = 0;
                return false;
            }

            opponentPlayer.Health -= selectedAttackCard.Damage;

            return true;
        }

        public void RemoveUsedCardFromHand(IList<ICard> hand, ICard usedCard)
        {
            hand.Remove(usedCard);
        }
    }
}
