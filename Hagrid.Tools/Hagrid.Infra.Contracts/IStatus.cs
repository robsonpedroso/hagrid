namespace Hagrid.Infra.Contracts
{
    public interface IStatus<T> where T : struct
    {
        T Status { get; }
    }

    public interface IStatus : IStatus<bool> { }
}   