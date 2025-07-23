using System.Collections.Concurrent;

public class ExtendedUserCacheItem : UserCacheItem
{
    public ConcurrentDictionary<int, PageValuation> PageValuations = new();
    public ConcurrentDictionary<int, QuestionValuationCacheItem> QuestionValuations = new();
    public ConcurrentDictionary<int, AnswerRecord> AnswerCounter = new();
    public ConcurrentDictionary<int, UserSkillCacheItem> Skills = new();

    public static ExtendedUserCacheItem CreateCacheItem(User user, PageViewRepo pageViewRepo)
    {
        var sessionUserCacheItem = new ExtendedUserCacheItem();

        if (user != null)
        {
            sessionUserCacheItem.Populate(user);
        }

        return sessionUserCacheItem;
    }

    public void AddOrUpdateQuestionValuations(QuestionValuationCacheItem questionValuationCacheItem)
    {
        QuestionValuations.TryGetValue(questionValuationCacheItem.Question.Id, out var valuation);

        if (valuation == null)
        {
            var result = QuestionValuations
                .TryAdd(questionValuationCacheItem.Question.Id, questionValuationCacheItem);

            if (result == false)
                Log.Error(
                    $"QuestionValuationCacheItem with Id {questionValuationCacheItem.Question.Id}" +
                    $" could not be added in {nameof(AddOrUpdateQuestionValuations)}");
        }
        else
        {
            var result = QuestionValuations.TryUpdate(
                questionValuationCacheItem.Question.Id,
                questionValuationCacheItem,
                valuation);

            if (result == false)
                Log.Error(
                    $"QuestionValuationCacheItem with Id {questionValuationCacheItem.Question.Id} " +
                    $"could not be updated in {nameof(AddOrUpdateQuestionValuations)}");
        }
    }

    public void AddOrUpdateSkill(UserSkillCacheItem userSkillCacheItem)
    {
        Skills.TryGetValue(userSkillCacheItem.PageId, out var existingSkill);

        if (existingSkill == null)
        {
            var result = Skills.TryAdd(userSkillCacheItem.PageId, userSkillCacheItem);

            if (result == false)
                Log.Error(
                    $"UserSkillCacheItem with PageId {userSkillCacheItem.PageId}" +
                    $" could not be added in {nameof(AddOrUpdateSkill)}");
        }
        else
        {
            var result = Skills.TryUpdate(
                userSkillCacheItem.PageId,
                userSkillCacheItem,
                existingSkill);

            if (result == false)
                Log.Error(
                    $"UserSkillCacheItem with PageId {userSkillCacheItem.PageId} " +
                    $"could not be updated in {nameof(AddOrUpdateSkill)}");
        }
    }

    public void RemoveSkill(int pageId)
    {
        Skills.TryRemove(pageId, out _);
    }

    public UserSkillCacheItem? GetSkill(int pageId)
    {
        Skills.TryGetValue(pageId, out var skill);
        return skill;
    }

    public ICollection<UserSkillCacheItem> GetAllSkills()
    {
        return Skills.Values;
    }
}