
namespace Auto_Technical_Center
{
    internal class AppDbContext : IDisposable
    {
        public object Clients { get; internal set; }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        internal void SaveChanges()
        {
            throw new NotImplementedException();
        }
    }
}