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
using NHibernate.Mapping;

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

        public static List<T> GetListByPropertyOrEmpty<T>(string propertyName, object value) where T : class
        {
            var session = SessionFactory.OpenSession();
            return (List<T>)session.CreateCriteria<T>()
                .Add(Restrictions.Or(
                    Restrictions.Eq(propertyName, value),
                    Restrictions.IsNull(propertyName)
                    ))
                .List<T>();
        }

        public static List<T> GetListByPropertiesOr<T>(string property1Name, object value1, string property2Name, object value2) where T : class
        {
            var session = SessionFactory.OpenSession();
            return (List<T>)session.CreateCriteria<T>()
                .Add(Restrictions.Or(
                    Restrictions.Eq(property1Name, value1), 
                    Restrictions.Eq(property2Name, value2)
                    ))
                .List<T>();
        }

        public static List<T> GetListByPropertiesAnd<T>(string property1Name, object value1, string property2Name, object value2) where T : class
        {
            var session = SessionFactory.OpenSession();
            return (List<T>)session.CreateCriteria<T>()
                .Add(Restrictions.And(
                    Restrictions.Eq(property1Name, value1),
                    Restrictions.Eq(property2Name, value2)
                    ))
                .List<T>();
        }

        public static List<T> GetListIfEmpty<T>(string propertyName) where T : class
        {
            var session = SessionFactory.OpenSession();
            return (List<T>)session.CreateCriteria<T>()
                .Add(Restrictions.IsNull(propertyName))
                .List<T>();
        }

        public static List<T> GetAll<T>() where T : class
        {
            var session = SessionFactory.OpenSession();
            return (List<T>) session.CreateCriteria<T>().List<T>();
        }

        public static string? GetMaxValue<T>(string propertyName) where T : class
        {
            var session = SessionFactory.OpenSession();
            return session.CreateCriteria<T>().
                SetProjection(Projections.Max(propertyName))
                .UniqueResult().ToString();
        }

        public static int? Insert<T>(T obj) where T : class
        {
            var session = SessionFactory.OpenSession();
            var tx = session.BeginTransaction();
            int? objId =  (int?) session.Save(obj);
            tx.Commit();
            return objId;
        }

        public static void Delete<T>(T obj) where T : class
        {
            var session = SessionFactory.OpenSession();
            var tx = session.BeginTransaction();
            session.Delete(obj);
            tx.Commit();
            return ;
        }

        public static void DeleteByParams(string table, string property1Name, object value1, string property2Name, object value2)
        {
            string query = "DELETE FROM " + table + " WHERE " + property1Name + "=:value1 AND " + property2Name + "=:value2";
            var session = SessionFactory.OpenSession();
            var tx = session.BeginTransaction();

            session.CreateSQLQuery(query)
            .SetParameter("value1", value1)
            .SetParameter("value2", value2)
            .ExecuteUpdate();
            tx.Commit();
            return;
        }

        public static List<T> GetFilteredListByPropertyOrEmpty<T>(
            string propertyName, object value, string filterName, string filterValue
            ) where T : class
        {
            var session = SessionFactory.OpenSession();
            return (List<T>)session.CreateCriteria<T>()
                .Add(Restrictions.And(
                            Restrictions.Or(
                                Restrictions.Eq(propertyName, value),
                                Restrictions.IsNull(propertyName)),
                            Restrictions.Like(filterName, "%" + filterValue + "%")
                            ))
                .List<T>();
        }

        public static List<T> GetFilteredListIfEmpty<T>(string propertyName, string filterName, string filterValue) where T : class
        {
            var session = SessionFactory.OpenSession();
            return (List<T>)session.CreateCriteria<T>()
                .Add(Restrictions.And(
                    Restrictions.IsNull(propertyName),
                    Restrictions.Like(filterName, "%" + filterValue + "%")))
                .List<T>();
        }

        public static List<T> GetAllFiltered<T>(string filterName, string filterValue) where T : class
        {
            var session = SessionFactory.OpenSession();
            return (List<T>)session.CreateCriteria<T>()
                .Add(Restrictions.Like(filterName, "%" + filterValue + "%"))
                .List<T>();
        }

        public static List<T> GetAllFiltered<T>(string filter1Name, string filter1Value, string filter2Name, string filter2Value) where T : class
        {
            var session = SessionFactory.OpenSession(); 
            return (List<T>)session.CreateCriteria<T>()
                .Add(Restrictions.And(
                    Restrictions.Like(filter1Name, "%" + filter1Value + "%"),
                    Restrictions.Like(filter2Name, "%" + filter2Value + "%")))
                .List<T>();
        }
    }
}
