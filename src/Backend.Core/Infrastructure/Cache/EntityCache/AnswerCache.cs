public class AnswerCache
{
    public static void AddAnswerToCache(ExtendedUserCache extendedUserCache, Answer answer)
    {
        var userId = answer.UserId;
        if (userId < 1)
        {
            var question = EntityCache.GetQuestion(answer.Question.Id);
            if (question != null)
            {
                question.AnswerCounter = UpdateAnswerRecord(question.AnswerCounter, answer);
                EntityCache.AddOrUpdate(question);
            }
        }
        else
        {
            var user = extendedUserCache.GetUser(userId);
            user.AnswerCounter.AddOrUpdate(
                answer.Question.Id,
                new AnswerRecord(0, 0, 0, 0),
                (key, existingValue) => UpdateAnswerRecord(existingValue, answer)
            );
        }
    }

    public static void UpdateAnswerInCache(ExtendedUserCache extendedUserCache, Answer answer)
    {
        var userId = answer.UserId;
        if (userId < 1)
        {
            var question = EntityCache.GetQuestion(answer.Question.Id);
            if (question != null)
            {
                question.AnswerCounter = UpdateAnswerRecord(question.AnswerCounter, answer);
                EntityCache.AddOrUpdate(question);
            }
        }
        else
        {
            var user = extendedUserCache.GetUser(userId);
            user.AnswerCounter.AddOrUpdate(
                answer.Question.Id,
                new AnswerRecord(0, 0, 0, 0),
                (key, existingValue) => UpdateAnswerRecord(existingValue, answer)
            );
        }
    }

    private static AnswerRecord UpdateAnswerRecord(AnswerRecord answerRecord, Answer answer)
    {
        switch (answer.AnswerredCorrectly)
        {
            case AnswerCorrectness.True:
                answerRecord.True++;
                break;
            case AnswerCorrectness.False:
                answerRecord.False++;
                break;
            case AnswerCorrectness.MarkedAsTrue:
                answerRecord.MarkedAsTrue++;
                break;
            case AnswerCorrectness.IsView:
                answerRecord.View++;
                break;
        }

        return answerRecord;
    }

    public static AnswerRecord AnswersToAnswerRecord(IEnumerable<Answer> answers)
    {
        var answerRecord = new AnswerRecord(0, 0, 0, 0);

        foreach (var answer in answers)
        {
            switch (answer.AnswerredCorrectly)
            {
                case AnswerCorrectness.True:
                    answerRecord.True++;
                    break;
                case AnswerCorrectness.False:
                    answerRecord.False++;
                    break;
                case AnswerCorrectness.MarkedAsTrue:
                    answerRecord.MarkedAsTrue++;
                    break;
                case AnswerCorrectness.IsView:
                    answerRecord.View++;
                    break;
            }
        }

        return answerRecord;
    }
}

public record struct AnswerRecord(int False, int True, int MarkedAsTrue, int View);