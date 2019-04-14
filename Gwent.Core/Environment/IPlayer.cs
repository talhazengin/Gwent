using System.Collections.Generic;

namespace Gwent.Core.Environment
{
    public interface IPlayer
    {
        byte PlayerNumber { get; set; }

        byte Health { get; set; }

        byte Mana { get; set; }

        byte ManaSlots { get; set; }

        IDeck Deck { get; }

        IList<ICard> Hand { get; }
    }
}