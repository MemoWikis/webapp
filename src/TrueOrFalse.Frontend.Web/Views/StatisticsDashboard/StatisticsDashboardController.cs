using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NHibernate;
using NHibernate.Util;
using System.Web.Http.Cors;

[EnableCors(origins: "localhost:8080", headers: "*", methods: "*")]
public class StatisticsDashboardController : Controller
{
    [HttpGet]
    public int[] GetCreatedQuestionsInTimeWindow(int amount, string interval)
    {
        var _session = Sl.R<ISession>();
        var i = 0;
        var questionsPerInterval = new List<int>();

        switch (interval)
        {
            case "day":
                {
                    while (i < amount)
                    {
                        var questionsInInterval = _session
                            .CreateSQLQuery("SELECT DateCreated FROM questionset WHERE YEAR(DateCreated) =" + DateTime.Now.AddDays(-i).Year + " AND MONTH(DateCreated) =" + DateTime.Now.AddDays(-i).Month + " AND DAY(DateCreated) =" + DateTime.Now.AddDays(-i).Day)
                            .List<DateTime>();
                        questionsPerInterval.Add(questionsInInterval.Count);
                        i++;
                    }
                    break;
                }
            case "month":
                {
                    while (i < amount)
                    {
                        var questionsInInterval = _session
                            .CreateSQLQuery("SELECT DateCreated FROM questionset WHERE YEAR(DateCreated) =" + DateTime.Now.AddMonths(-i).Year + " AND MONTH(DateCreated) =" + DateTime.Now.AddMonths(-i).Month)
                            .List<DateTime>();
                        questionsPerInterval.Add(questionsInInterval.Count);
                        i++;
                    }
                    break;
                }
            case "year":
                {
                    while (i < amount)
                    {
                        var questionsInInterval = _session
                            .CreateSQLQuery("SELECT DateCreated FROM questionset WHERE YEAR(DateCreated) =" + DateTime.Now.AddYears(-i).Year)
                            .List<DateTime>();
                        questionsPerInterval.Add(questionsInInterval.Count);
                        i++;
                    }
                    break;
                }
            default:
                return null;
        }


        var result = questionsPerInterval.ToArray();
        return result;
    }


    public int[] GetCreatedUsersInTimeWindow(int amount, string interval)
    {
        var _session = Sl.R<ISession>();
        var i = 0;
        var usersPerInterval = new List<int>();

        switch (interval)
        {
            case "day":
                {
                    while (i < amount)
                    {
                        var usersInInterval = _session
                            .CreateSQLQuery("SELECT DateCreated FROM user WHERE YEAR(DateCreated) =" + DateTime.Now.AddDays(-i).Year + " AND MONTH(DateCreated) =" + DateTime.Now.AddDays(-i).Month + " AND DAY(DateCreated) =" + DateTime.Now.AddDays(-i).Day)
                            .List<DateTime>();
                        usersPerInterval.Add(usersInInterval.Count);
                        i++;
                    }
                    break;
                }
            case "month":
                {
                    while (i < amount)
                    {
                        var usersInInterval = _session
                            .CreateSQLQuery("SELECT DateCreated FROM user WHERE YEAR(DateCreated) =" + DateTime.Now.AddMonths(-i).Year + " AND MONTH(DateCreated) =" + DateTime.Now.AddMonths(-i).Month)
                            .List<DateTime>();
                        usersPerInterval.Add(usersInInterval.Count);
                        i++;
                    }
                    break;
                }
            case "year":
                {
                    while (i < amount)
                    {
                        var usersInInterval = _session
                            .CreateSQLQuery("SELECT DateCreated FROM user WHERE YEAR(DateCreated) =" + DateTime.Now.AddYears(-i).Year)
                            .List<DateTime>();
                        usersPerInterval.Add(usersInInterval.Count);
                        i++;
                    }
                    break;
                }
            default:
                return null;
        }


        var result = usersPerInterval.ToArray();
        return result;
    }
    public int[] GetCreatedCategoriesInTimeWindow(int amount, string interval)
    {
        var _session = Sl.R<ISession>();
        var i = 0;
        var categoriesPerInterval = new List<int>();

        switch (interval)
        {
            case "day":
                {
                    while (i < amount)
                    {
                        var categoriesInInterval = _session
                            .CreateSQLQuery("SELECT DateCreated FROM category WHERE YEAR(DateCreated) =" + DateTime.Now.AddDays(-i).Year + " AND MONTH(DateCreated) =" + DateTime.Now.AddDays(-i).Month + " AND DAY(DateCreated) =" + DateTime.Now.AddDays(-i).Day)
                            .List<DateTime>();
                        categoriesPerInterval.Add(categoriesInInterval.Count);
                        i++;
                    }
                    break;
                }
            case "month":
                {
                    while (i < amount)
                    {
                        var categoriesInInterval = _session
                            .CreateSQLQuery("SELECT DateCreated FROM category WHERE YEAR(DateCreated) =" + DateTime.Now.AddMonths(-i).Year + " AND MONTH(DateCreated) =" + DateTime.Now.AddMonths(-i).Month)
                            .List<DateTime>();
                        categoriesPerInterval.Add(categoriesInInterval.Count);
                        i++;
                    }
                    break;
                }
            case "year":
                {
                    while (i < amount)
                    {
                        var categoriesInInterval = _session
                            .CreateSQLQuery("SELECT DateCreated FROM category WHERE YEAR(DateCreated) =" + DateTime.Now.AddYears(-i).Year)
                            .List<DateTime>();
                        categoriesPerInterval.Add(categoriesInInterval.Count);
                        i++;
                    }
                    break;
                }
            default:
                return null;
        }


        var result = categoriesPerInterval.ToArray();
        return result;
    }
}