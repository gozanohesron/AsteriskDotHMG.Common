namespace AsteriskDotHMG.Common.Helpers;

[AttributeUsage(AttributeTargets.Parameter)]
public sealed class FromMultiSourceAttribute : Attribute, IBindingSourceMetadata
{
    public BindingSource BindingSource { get; } = CompositeBindingSource.Create(
        new[] { BindingSource.Path, BindingSource.Query },
        nameof(FromMultiSourceAttribute));
}
