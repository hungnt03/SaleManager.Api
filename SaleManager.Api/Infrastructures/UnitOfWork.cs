using SaleManager.Api.Infrastructures.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SaleManager.Api.Infrastructures
{
    public interface IUnitOfWork
    {
        GenericRepository<Category> CategoryRepository { get; }

        void Commit();
        void Rollback();
    }
    public class UnitOfWork : IDisposable, IUnitOfWork
    {
        private ApplicationDbContext context;
        private bool disposed = false;
        private GenericRepository<Category> categoryRepository;
        
        public UnitOfWork(ApplicationDbContext context)
        {
            this.context = context;
        }
        public GenericRepository<Category> CategoryRepository
        {
            get
            {
                if (this.categoryRepository == null) 
                    this.categoryRepository = new GenericRepository<Category>(context);
                return categoryRepository;
            }
        }

        GenericRepository<Category> IUnitOfWork.CategoryRepository => CategoryRepository;

        public void Commit()
        {
            context.SaveChanges();
        }
        public void Rollback()
        {
            context.ChangeTracker.Entries().ToList().ForEach(x => x.Reload());
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
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
