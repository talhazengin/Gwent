using System;
using System.Linq;

using Gwent.Core.Environment;
using Gwent.Core.Game;

namespace Gwent.Game
{
    public class GwentGameIO : IGwentGameIO
    {
        public void StartMessage()
        {
            Console.WriteLine("\n*-*-*-*-*-*-*- WELCOME TO GWENT GAME -*-*-*-*-*-*-*");
        }

        public void NewGwentGameTurnMessage(IPlayer activePlayer)
        {
            Console.WriteLine($"\nPlayer{activePlayer.PlayerNumber} has started a new Gwent turn..");
        }

        public void RandomlySelectedPlayerStartsTheGameMessage(int playerNumber)
        {
            Console.WriteLine($"\nRandomly selected Player {playerNumber} starting the game..");
            Console.WriteLine("\n*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*");
        }

        public void CurrentGameStatusMessage(IPlayer activePlayer, IPlayer opponentPlayer)
        {
            Console.WriteLine($"\n\nCURRENT GAME STATUS:");
            Console.WriteLine($"\nOpponent Player's Health is {opponentPlayer.Health} ..");
            Console.WriteLine($"Your Health is {activePlayer.Health} ..");
            Console.WriteLine($"Your Mana is {activePlayer.Mana} ..");
            Console.WriteLine("Your hand's card damage powers: [{0}]\n", string.Join(", ", activePlayer.Hand.Select(card => card.Damage)));
        }

        public void InfoAboutRandomlyDrawnCardMessage(ICard drawnCard)
        {
            Console.WriteLine(
                drawnCard == null
                    ? "\nThere are not any cards in your deck.. You received 1 extra damage.."
                    : $"\nRandomly selected card from the deck has {drawnCard.Damage} damage power..");
        }

        public ICard AskToPlayerForAttackCardSelection(IPlayer player)
        {
            Console.WriteLine("\nWhich card will be use for attack? \nChoose a card index or type 'N/n' for releasing your turn.. \n");

            foreach ((ICard card, int index) in player.Hand.Select((card, i) => (card, i)))
            {
                Console.WriteLine($"Card: {index} -> Damage: {card.Damage}");
            }

            Console.WriteLine();

            ConsoleKeyInfo inputKeyInfo = Console.ReadKey();

            if (inputKeyInfo.Key == ConsoleKey.N)
            {
                return null;
            }

            if (int.TryParse(inputKeyInfo.KeyChar.ToString(), out int selectedCardIndex))
            {
                if (selectedCardIndex >= 0 && selectedCardIndex < player.Hand.Count)
                {
                    return player.Hand[selectedCardIndex];
                }
            }

            // If input is anything else, then force player to a valid input.
            Console.WriteLine("\nPlease make a valid choose..");

            return AskToPlayerForAttackCardSelection(player);
        }

        public void ClearScreenMessage()
        {
            Console.Write("\n\nTurn is over press a key to continue..");
            Console.ReadKey();
            Console.Clear();
        }

        public void ThereAreNotAnyPlayableCardMessage()
        {
            Console.WriteLine("\nThere are not any playable card in your hand..");
        }

        public void NotEnoughManaToPlayAnyCardMessage()
        {
            Console.WriteLine("\nThere is not enough mana in your hand to play more card..");
        }

        public void NotEnoughManaToPlaySelectedCardMessage()
        {
            Console.WriteLine("\nThere is not enough mana to play selected card..");
        }

        public void YouWinMessage(IPlayer activePlayer, IPlayer opponentPlayer)
        {
            Console.WriteLine("\n\nYou win ..");
            Console.WriteLine($"\nPlayer{opponentPlayer.PlayerNumber} has defeated. Player{activePlayer.PlayerNumber} has won..");
        }

        public void YouDefeatedMessage(IPlayer activePlayer, IPlayer opponentPlayer)
        {
            Console.WriteLine("\n\nYou defeated (bleeding out)..");
            Console.WriteLine($"\nPlayer{activePlayer.PlayerNumber} has defeated. Player{opponentPlayer.PlayerNumber} has won..");
        }
        
        public void RandomlyDrawnCardCouldNotAddedMessage()
        {
            Console.WriteLine("\nRandomly drawn card could not added to your hand, because your hand overloaded..");
        }

        public void EndMessage()
        {
            Console.WriteLine("\n*-*-*-*-*-*-*- THANKS FOR PLAYING GWENT -*-*-*-*-*-*-*");
            Console.ReadKey();
        }
    }
}
