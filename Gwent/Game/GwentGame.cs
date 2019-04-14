using Gwent.Core.Environment;
using Gwent.Core.Game;

namespace Gwent.Game
{
    /// <summary>
    /// Contains all the Gwent game logic.
    /// </summary>
    public class GwentGame
    {
        private readonly IGwentGameOperations _operations;
        private readonly IGwentGameIO _io;

        private IPlayer _activePlayer;
        private IPlayer _opponentPlayer;

        private bool _isGameContinuous = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="GwentGame"/> class. 
        /// </summary>
        public GwentGame(IPlayer player1, IPlayer player2, IGwentGameOperations operations, IGwentGameIO io)
        {
            _operations = operations;
            _io = io;

            player1.PlayerNumber = 1;
            player2.PlayerNumber = 2;

            (_activePlayer, _opponentPlayer) = _operations.SelectActiveAndOpponentPlayersRandomly(player1, player2);
        }

        /// <summary>
        /// Starts the gwent game.
        /// </summary>
        public void Start()
        {
            _io.StartMessage();

            _io.RandomlySelectedPlayerStartsTheGameMessage(_activePlayer.PlayerNumber);

            // The main game loop.
            while (_isGameContinuous)
            {
                GwentGameTurn();
            }

            _io.EndMessage();
        }

        /// <summary>
        /// A new Gwent game turn.
        /// </summary>
        private void GwentGameTurn()
        {
            _io.NewGwentGameTurnMessage(_activePlayer);

            RandomlyCardDrawOperations();

            _operations.RefillPlayerMana(_activePlayer);

            if (!_isGameContinuous)
            {
                return;
            }

            bool isActivePlayerContinue;
            do
            {
                _io.CurrentGameStatusMessage(_activePlayer, _opponentPlayer);

                isActivePlayerContinue = ContinuousTurnActions();
            }
            while (isActivePlayerContinue);

            _io.ClearScreenMessage();
        }

        /// <summary>
        /// Randomly card draw operations prior to game turn.
        /// </summary>
        private void RandomlyCardDrawOperations()
        {
            ICard drawnCard = _operations.RandomlyDrawOneCardFromPlayerDeck(_activePlayer);

            _io.InfoAboutRandomlyDrawnCardMessage(drawnCard);

            if (drawnCard == null)
            {
                _isGameContinuous = _operations.BleedingOut(_activePlayer);

                if (!_isGameContinuous)
                {
                    _io.YouDefeatedMessage(_activePlayer, _opponentPlayer);
                }

                return;
            }

            bool isOverloadedHand = _operations.AddDrawnCardToTheHand(_activePlayer, drawnCard);

            if (isOverloadedHand)
            {
                _io.RandomlyDrawnCardCouldNotAddedMessage();
            }
        }

        /// <summary>
        /// Active player's continuously turn actions.
        /// </summary>
        /// <returns>Info about "Will the active player continue to play?"</returns>
        private bool ContinuousTurnActions()
        {
            if (!CheckPlayability())
            {
                (_activePlayer, _opponentPlayer) = _operations.SwitchActiveAndOpponentPlayers(_activePlayer, _opponentPlayer);

                return false;
            }

            if (!AttackCardSelection(out ICard selectedAttackCard))
            {
                (_activePlayer, _opponentPlayer) = _operations.SwitchActiveAndOpponentPlayers(_activePlayer, _opponentPlayer);

                return false;
            }

            _isGameContinuous = _operations.AttackTheOpponent(selectedAttackCard, _activePlayer, _opponentPlayer);

            _operations.RemoveUsedCardFromHand(_activePlayer.Hand, selectedAttackCard);

            if (_isGameContinuous)
            {
                return true;
            }

            _io.YouWinMessage(_activePlayer, _opponentPlayer);

            return false;
        }

        /// <summary>
        /// Checks active player statuses for game play.
        /// </summary>
        /// <returns>Returns false if active player can not continue to play, otherwise returns true</returns>
        private bool CheckPlayability()
        {
            if (!_operations.AreThereAnyPlayableCardInHand(_activePlayer.Hand))
            {
                _io.ThereAreNotAnyPlayableCardMessage();

                return false;
            }

            if (_operations.IsThereEnoughManaForPlayability(_activePlayer.Hand, _activePlayer.Mana))
            {
                return true;
            }

            _io.NotEnoughManaToPlayAnyCardMessage();

            return false;
        }

        /// <summary>
        /// Selects attack card through asking to player.
        /// </summary>
        /// <returns>Returns true if card successfully selected, otherwise returns false</returns>
        private bool AttackCardSelection(out ICard selectedAttackCard)
        {
            selectedAttackCard = _io.AskToPlayerForAttackCardSelection(_activePlayer);

            if (selectedAttackCard == null)
            {
                return false;
            }

            if (_operations.IsThereEnoughManaForSelectedCard(selectedAttackCard.ManaCost, _activePlayer.Mana))
            {
                return true;
            }

            _io.NotEnoughManaToPlaySelectedCardMessage();

            return AttackCardSelection(out selectedAttackCard);
        }
    }
}