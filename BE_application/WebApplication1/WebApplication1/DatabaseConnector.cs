using System;
using System.Data;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Dialect;
using NHibernate.Driver;
using NHibernate.Mapping.ByCode;
using NHibernate.Tool.hbm2ddl;
using System.Reflection;
using WebApplication1.Classes;

namespace WebApplication1
{
    public class DatabaseConnector
    {
        protected static Configuration? NHConfig;
        protected static ISessionFactory? SessionFactory;

        private static Configuration ConfigureNHibernate() 
        {
            var configure = new Configuration();
            configure.SessionFactoryName("BuildIt");

            configure.DataBaseIntegration(db =>
            {
                db.Dialect<SQLiteDialect>();
                db.Driver<SQLite20Driver>();
                db.KeywordsAutoImport = Hbm2DDLKeyWords.AutoQuote;
                db.IsolationLevel = IsolationLevel.ReadCommitted;
                db.ConnectionString = "Data Source=C:\\Users\\msafran\\Documents\\school\\IV\\PIRaMS\\VSB_PIRaMS_project\\sample-database-sqlite-1\\Chinook.db";
                db.Timeout = 10;

                db.LogFormattedSql = true;
                db.LogSqlInConsole = true;
                db.AutoCommentSql = true;
            });

            return configure;
        }
    
        protected static HbmMapping GetMappings()
        {
            ModelMapper mapper = new ModelMapper();

            mapper.AddMapping<EmployeeMap>();

            return mapper.CompileMappingFor(new[] {typeof(Employee)});
        }
    
        public static void Setup()
        {
            NHConfig = ConfigureNHibernate();
            HbmMapping mapping = GetMappings();
            NHConfig.AddDeserializedMapping(mapping, "WebApplication1");
            SchemaMetadataUpdater.QuoteTableAndColumns(NHConfig);
            SessionFactory = NHConfig.BuildSessionFactory();
        }

    }
}
