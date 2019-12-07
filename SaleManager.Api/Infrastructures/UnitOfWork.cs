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
        GenericRepository<Supplier> SupplierRepository { get; }
        GenericRepository<Customer> CustomerRepository { get; }
        GenericRepository<Product> ProductRepository { get; }

        void Commit();
        void Rollback();
    }
    public class UnitOfWork : IDisposable, IUnitOfWork
    {
        private ApplicationDbContext context;
        private bool disposed = false;
        private GenericRepository<Category> categoryRepository;
        private GenericRepository<Supplier> supplierRepository;
        private GenericRepository<Customer> customerRepository;
        private GenericRepository<Product> productRepository;

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
        public GenericRepository<Supplier> SupplierRepository
        {
            get
            {
                if (this.supplierRepository == null)
                    this.supplierRepository = new GenericRepository<Supplier>(context);
                return supplierRepository;
            }
        }
        public GenericRepository<Customer> CustomerRepository
        {
            get
            {
                if (this.customerRepository == null)
                    this.customerRepository = new GenericRepository<Customer>(context);
                return customerRepository;
            }
        }
        public GenericRepository<Product> ProductRepository
        {
            get
            {
                if (this.productRepository == null)
                    this.productRepository = new GenericRepository<Product>(context);
                return productRepository;
            }
        }

        GenericRepository<Category> IUnitOfWork.CategoryRepository => CategoryRepository;
        GenericRepository<Supplier> IUnitOfWork.SupplierRepository => SupplierRepository;
        GenericRepository<Product> IUnitOfWork.ProductRepository => ProductRepository;

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
