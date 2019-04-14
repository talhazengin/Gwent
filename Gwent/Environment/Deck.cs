using System.Collections.Generic;
using System.Linq;

using Gwent.Core.Environment;

namespace Gwent.Environment
{
    public class Deck : IDeck
    {
        public Deck()
        {
            InitCards();
        }

        public IList<ICard> Cards { get; private set; }

        private void InitCards()
        {
            var manaCosts = new byte[] { 0, 0, 1, 1, 2, 2, 2, 3, 3, 3, 3, 4, 4, 4, 5, 5, 6, 6, 7, 8 };

            Cards = manaCosts.Select(manaCost => new Card { ManaCost = manaCost }).Cast<ICard>().ToList();
        }
    }
}