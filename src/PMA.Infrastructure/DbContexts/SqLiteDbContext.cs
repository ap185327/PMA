// <copyright file="SqLiteDbContext.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using Microsoft.EntityFrameworkCore;
using PMA.Infrastructure.Entities;
using System.IO;
using System.Reflection;

namespace PMA.Infrastructure.DbContexts
{
    /// <summary>
    ///  A SqLiteDbContext instance represents a session with the database PMA and can be used to query and save instances of entities.
    /// </summary>
    public class SqLiteDbContext : DbContext
    {
        /// <summary>
        /// The data source.
        /// </summary>
        private readonly string _dataSource;

        /// <summary>
        /// Initializes the new instance of <see cref="SqLiteDbContext"/> class.
        /// </summary>
        /// <param name="dataSource">The data source.</param>
        public SqLiteDbContext(string dataSource)
        {
            _dataSource = Path.GetFullPath(dataSource);
        }

        #region DbSets

        /// <summary>
        /// Gets or set a database set of morphological combinations.
        /// </summary>
        public DbSet<MorphCombinationEntity> MorphCombinations { get; set; }

        /// <summary>
        /// Gets or set a database set of morphological entries.
        /// </summary>
        public DbSet<MorphEntryEntity> MorphEntries { get; set; }

        /// <summary>
        /// Gets or set a database set of morphological parameters.
        /// </summary>
        public DbSet<MorphParameterEntity> MorphParameters { get; set; }

        /// <summary>
        /// Gets or set a database set of morphological rules.
        /// </summary>
        public DbSet<MorphRuleEntity> MorphRules { get; set; }

        /// <summary>
        /// Gets or set a database set of sandhi groups.
        /// </summary>
        public DbSet<SandhiGroupEntity> SandhiGroups { get; set; }

        /// <summary>
        /// Gets or set a database set of sandhi results.
        /// </summary>
        public DbSet<SandhiResultEntity> SandhiResults { get; set; }

        /// <summary>
        /// Gets or set a database set of sandhi rules.
        /// </summary>
        public DbSet<SandhiRuleEntity> SandhiRules { get; set; }

        /// <summary>
        /// Gets or set a database set of settings.
        /// </summary>
        public DbSet<SettingEntity> Settings { get; set; }

        /// <summary>
        /// Gets or set a database set of strings.
        /// </summary>
        public DbSet<StringEntity> Strings { get; set; }

        /// <summary>
        /// Gets or set a database set of terms.
        /// </summary>
        public DbSet<TermEntity> Terms { get; set; }

        /// <summary>
        /// Gets or set a database set of word frequencies.
        /// </summary>
        public DbSet<FreqGram1Entity> FreqGram1Entities { get; set; }

        #endregion

        #region Overrides of DbContext

        /// <summary>
        ///     <para>
        ///         Override this method to configure the database (and other options) to be used for this context.
        ///         This method is called for each instance of the context that is created.
        ///         The base implementation does nothing.
        ///     </para>
        ///     <para>
        ///         In situations where an instance of <see cref="T:Microsoft.EntityFrameworkCore.DbContextOptions" /> may or may not have been passed
        ///         to the constructor, you can use <see cref="P:Microsoft.EntityFrameworkCore.DbContextOptionsBuilder.IsConfigured" /> to determine if
        ///         the options have already been set, and skip some or all of the logic in
        ///         <see cref="M:Microsoft.EntityFrameworkCore.DbContext.OnConfiguring(Microsoft.EntityFrameworkCore.DbContextOptionsBuilder)" />.
        ///     </para>
        /// </summary>
        /// <param name="optionsBuilder">
        ///     A builder used to create or modify options for this context. Databases (and other extensions)
        ///     typically define extension methods on this object that allow you to configure the context.
        /// </param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source={_dataSource}", options =>
            {
                options.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName);
            });

            base.OnConfiguring(optionsBuilder);
        }

        /// <summary>
        ///     Override this method to further configure the model that was discovered by convention from the entity types
        ///     exposed in <see cref="T:Microsoft.EntityFrameworkCore.DbSet`1" /> properties on your derived context. The resulting model may be cached
        ///     and re-used for subsequent instances of your derived context.
        /// </summary>
        /// <remarks>
        ///     If a model is explicitly set on the options for this context (via <see cref="M:Microsoft.EntityFrameworkCore.DbContextOptionsBuilder.UseModel(Microsoft.EntityFrameworkCore.Metadata.IModel)" />)
        ///     then this method will not be run.
        /// </remarks>
        /// <param name="modelBuilder">
        ///     The builder being used to construct the model for this context. Databases (and other extensions) typically
        ///     define extension methods on this object that allow you to configure aspects of the model that are specific
        ///     to a given database.
        /// </param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FreqGram1Entity>(builder =>
            {
                builder.HasNoKey();
                builder.ToTable("Freq1Gram");
            });
        }

        #endregion

        /// <summary>
        /// Resets a primary key sequence.
        /// </summary>
        /// <param name="tableName">The table name.</param>
        public void ResetPrimaryKeySequence(string tableName)
        {
            Database.ExecuteSqlRaw($"UPDATE SQLITE_SEQUENCE SET SEQ=0 WHERE NAME='{tableName}';");
        }
    }
}
