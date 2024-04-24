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
using ISession = NHibernate.ISession;
using NHibernate.Criterion;

namespace WebApplication1
{
    public class DatabaseHandler
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
                db.ConnectionString = "Data Source=..\\..\\..\\sample-database-sqlite-1\\Chinook.db";
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
            mapper.AddMapping<CustomerMap>();
            mapper.AddMapping<PlaylistMap>();
            mapper.AddMapping<TrackMap>();
            mapper.AddMapping<PlaylistTrackMap>();

            return mapper.CompileMappingFor(new[]
            { typeof(Employee), typeof(Customer), typeof(Playlist), typeof(Track), typeof(PlaylistTrack) }
            );
        }

        public static void Setup()
        {
            NHConfig = ConfigureNHibernate();
            HbmMapping mapping = GetMappings();
            NHConfig.AddDeserializedMapping(mapping, "WebApplication1");
            SchemaMetadataUpdater.QuoteTableAndColumns(NHConfig);
            SessionFactory = NHConfig.BuildSessionFactory();
        }

        public static T? GetById<T>(int id) where T : class
        {
            var session = SessionFactory.OpenSession();
            return session.Get<T>(id);
        }

        public static T? GetByProperty<T>(string propertyName, object value) where T : class
        {
            var session = SessionFactory.OpenSession();
            return session.CreateCriteria<T>()
                .Add(Restrictions.Eq(propertyName, value))
                .SetMaxResults(1)
                .List<T>()
                .FirstOrDefault();
        }

        public static List<T> GetListByProperty<T>(string propertyName, object value) where T : class
        {
            var session = SessionFactory.OpenSession();
            return (List<T>)session.CreateCriteria<T>()
                .Add(Restrictions.Eq(propertyName, value))
                .List<T>();
        }

        public static List<T> GetAll<T>() where T : class
        {
            var session = SessionFactory.OpenSession();
            return (List<T>) session.CreateCriteria<T>().List<T>();
        }

    }
}
