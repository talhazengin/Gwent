using System;
using System.Collections.Generic;

using Gwent.Core.Environment;

namespace Gwent.Environment
{
    public class Player : IPlayer
    {
        public Player(IDeck deck)
        {
            Deck = deck;
            InitHand();
        }

        public byte PlayerNumber { get; set; }

        public byte Health { get; set; } = 30;

        public byte Mana { get; set; }

        public byte ManaSlots { get; set; }

        public IDeck Deck { get; }

        public IList<ICard> Hand { get; private set; }

        private void InitHand()
        {
            Hand = new List<ICard>();

            var randomGenerator = new Random();

            // Make 3 randomize and assign the chosen cards to our initial hand.
            for (int i = 0; i < 3; i++)
            {
                int randomIndex = randomGenerator.Next(Deck.Cards.Count);

                // Add chosen card to our initial hand.
                Hand.Add(Deck.Cards[randomIndex]);

                // Remove chosen card from the deck.
                Deck.Cards.RemoveAt(randomIndex);
            }
        }
    }
}