using Gwent.Core.Environment;

namespace Gwent.Environment
{
    public class Card : ICard
    {
        public byte ManaCost { get; set; }

        public byte Damage => ManaCost;
    }
}
