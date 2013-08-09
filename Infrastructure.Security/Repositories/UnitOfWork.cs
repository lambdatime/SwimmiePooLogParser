using System;

namespace SwimmiePooLogParserDispatcher.Infrastructure.Security.Repositories
{
    public class UnitOfWork : IDisposable
    {
        private SecurityContext _context = new SecurityContext();
        private UserProfileRepository _userProfileRepository;
        private ActiveRoleRepository _activeRoleRepository;

        public UnitOfWork()
        {
            _userProfileRepository = new UserProfileRepository(_context);
            _activeRoleRepository = new ActiveRoleRepository(_context);
        }

        public UserProfileRepository UserProfileRepository
        {
            get { return _userProfileRepository; }
        }

        public ActiveRoleRepository ActiveRoleRepository
        {
            get { return _activeRoleRepository; }
        }

        public int Save()
        {
            return _context.SaveChanges();
        }

        public SecurityContext Context
        {

            get
            {
                return _context;
            }
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}