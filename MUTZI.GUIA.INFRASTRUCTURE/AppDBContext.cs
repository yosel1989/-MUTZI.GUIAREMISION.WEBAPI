using Microsoft.EntityFrameworkCore.Storage;
using MUTZI.GUIA.DOMAIN.Interfaces;
using MUTZI.GUIA.INFRASTRUCTURE.Persistence;

namespace MUTZI.GUIA.INFRASTRUCTURE
{
    public class AppDBContext : IAppDBContext, IDisposable
    {
        private readonly LogisticaContext _context;
        private IDbContextTransaction? _transaction;

        public AppDBContext(LogisticaContext context)
        {
            _context = context;
        }

        public async Task BeginTransactionAsync()
        {
            // Si ya existe una transacción activa, no abrir otra
            if (_transaction == null)
            {
                _transaction = await _context.Database.BeginTransactionAsync();
            }
        }

        public async Task CommitAsync()
        {
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public async Task RollbackAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public void Dispose()
        {
            _transaction?.Dispose();
        }
    }
}
