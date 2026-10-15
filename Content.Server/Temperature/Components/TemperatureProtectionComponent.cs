using Content.Server.Temperature.Systems;

namespace Content.Server.Temperature.Components;

[RegisterComponent]
[Access(typeof(TemperatureSystem))]
public sealed partial class TemperatureProtectionComponent : Component
{
    /// <summary>
    ///     Multiplier for the transferred heat when heating up or cooling down.
    /// </summary>
    [DataField]
    public float Coefficient = 1.0f;

    /// <summary>
    /// Multiplier for the transferred heat when heating up. If null, <see cref="Coefficient"/> is used.
    /// </summary>
    [DataField]
    public float? HeatingCoefficient;

    /// <summary>
    /// Multiplier for the transferred heat when cooling down. If null, <see cref="Coefficient"/> is used.
    /// </summary>
    [DataField]
    public float? CoolingCoefficient;
}

/// <summary>
/// Event raised on an entity with <see cref="TemperatureProtectionComponent"/> to determine the actual value of the coefficient.
/// </summary>
[ByRefEvent]
public record struct GetTemperatureProtectionEvent(float Coefficient);
