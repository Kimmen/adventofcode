namespace Aoc;

public interface IChallenge
{
    void UseDevInput();
    void SetSpeed(int millisecondsDelay);
    void Run();
}