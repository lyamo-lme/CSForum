using AutoFixture;

namespace TestLibrary;

public static class AutoFixtureExtensions
{
    public static T CreateEntityWithoutThrowingRecursionError<T>(this Fixture fixture)
    {
        fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => fixture.Behaviors.Remove(b));
        fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        return fixture.Create<T>();
    }
}