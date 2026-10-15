using Content.Server.Temperature.Components;
using Content.Server.Temperature.Systems;
using Content.IntegrationTests.Fixtures;
using Content.Shared.Temperature.Components;
using Robust.Shared.GameObjects;
using Robust.Shared.Map;

namespace Content.IntegrationTests.Tests.Temperature;

[TestFixture]
public sealed class TemperatureProtectionTest : GameTest
{
    [Test]
    public async Task VulpkaninHeatProtectionTest()
    {
        var entityManager = Server.ResolveDependency<IEntityManager>();
        EntityUid heatingVulp = default;
        EntityUid coolingVulp = default;

        await Server.WaitPost(() =>
        {
            heatingVulp = entityManager.SpawnEntity("MobVulpkanin", MapCoordinates.Nullspace);
            coolingVulp = entityManager.SpawnEntity("MobVulpkanin", MapCoordinates.Nullspace);
        });
        await Pair.RunTicksSync(1);

        await Server.WaitAssertion(() =>
        {
            var temperatureSystem = entityManager.System<TemperatureSystem>();
            var heatingTemperature = entityManager.GetComponent<TemperatureComponent>(heatingVulp);
            var coolingTemperature = entityManager.GetComponent<TemperatureComponent>(coolingVulp);
            var protection = entityManager.GetComponent<TemperatureProtectionComponent>(heatingVulp);
            var heatingNormalTemperature = heatingTemperature.Temperature;
            var coolingNormalTemperature = coolingTemperature.Temperature;

            temperatureSystem.ConductHeat((heatingVulp, heatingTemperature), heatingNormalTemperature + 100f, 1f);
            temperatureSystem.ConductHeat((coolingVulp, coolingTemperature), coolingNormalTemperature - 100f, 1f);
            var heatingDelta = heatingTemperature.Temperature - heatingNormalTemperature;
            var coolingDelta = coolingNormalTemperature - coolingTemperature.Temperature;

            Assert.That(protection.HeatingCoefficient, Is.EqualTo(1.2f));
            Assert.That(protection.CoolingCoefficient, Is.EqualTo(0.3f));
            Assert.That(heatingDelta / coolingDelta, Is.EqualTo(4f).Within(0.01f));
        });
    }
}
