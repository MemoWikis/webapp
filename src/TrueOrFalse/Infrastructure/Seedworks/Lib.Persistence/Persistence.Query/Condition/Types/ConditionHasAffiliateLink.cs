using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate;
using NHibernate.Criterion;

namespace Seedworks.Lib.Persistence
{
	/// <summary>
	/// Tightly coupled to Generic.Core AffiliateLink functionality, but here for inspiration :-)
	/// </summary>
	/// <typeparam name="TParent">The type of the entity the query is built for.</typeparam>
	/// <typeparam name="TChild">The type of the referenced entity.</typeparam>
    [Serializable]
    public class ConditionHasAffiliateLink<TParent, TChild> : ConditionObject<TChild>
	{
		private readonly List<int> _affiliateLinkIds = new List<int>();
		protected string Alias { get; set; }

		public ConditionHasAffiliateLink(ConditionContainer conditions) : base(conditions)
		{
		}

		public ConditionHasAffiliateLink(ConditionContainer conditions, string propertyName)
			: base(conditions, propertyName)
		{
		}

		public ConditionHasAffiliateLink<TParent, TChild> Add(params int[] affiliateLinkIds)
		{
			_affiliateLinkIds.RemoveAll(i => affiliateLinkIds.Contains(i));
			_affiliateLinkIds.AddRange(affiliateLinkIds);
			AddUniqueToContainer();
			return this;
		}

		public void Remove(int value)
		{
			_affiliateLinkIds.RemoveAll(i => i == value);
			if (_affiliateLinkIds.Count <= 0)
				Reset();
		}

		public override void AddToCriteria(ICriteria criteria)
		{
			Alias = criteria.Alias;
			criteria.Add(GetCriterion());
		}

		public override ICriterion GetCriterion()
		{
			var detachedCriteria = DetachedCriteria.For<TChild>("CAL");
			detachedCriteria.Add(Restrictions.EqProperty("GeoObject.Id", Alias + ".Id"))
				.Add(Restrictions.Eq("IsActive", true))
				.Add(Restrictions.InG("AffiliateLink.Id", _affiliateLinkIds))
				.CreateAlias("AffiliateLink", "al")
				.Add(Restrictions.Eq("al.IsActive", true))
				.SetProjection(Projections.CountDistinct("AffiliateLink.Id"));

			return Restrictions.Eq(Projections.SubQuery(detachedCriteria), _affiliateLinkIds.Count);
		}

		public bool Contains(int affiliateLinkId)
		{
			return _affiliateLinkIds.Contains(affiliateLinkId);
		}

		public override void Reset()
		{
			_affiliateLinkIds.Clear();
			Alias = null;
			base.Reset();
		}

		public List<int> Ids
		{
			get { return _affiliateLinkIds; }
		}
	}
}