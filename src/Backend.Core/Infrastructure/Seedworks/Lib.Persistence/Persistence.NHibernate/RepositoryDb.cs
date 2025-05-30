using System.Collections;
using NHibernate;
using NHibernate.Criterion;

public class RepositoryDb<TDomainObject>
    where TDomainObject : class, IPersistable
{
    protected readonly ISession _session;
    private List<TDomainObject> _allItemsCached;
    protected event EventHandler<RepositoryDbEventArgs> OnItemMutated;
    protected event EventHandler<RepositoryDbEventArgs> OnItemCreated;
    protected event EventHandler<RepositoryDbEventArgs> OnItemDeleted;
    protected event EventHandler<RepositoryDbEventArgs> OnItemUpdated;

    public IQueryOver<TDomainObject, TDomainObject> Query => _session.QueryOver<TDomainObject>();

    public ISession Session => _session;

    /// <summary>
    /// Occurs after a TDomainObject is retrieved from DB
    /// </summary>
    protected event EventHandler<TDomainObjectArgs> AfterItemRetrieved;

    /// <summary>
    /// Occurs after a TDomainObjectList is retrieved from DB
    /// </summary>
    protected event EventHandler<TDomainObjectListArgs> AfterItemListRetrieved;

    public RepositoryDb(ISession session)
    {
        _session = session;
        OnItemCreated += ItemMutated;
        OnItemDeleted += ItemMutated;
        OnItemUpdated += ItemMutated;
    }

    private void ItemMutated(object sender, RepositoryDbEventArgs e)
    {
        if (OnItemMutated != null)
            OnItemMutated(this, e);
    }

    public DetachedCriteria GetDetachedCriteria()
    {
        return DetachedCriteria.For(typeof(TDomainObject),
            typeof(TDomainObject).Name.ToLower());
    }

    public ICriteria GetExecutableCriteria()
    {
        return GetDetachedCriteria().GetExecutableCriteria(_session);
    }

    public void AddGenericConditions(ICriteria criteria, ConditionContainer filter)
    {
        filter.CreateAliases(criteria);
        foreach (Condition condition in filter)
            condition.AddToCriteria(criteria);
    }

    public void AddOrderBy(ICriteria criteria, OrderByCriteria orderBy)
    {
        AddOrderBy(criteria, orderBy, null);
    }

    public virtual void AddOrderBy(
        ICriteria criteria,
        OrderByCriteria orderByCriteria,
        string tableAlias)
    {
        if (!orderByCriteria.IsSet()) return;

        foreach (var orderBy in orderByCriteria.CurrentList)
        {
            if (orderBy.HasCriteriaAction)
                orderBy.CriteriaAction(criteria);

            AddOrderBy(criteria, orderBy, orderBy.HasAlias ? orderBy.Alias : tableAlias);
        }
    }

    private static void AddOrderBy(ICriteria criteria, OrderBy orderBy, string tableAlias)
    {
        if (orderBy == null) return;

        var propertyName = (string.IsNullOrEmpty(tableAlias)
                               ? string.Empty
                               : tableAlias.EnsureEndsWith("."))
                           + orderBy.PropertyName;

        if (orderBy.Direction == OrderDirection.Ascending)
            criteria.AddOrder(Order.Asc(propertyName));
        else
            criteria.AddOrder(Order.Desc(propertyName));
    }

    public void SetPager(ICriteria criteria, IPager pager)
    {
        if (!pager.QueryAll)
        {
            criteria.SetMaxResults(pager.PageSize);
            criteria.SetFirstResult(pager.FirstResult);
        }
    }

    /// <summary>
    /// Add Object in  Database, set Dates Date(Created, Modified) and deleted MemoCache
    /// </summary>
    /// <param name="domainObject"></param>
    public virtual void Create(TDomainObject domainObject)
    {
        if (domainObject is WithDateCreated)
            (domainObject as WithDateCreated).DateCreated = DateTime.Now;

        if (domainObject is WithDateModified)
            (domainObject as WithDateModified).DateModified = DateTime.Now;

        _session.Save(domainObject);
        ClearAllItemCache();

        if (OnItemCreated != null)
            OnItemCreated(this, new RepositoryDbEventArgs(domainObject));
    }

    public virtual void Create(IList<TDomainObject> domainObjects)
    {
        foreach (var domainObject in domainObjects)
            Create(domainObject);
    }

    public virtual void Update(TDomainObject domainObject)
    {
        if (domainObject is WithDateModified)
            (domainObject as WithDateModified).DateModified = DateTime.Now;

        try
        {
            _session.Update(domainObject);
        }
        catch (NonUniqueObjectException)
        {
            _session.Merge(domainObject);
        }

        ClearAllItemCache();

        if (OnItemUpdated != null)
            OnItemUpdated(this, new RepositoryDbEventArgs(domainObject));
    }

    public virtual void Merge(TDomainObject domainObject)
    {
        if (domainObject is WithDateModified)
            (domainObject as WithDateModified).DateModified = DateTime.Now;

        domainObject = _session.Merge(domainObject);
        ClearAllItemCache();

        if (OnItemUpdated != null)
            OnItemUpdated(this, new RepositoryDbEventArgs(domainObject));
    }

    public virtual void CreateOrUpdate(TDomainObject domainObject)
    {
        var creating = domainObject.Id == 0;

        if (domainObject is WithDateCreated)
        {
            if ((domainObject as WithDateCreated).DateCreated == DateTime.MinValue)
                (domainObject as WithDateCreated).DateCreated = DateTime.Now;
        }

        if (domainObject is WithDateModified)
            (domainObject as WithDateModified).DateModified = DateTime.Now;

        _session.SaveOrUpdate(domainObject);
        ClearAllItemCache();

        if (creating && OnItemCreated != null)
            OnItemCreated(this, new RepositoryDbEventArgs(domainObject));
        else if (!creating && OnItemUpdated != null)
            OnItemUpdated(this, new RepositoryDbEventArgs(domainObject));
    }

    public virtual void Delete(TDomainObject domainObject)
    {
        _session.Delete(domainObject);
        ClearAllItemCache();
        Flush();

        if (OnItemDeleted != null)
            OnItemDeleted(this, new RepositoryDbEventArgs(domainObject));
    }

    public virtual void Delete(int id)
    {
        Delete(GetById(id));
    }

    public void ClearAllItemCache()
    {
        _allItemsCached = null;
    }

    public virtual IList<TDomainObject> GetAll()
    {
        var list = new List<TDomainObject>();

        if (_allItemsCached != null && _allItemsCached.Count != 0)
            list.AddRange(_allItemsCached);
        else
        {
            list.AddRange(_session.CreateCriteria(typeof(TDomainObject))
                .List<TDomainObject>());

            _allItemsCached = new List<TDomainObject>();
            _allItemsCached.AddRange(list);
        }

        if (AfterItemListRetrieved != null)
            AfterItemListRetrieved(this, new TDomainObjectListArgs(list));

        return list;
    }

    public virtual TDomainObject? GetById(int id)
    {
        var result = _session.CreateCriteria(typeof(TDomainObject))
            .Add(Restrictions.Eq("Id", id))
            .UniqueResult<TDomainObject>();

        if (AfterItemRetrieved != null)
            AfterItemRetrieved(this, new TDomainObjectArgs(result));

        return result;
    }

    public virtual IList<TDomainObject> GetByIds(params int[] pageIds)
    {
        var list = new List<TDomainObject>();
        list.AddRange(_session.CreateCriteria(typeof(TDomainObject))
            .Add(Restrictions.In("Id", pageIds))
            .List<TDomainObject>());

        if (AfterItemListRetrieved != null)
            AfterItemListRetrieved(this, new TDomainObjectListArgs(list));

        return list;
    }

    public virtual IList<TDomainObject> GetBy(ISearchDesc searchSpec)
    {
        return GetBy(searchSpec, null);
    }

    /// <param name="searchSpec"></param>
    /// <param name="criteriaExtender">Here you can plug in additional changes of the criteria.</param>
    /// <returns></returns>
    public IList<TDomainObject> GetBy(
        ISearchDesc searchSpec,
        Action<ICriteria> criteriaExtender)
    {
        var criteria = GetExecutableCriteria();

        AddGenericConditions(criteria, searchSpec.Filter);
        AddOrderBy(criteria, searchSpec.OrderBy);

        if (criteriaExtender != null)
            criteriaExtender.Invoke(criteria);

        var totalCountCriteria = CriteriaTransformer.TransformToRowCount(criteria);

        SetPager(criteria, searchSpec);

        // Use MultiCriteria to reduce DB roundtrips.
        var multiCriteria = _session
            .CreateMultiCriteria()
            .Add("data", criteria)
            .Add("rowCount", totalCountCriteria);
        IList multiResult;

        try
        {
            multiResult = multiCriteria.List();
        }
        catch (ADOException e)
        {
            _session.Connection.Close();
            Console.WriteLine(
                $"____________________________________ {e}------------------------------------");
            throw;
        }

        // Extract results from the multiple result sets
        var list = new List<TDomainObject>();
        list.AddRange(((IList)multiResult[0]).Cast<TDomainObject>());

        searchSpec.TotalItems = (int)((IList)multiResult[1])[0];

        if (AfterItemListRetrieved != null)
            AfterItemListRetrieved(this, new TDomainObjectListArgs(list));

        return list;
    }

    public void Flush()
    {
        _session.Flush();
    }

    public void Refresh(TDomainObject item)
    {
        _session.Refresh(item);
    }

    protected class RepositoryDbEventArgs : EventArgs
    {
        private readonly TDomainObject _item;

        public TDomainObject Item
        {
            get { return _item; }
        }

        public RepositoryDbEventArgs(TDomainObject item)
        {
            _item = item;
        }
    }

    protected class TDomainObjectArgs : RepositoryDbEventArgs
    {
        public TDomainObjectArgs(TDomainObject item) : base(item)
        {
        }
    }

    protected class TDomainObjectListArgs : EventArgs
    {
        private readonly IList<TDomainObject> _items;

        public IList<TDomainObject> Items
        {
            get { return _items; }
        }

        public TDomainObjectListArgs(IList<TDomainObject> items)
        {
            _items = items;
        }
    }

    public IList<int> GetAllIds()
    {
        DetachedCriteria queryObjects =
            DetachedCriteria.For(typeof(TDomainObject), "o");

        ICriteria criteria = queryObjects.GetExecutableCriteria(_session)
            .SetProjection(Projections.Property("Id")); // guaranteed to exist by IPersistable

        return criteria.List<int>();
    }

    public void ClearNHibernateCaches()
    {
        _session.Clear(); // Clear the first-level cache (session cache)

        var sessionFactory = _session.SessionFactory;
        foreach (var collectionMetadata in sessionFactory.GetAllCollectionMetadata())
        {
            sessionFactory.EvictCollection(collectionMetadata.Key);
        }

        foreach (var classMetadata in sessionFactory.GetAllClassMetadata())
        {
            sessionFactory.EvictEntity(classMetadata.Key);
        }

        sessionFactory.EvictQueries();
    }
}