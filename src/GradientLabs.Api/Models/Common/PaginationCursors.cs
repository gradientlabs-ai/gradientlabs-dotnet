namespace GradientLabs.Api;

internal sealed class PaginationCursors
{
    public string? Next { get; init; }
    public string? Prev { get; init; }

    internal Page<T> ToPage<T>(IReadOnlyList<T> items) => new() { Items = items, Next = Next, Prev = Prev };
}
