using System;
using System.Text.Json;
using FluentAssertions;
using Moq;
using Snapper.Core;
using Snapper.Json;
using Xunit;

namespace Snapper.Internals.Tests.Core;

public class SnapperCoreTests
{
    private readonly JsonElement _obj = JsonSerializer.SerializeToElement(new {value = 1});
    private readonly SnapperCore _snapperCore;
    private readonly Mock<ISnapshotStore> _store;
    private readonly Mock<ISnapshotUpdateDecider> _updateDecider;

    public SnapperCoreTests()
    {
        _store = new Mock<ISnapshotStore>();
        _updateDecider = new Mock<ISnapshotUpdateDecider>();

        _snapperCore = new SnapperCore(_store.Object, _updateDecider.Object);
    }

    [Fact]
    public void SnapshotMatches_ResultStatusIs_SnapshotsMatch()
    {
        _store.Setup(a => a.GetSnapshot(It.IsAny<SnapshotId>())).Returns(MakeJsonSnapshot(_obj));
        _updateDecider.Setup(a => a.ShouldUpdateSnapshot()).Returns(false);

        var result = _snapperCore.Snap(MakeJsonSnapshot(_obj));

        result.Status.Should().Be(SnapResultStatus.SnapshotsMatch);
        result.OldSnapshot.Value.Should().BeEquivalentTo(_obj);
        result.NewSnapshot.Value.Should().BeEquivalentTo(_obj);
    }

    [Fact]
    public void SnapshotMatches_ShouldUpdate_ResultStatusIs_SnapshotsMatch()
    {
        _store.Setup(a => a.GetSnapshot(It.IsAny<SnapshotId>())).Returns(MakeJsonSnapshot(_obj));
        _updateDecider.Setup(a => a.ShouldUpdateSnapshot()).Returns(true);

        var result = _snapperCore.Snap(MakeJsonSnapshot(_obj));

        _store.Verify(a => a.StoreSnapshot(It.IsAny<JsonSnapshot>()), Times.Never);
        result.Status.Should().Be(SnapResultStatus.SnapshotsMatch);
        result.OldSnapshot.Value.Should().BeEquivalentTo(_obj);
        result.NewSnapshot.Value.Should().BeEquivalentTo(_obj);
    }

    [Fact]
    public void SnapshotDoesNotMatch_ResultStatusIs_SnapshotsDoNotMatch()
    {
        _store.Setup(a => a.GetSnapshot(It.IsAny<SnapshotId>())).Returns(MakeJsonSnapshot(_obj));
        _updateDecider.Setup(a => a.ShouldUpdateSnapshot()).Returns(false);

        var newObj = JsonSerializer.SerializeToElement(new {value = 2});
        var result = _snapperCore.Snap(MakeJsonSnapshot(newObj));

        result.Status.Should().Be(SnapResultStatus.SnapshotsDoNotMatch);
        result.OldSnapshot.Value.Should().BeEquivalentTo(_obj);
        result.NewSnapshot.Value.Should().BeEquivalentTo(newObj);
    }

    [Fact]
    public void SnapshotDoesNotMatch_ShouldUpdate_ResultStatusIs_SnapshotUpdated()
    {
        _store.Setup(a => a.GetSnapshot(It.IsAny<SnapshotId>())).Returns(MakeJsonSnapshot(_obj));
        _updateDecider.Setup(a => a.ShouldUpdateSnapshot()).Returns(true);

        var newObj = JsonSerializer.SerializeToElement(new {value = 2});
        var result = _snapperCore.Snap(MakeJsonSnapshot(newObj));

        _store.Verify(a => a.StoreSnapshot(It.IsAny<JsonSnapshot>()), Times.Once);
        result.Status.Should().Be(SnapResultStatus.SnapshotUpdated);
        result.OldSnapshot.Value.Should().BeEquivalentTo(_obj);
        result.NewSnapshot.Value.Should().BeEquivalentTo(newObj);
    }

    [Fact]
    public void SnapshotDoesNotExist_ShouldUpdate_ResultStatusIs_SnapshotUpdated()
    {
        _store.Setup(a => a.GetSnapshot(It.IsAny<SnapshotId>())).Returns((JsonSnapshot) null);
        _updateDecider.Setup(a => a.ShouldUpdateSnapshot()).Returns(true);

        var result = _snapperCore.Snap(MakeJsonSnapshot(_obj));

        _store.Verify(a => a.StoreSnapshot(It.IsAny<JsonSnapshot>()), Times.Once);
        result.Status.Should().Be(SnapResultStatus.SnapshotUpdated);
        result.OldSnapshot.Should().BeNull();
        result.NewSnapshot.Value.Should().BeEquivalentTo(_obj);
    }

    [Fact]
    public void SnapshotDoesNotExist_AndIsNotCI_ResultStatusIs_SnapshotUpdated()
    {
        // Tests run on CI so clearing the CI environment variable to emulate local machine
        Environment.SetEnvironmentVariable("CI", null, EnvironmentVariableTarget.Process);

        _store.Setup(a => a.GetSnapshot(It.IsAny<SnapshotId>())).Returns((JsonSnapshot) null);
        _updateDecider.Setup(a => a.ShouldUpdateSnapshot()).Returns(false);

        var result = _snapperCore.Snap(MakeJsonSnapshot(_obj));

        _store.Verify(a => a.StoreSnapshot(It.IsAny<JsonSnapshot>()), Times.Once);
        result.Status.Should().Be(SnapResultStatus.SnapshotUpdated);
        result.OldSnapshot.Should().BeNull();
        result.NewSnapshot.Value.Should().BeEquivalentTo(_obj);
    }

    [Fact]
    public void SnapshotDoesNotExist_And_IsCiEnv_ResultStatusIs_SnapshotDoesNotExist()
    {
        Environment.SetEnvironmentVariable("CI", "true", EnvironmentVariableTarget.Process);
        _store.Setup(a => a.GetSnapshot(It.IsAny<SnapshotId>())).Returns((JsonSnapshot) null);
        _updateDecider.Setup(a => a.ShouldUpdateSnapshot()).Returns(false);

        var result = _snapperCore.Snap(MakeJsonSnapshot(_obj));

        result.Status.Should().Be(SnapResultStatus.SnapshotDoesNotExist);
        result.OldSnapshot.Should().BeNull();
        result.NewSnapshot.Value.Should().BeEquivalentTo(_obj);
        Environment.SetEnvironmentVariable("CI", null, EnvironmentVariableTarget.Process);
    }

    private static JsonSnapshot MakeJsonSnapshot(JsonElement obj)
        => new JsonSnapshot(new SnapshotId("", "", ""), obj);
}
