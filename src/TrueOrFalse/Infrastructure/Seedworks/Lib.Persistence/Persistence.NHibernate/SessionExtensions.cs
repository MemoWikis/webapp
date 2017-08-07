using System;
using System.Collections.Generic;
using System.Reflection;
using NHibernate;
using NHibernate.Engine;
using NHibernate.Persister.Entity;
using NHibernate.Proxy;

namespace Seedworks.Lib.Persistence.Persistance.NHibernate
{
	public static class SessionExtensions
	{
		public static Boolean IsDirtyEntity(this ISession session, Object entity)
		{
			String className = NHibernateProxyHelper.GuessClass(entity).FullName;

			var sessionImpl = session.GetSessionImplementation();
			var persister = sessionImpl.Factory.GetEntityPersister(className);
			var oldEntry = sessionImpl.PersistenceContext.GetEntry(entity);


			if ((oldEntry == null) && (entity is INHibernateProxy))
			{
				var proxy = entity as INHibernateProxy;
				var obj = sessionImpl.PersistenceContext.Unproxy(proxy);

				oldEntry = sessionImpl.PersistenceContext.GetEntry(obj);
			}


			Object[] oldState = oldEntry.LoadedState;
			Object[] currentState = persister.GetPropertyValues(entity, sessionImpl.EntityMode);
			Int32[] dirtyProps = persister.FindDirty(currentState, oldState, entity, sessionImpl);

			return (dirtyProps != null);
		}


		public static Boolean IsDirtyProperty(this ISession session, Object entity, String propertyName)
		{
			String className = NHibernateProxyHelper.GuessClass(entity).FullName;
			ISessionImplementor sessionImpl = session.GetSessionImplementation();
			IEntityPersister persister = sessionImpl.Factory.GetEntityPersister(className);
			EntityEntry oldEntry = sessionImpl.PersistenceContext.GetEntry(entity);

			if (oldEntry == null)
			{
				if (entity is INHibernateProxy)
				{
					var proxy = entity as INHibernateProxy;
					Object obj = sessionImpl.PersistenceContext.Unproxy(proxy);
					oldEntry = sessionImpl.PersistenceContext.GetEntry(obj);
				}
				else
				{
					// We don't know...
					return true;
				}
			}


			Object[] oldState = oldEntry.LoadedState;
			Object[] currentState = persister.GetPropertyValues(entity, sessionImpl.EntityMode);
			Int32[] dirtyProps = persister.FindDirty(currentState, oldState, entity, sessionImpl);
			Int32 index = Array.IndexOf(persister.PropertyNames, propertyName);

			Boolean isDirty = (dirtyProps != null) ? (Array.IndexOf(dirtyProps, index) != -1) : false;

			return (isDirty);
		}

		public static List<string> GetDirtyPropertyNames(this ISession session, Object entity)
		{
			var result = new List<string>();
			var type = entity.GetType();

			var propertyInfos = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);

			foreach (var propertyInfo in propertyInfos)
				if (session.IsDirtyProperty(entity, propertyInfo.Name))
					result.Add(propertyInfo.Name);

			return result;
		}

        public static Object GetOriginalEntityProperty(this ISession session, Object entity, String propertyName)
		{
			String className = NHibernateProxyHelper.GuessClass(entity).FullName;
			ISessionImplementor sessionImpl = session.GetSessionImplementation();
			IPersistenceContext persistenceContext = sessionImpl.PersistenceContext;
			IEntityPersister persister = sessionImpl.Factory.GetEntityPersister(className);
			EntityEntry oldEntry = sessionImpl.PersistenceContext.GetEntry(sessionImpl.PersistenceContext.Unproxy(entity));
			//EntityEntry oldEntry = sessionImpl.PersistenceContext.GetEntry(entity);


			//if ((oldEntry == null) && (entity is INHibernateProxy))
			//{

			//    INHibernateProxy proxy = entity as INHibernateProxy;

			//    Object obj = sessionImpl.PersistenceContext.Unproxy(proxy);

			//    oldEntry = sessionImpl.PersistenceContext.GetEntry(obj);

			//}


			Object[] oldState = oldEntry.LoadedState;
			Object[] currentState = persister.GetPropertyValues(entity, sessionImpl.EntityMode);
			Int32[] dirtyProps = persister.FindDirty(currentState, oldState, entity, sessionImpl);
            Int32 index = Array.IndexOf(persister.PropertyNames, propertyName);

			Boolean isDirty = (dirtyProps != null) ? (Array.IndexOf(dirtyProps, index) != -1) : false;

			return (isDirty ? oldState[index] : currentState[index]);
		}
	}
}