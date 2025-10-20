using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

[DebuggerDisplay("Id={Id} Name={Text}")]
[Serializable]
public class QuestionCacheItem
{
    public QuestionCacheItem()
    {
        Pages = new List<PageCacheItem>();
        References = new List<ReferenceCacheItem>();
    }

    public virtual UserCacheItem Creator
        => EntityCache.GetUserById(CreatorId);

    public virtual IList<PageCacheItem> Pages { get; set; }

    public virtual int CorrectnessProbability { get; set; }
    public virtual int CorrectnessProbabilityAnswerCount { get; set; }

    public int CreatorId { get; set; }
    public virtual DateTime DateCreated { get; set; }
    public virtual DateTime DateModified { get; set; }
    public virtual string Description { get; set; }
    public virtual string DescriptionHtml { get; set; }
    public virtual int Id { get; set; }

    public virtual LicenseQuestion License
    {
        get => LicenseQuestionRepo.GetById(LicenseId);
        set
        {
            if (value == null)
            {
                return;
            }

            LicenseId = value.Id;
        }
    }

    public virtual int LicenseId { get; set; }
    public virtual IList<ReferenceCacheItem> References { get; set; }
    public virtual bool SkipMigration { get; set; }

    public virtual string Solution { get; set; }
    public virtual string SolutionMetadataJson { get; set; }
    public virtual SolutionType SolutionType { get; set; }
    public virtual string Text { get; set; }
    public virtual string TextExtended { get; set; }
    public virtual string TextExtendedHtml { get; set; }

    public virtual string TextHtml { get; set; }
    public virtual int TotalFalseAnswers { get; set; }

    public virtual int TotalQualityAvg { get; set; }
    public virtual int TotalQualityEntries { get; set; }

    public virtual int TotalRelevanceForAllAvg { get; set; }
    public virtual int TotalRelevanceForAllEntries { get; set; }

    public virtual int TotalRelevancePersonalAvg { get; set; }
    public virtual int TotalRelevancePersonalEntries { get; set; }

    public virtual int TotalTrueAnswers { get; set; }

    public virtual int TotalViews { get; set; }
    public virtual List<DailyViews> ViewsOfPast90Days { get; set; }
    public virtual QuestionVisibility Visibility { get; set; }
    public bool IsPublic => Visibility == QuestionVisibility.Public;

    public virtual List<QuestionChangeCacheItem> QuestionChangeCacheItems { get; set; }

    public virtual List<int> CommentIds { get; set; }

    public virtual AnswerRecord AnswerCounter { get; set; }

    public virtual DateTime LastPublishDate { get; set; }

    public virtual string Language { get; set; } = "de";

    public static string AnswersAsHtml(string answerText, SolutionType solutionType)
    {
        switch (solutionType)
        {
            case SolutionType.MatchList:

                //Quick Fix: Prevent null reference exeption
                if (answerText == "" || answerText == null)
                {
                    return "";
                }

                var answerObject = QuestionSolutionMatchList.DeserializeMatchListAnswer(answerText);
                if (answerObject.Pairs.Count == 0)
                {
                    return "(keine Auswahl)";
                }

                var formattedMatchListAnswer = answerObject.Pairs.Aggregate("</br><ul>",
                    (current, pair) => current + "<li>" + pair.ElementLeft.Text + " - " +
                                       pair.ElementRight.Text +
                                       "</li>");
                formattedMatchListAnswer += "</ul>";
                return formattedMatchListAnswer;

            case SolutionType.MultipleChoice:
                if (answerText == "")
                {
                    return "(keine Auswahl)";
                }

                var builder = new StringBuilder(answerText);
                var formattedMultipleChoiceAnswer = "</br> <ul> <li>" +
                                                    builder.Replace("%seperate&xyz%", "</li><li>") +
                                                    "</li> </ul>";
                return formattedMultipleChoiceAnswer;
        }

        return answerText;
    }

    public virtual string GetShortTitle(int length = 96)
    {
        var safeText = Regex.Replace(Text, "<.*?>", "");
        return safeText.TruncateAtWord(length);
    }

    public virtual IEnumerable<PageCacheItem> PagesVisibleToCurrentUser(
        PermissionCheck permissionCheck)
    {
        return Pages.Where(permissionCheck.CanView);
    }

    public virtual IEnumerable<PageCacheItem> PublicPages() => Pages.Where(p => p.IsPublic);

    public virtual bool IsInWishKnowledge(int userId, ExtendedUserCache extendedUserCache)
    {
        return extendedUserCache.IsQuestionInWishKnowledge(userId, Id);
    }

    public virtual bool IsPrivate()
    {
        return Visibility != QuestionVisibility.Public;
    }

    public static QuestionCacheItem ToCacheQuestion(Question question, IList<QuestionViewSummaryWithId>? questionViews = null, List<QuestionChange>? questionChanges = null, AnswerRecord? answers = null)
    {
        var questionCacheItem = new QuestionCacheItem
        {
            Id = question.Id,
            CorrectnessProbability = question.CorrectnessProbability,
            CorrectnessProbabilityAnswerCount = question.CorrectnessProbabilityAnswerCount,
            Description = question.Description,
            SkipMigration = question.SkipMigration,
            Visibility = question.Visibility,
            TotalRelevancePersonalEntries = question.TotalRelevancePersonalEntries,
            Pages = EntityCache.GetPages(question.Pages?.Select(c => c.Id)).ToList(),
            CreatorId = question.Creator?.Id ?? -1,
            DateCreated = question.DateCreated,
            DateModified = question.DateModified,
            DescriptionHtml = question.DescriptionHtml,
            TotalQualityAvg = question.TotalQualityAvg,
            TotalQualityEntries = question.TotalQualityEntries,
            TextExtended = question.TextExtended,
            TextExtendedHtml = question.TextExtendedHtml,
            TotalRelevanceForAllEntries = question.TotalRelevanceForAllEntries,
            TotalRelevanceForAllAvg = question.TotalRelevanceForAllAvg,
            TotalRelevancePersonalAvg = question.TotalRelevancePersonalAvg,
            Text = question.Text,
            TextHtml = question.TextHtml,
            TotalFalseAnswers = question.TotalFalseAnswers,
            TotalTrueAnswers = question.TotalTrueAnswers,
            SolutionType = question.SolutionType,
            LicenseId = question.LicenseId,
            Solution = question.Solution,
            SolutionMetadataJson = question.SolutionMetadataJson,
            License = question.License
        };

        if (EntityCache.IsFirstStart)
        {
            if (questionViews != null)
            {
                questionCacheItem.TotalViews = (int)questionViews.Sum(qv => qv.Count);

                var startDate = DateTime.Now.Date.AddDays(-90);
                var endDate = DateTime.Now.Date;

                var dateRange = Enumerable.Range(0, (endDate - startDate).Days + 1)
                    .Select(d => startDate.AddDays(d));

                questionCacheItem.ViewsOfPast90Days = questionViews.Where(qv => dateRange.Contains(qv.DateOnly))
                    .Select(qv => new DailyViews
                    {
                        Date = qv.DateOnly,
                        Count = qv.Count
                    })
                    .OrderBy(v => v.Date)
                    .ToList();
            }

            if (questionChanges != null)
            {
                QuestionEditData? previousData = null;
                QuestionEditData currentData;
                var questionChangeCacheItems = new List<QuestionChangeCacheItem>();
                var currentVisibility = QuestionVisibility.Private;
                var lastVisibilityChange = question.DateCreated;

                foreach (var curr in questionChanges)
                {
                    if (curr.DataVersion == 1)
                    {
                        currentData = QuestionEditData_V1.CreateFromJson(curr.Data);
                    }
                    else
                    {
                        throw new ArgumentOutOfRangeException($"Invalid data version number {curr.DataVersion} for page change id {curr.Id}");
                    }

                    if (currentData == null)
                        continue;

                    var cacheItem = QuestionChangeCacheItem.ToQuestionChangeCacheItem(curr, currentData, previousData);
                    questionChangeCacheItems.Add(cacheItem);
                    previousData = currentData;

                    if (currentVisibility != cacheItem.Visibility)
                    {
                        lastVisibilityChange = cacheItem.DateCreated;
                        currentVisibility = cacheItem.Visibility;
                    }
                }

                questionCacheItem.QuestionChangeCacheItems = questionChangeCacheItems
                    .OrderByDescending(change => change.DateCreated)
                    .ToList();

                if (question.Visibility == QuestionVisibility.Public)
                    questionCacheItem.LastPublishDate = lastVisibilityChange;
            }

            if (answers != null)
            {
                questionCacheItem.AnswerCounter = (AnswerRecord)answers;
            }
        }

        if (!EntityCache.IsFirstStart)
        {
            questionCacheItem.References =
                ReferenceCacheItem.ToReferenceCacheItems(question.References).ToList();
        }

        return questionCacheItem;
    }

    public static IEnumerable<QuestionCacheItem> ToCacheQuestions(IList<Question> questions, IList<QuestionViewSummaryWithId> questionViews, IList<QuestionChange> questionChanges, IList<Answer>? answers = null)
    {
        var questionViewsByQuestionId = questionViews
            .GroupBy(qv => qv.QuestionId)
            .ToDictionary(g => g.Key, g => g.ToList());

        var questionChangesDictionary = questionChanges
            .GroupBy(qc => qc.Question?.Id ?? -1)
            .ToDictionary(g => g.Key, g => g.ToList());

        var answersDictionary = answers?
            .GroupBy(a => a.Question?.Id ?? -1)
            .ToDictionary(g => g.Key, AnswerCache.AnswersToAnswerRecord);

        var result = questions.Select(q =>
        {
            questionViewsByQuestionId.TryGetValue(q.Id, out var questionViewsWithId);
            questionChangesDictionary.TryGetValue(q.Id, out var questionChanges);

            if (answersDictionary != null)
            {
                answersDictionary.TryGetValue(q.Id, out var answersByQuestionId);
                return ToCacheQuestion(q, questionViewsWithId, questionChanges, answers: answersByQuestionId);
            }

            return ToCacheQuestion(q, questionViewsWithId, questionChanges, answers: null);
        });

        return result;
    }

    public virtual int TotalAnswers()
    {
        return TotalFalseAnswers + TotalTrueAnswers;
    }

    public virtual int TotalFalseAnswerPercentage()
    {
        if (TotalAnswers() == 0)
        {
            return 0;
        }

        if (TotalFalseAnswers == 0)
        {
            return 0;
        }

        return Convert.ToInt32((decimal)TotalFalseAnswers / TotalAnswers() * 100);
    }

    public virtual int TotalTrueAnswersPercentage()
    {
        if (TotalAnswers() == 0)
        {
            return 0;
        }

        if (TotalTrueAnswers == 0)
        {
            return 0;
        }

        return Convert.ToInt32((decimal)TotalTrueAnswers / TotalAnswers() * 100);
    }

    public virtual void UpdateReferences(IList<Reference> references)
    {
        var newReferences = ReferenceCacheItem
            .ToReferenceCacheItems(references.Where(r => r.Id == -1 || r.Id == 0).ToList())
            .ToArray();
        var removedReferences =
            References.Where(r => references.All(r2 => r2.Id != r.Id)).ToArray();
        var existingReferenes =
            references.Where(r => References.Any(r2 => r2.Id == r.Id)).ToArray();

        newReferences.ToList().ForEach(r =>
        {
            r.DateCreated = DateTime.Now;
            r.DateModified = DateTime.Now;
        });

        for (var i = 0; i < newReferences.Count(); i++)
        {
            newReferences[i].Id = default;
            References.Add(newReferences[i]);
        }

        for (var i = 0; i < removedReferences.Count(); i++)
        {
            References.Remove(removedReferences[i]);
        }

        for (var i = 0; i < existingReferenes.Count(); i++)
        {
            var reference = References.First(r => r.Id == existingReferenes[i].Id);
            reference.DateModified = DateTime.Now;
            reference.AdditionalInfo = existingReferenes[i].AdditionalInfo;
            reference.ReferenceText = existingReferenes[i].ReferenceText;
        }
    }

    public virtual bool IsCreator(int userId)
    {
        return userId == Creator?.Id;
    }

    public virtual string GetRenderedQuestionTextExtended()
    {
        var str = "";

        if (TextExtendedHtml?.Length > 0)
            str = TextExtendedHtml;
        else if (TextExtended?.Length > 0)
            str = MarkdownMarkdig.ToHtml(TextExtended);

        return str;
    }

    public void AddQuestionView(DateTime date)
    {
        if (ViewsOfPast90Days == null)
            GenerateEmptyViewsOfPast90DaysList();

        var existingView = ViewsOfPast90Days.FirstOrDefault(v => v.Date.Date == date);

        if (existingView != null)
        {
            existingView.Count++;
        }
        else
        {
            ViewsOfPast90Days.Add(new DailyViews
            {
                Date = date,
                Count = 1
            });
        }
        TotalViews++;
    }

    public virtual List<DailyViews> GetViewsOfPast90Days()
    {
        var startDate = DateTime.Now.Date.AddDays(-90);
        var endDate = DateTime.Now.Date;

        var dateRange = Enumerable.Range(0, (endDate - startDate).Days + 1)
            .Select(d => startDate.AddDays(d));

        if (ViewsOfPast90Days == null)
            GenerateEmptyViewsOfPast90DaysList();

        ViewsOfPast90Days = dateRange
            .GroupJoin(
                ViewsOfPast90Days,
                date => date,
                view => view.Date,
                (date, views) => views.DefaultIfEmpty(new DailyViews { Date = date, Count = 0 })
            )
            .SelectMany(group => group)
            .OrderBy(view => view.Date)
            .ToList();

        return ViewsOfPast90Days;
    }

    private void GenerateEmptyViewsOfPast90DaysList()
    {
        ViewsOfPast90Days = Enumerable.Range(0, 90)
            .Select(i => new DailyViews
            {
                Date = DateTime.Now.Date.AddDays(-i),
                Count = 0
            })
            .Reverse()
            .ToList();
    }

    public void AddQuestionChangeToPageChangeCacheItems(QuestionChange questionChange)
    {
        QuestionChangeCacheItems ??= new List<QuestionChangeCacheItem>();

        var currentData = questionChange.GetQuestionChangeData();
        QuestionEditData? previousData = QuestionChangeCacheItems.Count > 0 ? QuestionChangeCacheItems.First().GetQuestionChangeData() : null;

        var cacheItem = QuestionChangeCacheItem.ToQuestionChangeCacheItem(questionChange, currentData, previousData);

        if (previousData?.Visibility == QuestionVisibility.Private &&
            cacheItem.Visibility == QuestionVisibility.Public)
            LastPublishDate = cacheItem.DateCreated;

        QuestionChangeCacheItems.Insert(0, cacheItem);
        EntityCache.AddOrUpdate(this);
    }

    public void AddComment(Comment comment)
    {
        if (CommentIds == null)
        {
            CommentIds = new List<int>();
        }

        CommentIds.Add(comment.Id);
    }

}