namespace Gwent.Core.Environment
{
    public interface ICard
    {
        byte ManaCost { get; set; }

        byte Damage { get; }
    }
}