using System;
using System.Collections.Generic;
using System.Linq;

using Gwent.Core.Environment;
using Gwent.Core.Game;
using Gwent.Environment;
using Gwent.Game;
using Moq;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class GwentGameOperationsTests
    {
        private IGwentGameOperations _operations;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _operations = new GwentGameOperations();
        }

        [Test]
        public void SelectActiveAndOpponentPlayersRandomlyTests()
        {
            var player1Mock = new Mock<IPlayer>();
            var player2Mock = new Mock<IPlayer>();

            player1Mock.SetupProperty(player => player.PlayerNumber, (byte)1);
            player2Mock.SetupProperty(player => player.PlayerNumber, (byte)2);

            (IPlayer activePlayer, IPlayer opponentPlayer)
                = _operations.SelectActiveAndOpponentPlayersRandomly(player1Mock.Object, player2Mock.Object);

            if (activePlayer.PlayerNumber == 1)
            {
                Assert.AreEqual(player1Mock.Object, activePlayer);
            }

            if (activePlayer.PlayerNumber == 2)
            {
                Assert.AreEqual(player2Mock.Object, activePlayer);
            }

            if (opponentPlayer.PlayerNumber == 1)
            {
                Assert.AreEqual(player1Mock.Object, opponentPlayer);
            }

            if (opponentPlayer.PlayerNumber == 2)
            {
                Assert.AreEqual(player2Mock.Object, opponentPlayer);
            }
        }

        [Test]
        public void SwitchActiveAndOpponentPlayers()
        {
            var activePlayerMock = new Mock<IPlayer>();
            var opponentPlayerMock = new Mock<IPlayer>();

            (IPlayer activePlayer, IPlayer opponentPlayer)
                = _operations.SwitchActiveAndOpponentPlayers(activePlayerMock.Object, opponentPlayerMock.Object);

            Assert.AreNotEqual(activePlayerMock.Object, activePlayer);
            Assert.AreNotEqual(opponentPlayerMock.Object, opponentPlayer);
        }

        [Test]
        public void ReFillPlayerManaTests()
        {
            var playerMock = new Mock<IPlayer>();

            playerMock.SetupProperty(player => player.Mana, (byte)0);
            playerMock.SetupProperty(player => player.ManaSlots, (byte)2);

            _operations.RefillPlayerMana(playerMock.Object);

            Assert.AreEqual(3, playerMock.Object.Mana);

            playerMock.Object.ManaSlots = 0;

            _operations.RefillPlayerMana(playerMock.Object);

            Assert.AreEqual(1, playerMock.Object.Mana);
            Assert.AreEqual(1, playerMock.Object.ManaSlots);
        }

        [Test]
        public void RandomlyDrawOneCardFromPlayerDeckTests()
        {
            var playerMock = new Mock<IPlayer>();

            var playerDeck = new Deck();

            playerMock.Setup(player => player.Deck).Returns(playerDeck);

            ICard drawnCard = _operations.RandomlyDrawOneCardFromPlayerDeck(playerMock.Object);

            Assert.AreEqual(false, playerDeck.Cards.Any(card => card == drawnCard));

            playerDeck.Cards.Clear();

            drawnCard = _operations.RandomlyDrawOneCardFromPlayerDeck(playerMock.Object);

            Assert.AreEqual(null, drawnCard);
        }

        [Test]
        public void BleedingOutTests()
        {
            var playerMock = new Mock<IPlayer>();

            playerMock.SetupProperty(player => player.Health, (byte)10);

            playerMock.Object.Health = 10;

            bool isGameContinues = _operations.BleedingOut(playerMock.Object);

            Assert.AreEqual(9, playerMock.Object.Health);
            Assert.AreEqual(true, isGameContinues);

            playerMock.Object.Health = 1;

            isGameContinues = _operations.BleedingOut(playerMock.Object);

            Assert.AreEqual(0, playerMock.Object.Health);
            Assert.AreEqual(false, isGameContinues);
        }

        [Test]
        public void AddDrawnCardToTheHandTests()
        {
            var hand = new List<ICard>
            {
                new Card { ManaCost = 1 }, new Card { ManaCost = 2 },
                new Card { ManaCost = 3 }, new Card { ManaCost = 4 }
            };

            var drawnCard = new Card { ManaCost = 8 };

            var playerMock = new Mock<IPlayer>();

            playerMock.SetupGet(player => player.Hand).Returns(hand);

            bool isOverload = _operations.AddDrawnCardToTheHand(playerMock.Object, drawnCard);

            Assert.AreEqual(5, playerMock.Object.Hand.Count);
            Assert.AreEqual(false, isOverload);

            isOverload = _operations.AddDrawnCardToTheHand(playerMock.Object, drawnCard);

            Assert.AreEqual(5, playerMock.Object.Hand.Count);
            Assert.AreEqual(true, isOverload);
        }

        [Test]
        public void AreThereAnyPlayableCardInHandTests()
        {
            var hand = new List<ICard>
            {
                new Card { ManaCost = 1 }, new Card { ManaCost = 2 },
                new Card { ManaCost = 3 }, new Card { ManaCost = 4 }
            };

            bool areThereAnyPlayableCardInHand = _operations.AreThereAnyPlayableCardInHand(hand);

            Assert.AreEqual(true, areThereAnyPlayableCardInHand);

            hand.Clear();

            areThereAnyPlayableCardInHand = _operations.AreThereAnyPlayableCardInHand(hand);

            Assert.AreEqual(false, areThereAnyPlayableCardInHand);
        }

        [Test]
        public void IsThereEnoughManaForPlayabilityTests()
        {
            var hand = new List<ICard>
            {
                new Card { ManaCost = 1 }, new Card { ManaCost = 2 },
                new Card { ManaCost = 3 }, new Card { ManaCost = 4 }
            };

            byte manaOfPlayer = 2;

            bool isThereEnoughManaForPlayability = _operations.IsThereEnoughManaForPlayability(hand, manaOfPlayer);

            Assert.AreEqual(true, isThereEnoughManaForPlayability);

            hand = new List<ICard>
            {
                new Card { ManaCost = 5 }, new Card { ManaCost = 4 },
                new Card { ManaCost = 3 }, new Card { ManaCost = 6 }
            };

            isThereEnoughManaForPlayability = _operations.IsThereEnoughManaForPlayability(hand, manaOfPlayer);

            Assert.AreEqual(false, isThereEnoughManaForPlayability);
        }

        [Test]
        public void IsThereEnoughManaForSelectedCardTests()
        {
            byte manaOfPlayer = 2;
            byte manaCost = 3;

            bool isThereEnoughManaForSelectedCard = _operations.IsThereEnoughManaForSelectedCard(manaCost, manaOfPlayer);

            Assert.AreEqual(false, isThereEnoughManaForSelectedCard);

            manaOfPlayer = 4;
            manaCost = 4;

            isThereEnoughManaForSelectedCard = _operations.IsThereEnoughManaForSelectedCard(manaCost, manaOfPlayer);

            Assert.AreEqual(true, isThereEnoughManaForSelectedCard);
        }

        [Test]
        public void AttackTheOpponentTests()
        {
            var activePlayerMock = new Mock<IPlayer>();
            var opponentPlayerMock = new Mock<IPlayer>();

            activePlayerMock.SetupProperty(player => player.Mana, (byte)8);
            opponentPlayerMock.SetupProperty(player => player.Health, (byte)6);

            var selectedCard = new Card { ManaCost = 7 };

            bool isGameContinues = _operations.AttackTheOpponent(selectedCard, activePlayerMock.Object, opponentPlayerMock.Object);

            Assert.AreEqual(false, isGameContinues);
            Assert.AreEqual(0, opponentPlayerMock.Object.Health);
            Assert.AreEqual(1, activePlayerMock.Object.Mana);

            activePlayerMock.Object.Mana = 4;
            opponentPlayerMock.Object.Health = 19;
            selectedCard.ManaCost = 3;

            isGameContinues = _operations.AttackTheOpponent(selectedCard, activePlayerMock.Object, opponentPlayerMock.Object);

            Assert.AreEqual(true, isGameContinues);
            Assert.AreEqual(1, activePlayerMock.Object.Mana);
            Assert.AreEqual(16, opponentPlayerMock.Object.Health);

            activePlayerMock.Object.Mana = 1;

            try
            {
                isGameContinues = _operations.AttackTheOpponent(selectedCard, activePlayerMock.Object, opponentPlayerMock.Object);
            }
            catch (ArgumentOutOfRangeException)
            {
                Assert.AreEqual(1, activePlayerMock.Object.Mana);
                Assert.AreEqual(16, opponentPlayerMock.Object.Health);
                Assert.AreEqual(true, isGameContinues);
            }
        }

        [Test]
        public void RemoveUsedCardFromHandTests()
        {
            var hand = new List<ICard>
            {
                new Card { ManaCost = 1 }, new Card { ManaCost = 2 },
                new Card { ManaCost = 3 }, new Card { ManaCost = 4 }
            };

            ICard usedCard = hand[2];

            _operations.RemoveUsedCardFromHand(hand, usedCard);

            Assert.AreEqual(3, hand.Count);
            Assert.AreNotEqual(true, hand.Any(card => card.ManaCost == 3));
        }
    }
}