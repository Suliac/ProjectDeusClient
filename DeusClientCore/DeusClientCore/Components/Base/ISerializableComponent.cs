namespace DeusClientCore.Components
{
    public interface ISerializableComponent : ISerializable
    {
        uint ComponentId { get; }
        EComponentType ComponentType { get; }
    }
}