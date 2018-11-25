using System;
using Microsoft.EntityFrameworkCore;
using NewsPublish.Model.Entity;

namespace NewsPublish.Service
{
    public class Db:DbContext
    {

        public Db(DbContextOptions<Db> options ):base(options){

        }

        public Db(string connection)
        {
            this._connection = connection;
        }
        private string _connection;
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            if (!string.IsNullOrWhiteSpace (_connection))
            {
                optionsBuilder.UseMySql(_connection);
            }
        }
        public DbSet<Banner> Banner
        {
            get;
            set;
        }
        public DbSet<NewsClassify> NewsClassify
        {
            get;
            set;
        }
        public DbSet<News> News
        {
            get;
            set;
        }
        public DbSet<NewsComment> NewsComment
        {
            get;
            set;
        }

    }
}
