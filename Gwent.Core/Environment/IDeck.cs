using System.Collections.Generic;

namespace Gwent.Core.Environment
{
    public interface IDeck
    {
        IList<ICard> Cards { get; }
    }
}