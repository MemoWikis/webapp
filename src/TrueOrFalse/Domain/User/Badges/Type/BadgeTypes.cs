using System.Collections.Generic;
using System.Linq;
using NHibernate;

public class BadgeTypes
{
    private static IList<BadgeType> _allBadgeTypes;
    public static IList<BadgeType> All()
    {
        return _allBadgeTypes ?? (_allBadgeTypes = new List <BadgeType>
        {
            //FirstSteps
            new BadgeType
            {
                Key = "FirstHourSupporter",
                Name = "Fördermitglied der 1. Stunde",
                Description = "Während des 1. Jahres Fördermitglied geworden (und für ein Jahr geblieben)",
                Group =  BadgeTypeGroups.GetByKey(BadgeTypeGroupKeys.FirstSteps),
                Levels = new List<BadgeLevel>{ BadgeLevel.GetGold()},
                BadgeCheckOn = new []{ BadgeCheckOn.OncePerDay },
                AwardCheck = filterParams => new BadgeAwardCheckResult {Success = false},
            },
            new BadgeType
            {
                Key = "FirstHourUser",
                Name = "Nutzer der 1. Stunde",
                Description = "Während der Beta-Phase Nutzer geworden",
                Group =  BadgeTypeGroups.GetByKey(BadgeTypeGroupKeys.FirstSteps),
                Levels = new List<BadgeLevel>{ BadgeLevel.GetBronze()},
                BadgeCheckOn = new []{ BadgeCheckOn.Registration },
                AwardCheck = BadgeAwardCheck.AlwaysFalse(),
            },
            new BadgeType
            {
                Key = "FirstHourUser",
                Name = "Beta-Berater",
                Description = "Per Hand verliehen an alle, die während der Beta-Phase memucho genutzt und uns beraten haben",
                Group =  BadgeTypeGroups.GetByKey(BadgeTypeGroupKeys.FirstSteps),
                Levels = new List<BadgeLevel>{ BadgeLevel.GetSilver()},
                BadgeCheckOn = new []{ BadgeCheckOn.Manually },
                AwardCheck = filterParams => new BadgeAwardCheckResult {Success = false},
            },
            new BadgeType
            {
                Key = "NewbieBronze",
                Name = "Newbie",
                Description = "1 Frage im Wuwi, 1 Frage beantwortet",
                Group =  BadgeTypeGroups.GetByKey(BadgeTypeGroupKeys.FirstSteps),
                Levels = new List<BadgeLevel>{ BadgeLevel.GetBronze()},
                BadgeCheckOn = new []{ BadgeCheckOn.Answer, BadgeCheckOn.WishKnowledgeAdd},
                AwardCheck = BadgeAwardCheck.Get(fp => 
                    fp.WuWi_Count() >= 1 && fp.AnswerCount() >= 1
                        ? BadgeLevel.GetBronze()
                        : null),
            },
            new BadgeType
            {
                Key = "NewbieSilver",
                Name = "Newbie",
                Description = "2 Multiple-Choice-Fragen mit Thema erstellt, 2 Lernsets mit mind. 10 Fragen erstellt, 2 fremde und 2 eigene Fragen in Wunschwissen aufgenommen",
                Group =  BadgeTypeGroups.GetByKey(BadgeTypeGroupKeys.FirstSteps),
                Levels = new List<BadgeLevel>{ BadgeLevel.GetSilver()},
                BadgeCheckOn = new []{ BadgeCheckOn.QuestionUpdateOrCreate, BadgeCheckOn.SetUpdateOrCreate, BadgeCheckOn.WishKnowledgeAdd},
                AwardCheck = BadgeAwardCheck.Get(fp =>
                    fp.Questions_MultipleChoice_WithCategories() >= 2 &&
                    fp.SetsWithAtLeast10Questions().Count() >= 2 &&
                    fp.WuWi_UserIsCreator() >= 2 &&
                    fp.WuWi_OtherIsCreator() >= 2
                        ? BadgeLevel.GetSilver()
                        : null),
            },
            new BadgeType
            {
                Key = "NewbieGold",
                Name = "Newbie",
                Description = "wie Silber, dazu: 3 Spiele gespielt; 3 Termine angelegt; 3 Nutzern gefolgt, 30 Fragen im Wuwi, 1 Kommentar geschrieben, 1 Thema erstellt",
                Group =  BadgeTypeGroups.GetByKey(BadgeTypeGroupKeys.FirstSteps),
                Levels = new List<BadgeLevel>{ BadgeLevel.GetGold()},
                BadgeCheckOn = new []{ BadgeCheckOn.GameFinished, BadgeCheckOn.DateCreated, BadgeCheckOn.UserFollowed, BadgeCheckOn.WishKnowledgeAdd, BadgeCheckOn.CommentedAdded, BadgeCheckOn.CategoryUpdateOrCreate },
                AwardCheck = BadgeAwardCheck.Get(fp => 
                    fp.IsBadgeAwarded("NewbieSilver", fp) &&
                    fp.UsersFollowing() >= 3
                        ? BadgeLevel.GetGold() 
                        : null)
            },
            new BadgeType
            {
                Key = "HelloWorld",
                Name = "Hello World",
                Description = "Vollständiges Profil",
                Group =  BadgeTypeGroups.GetByKey(BadgeTypeGroupKeys.FirstSteps),
                Levels = new List<BadgeLevel>{ BadgeLevel.GetBronze()},
                BadgeCheckOn = new []{ BadgeCheckOn.UserProfileUpdated },
                /* NOT-DONE AwardCheck = BadgeAwardCheck.Get(filterParams => null) */
            },
            
            //Questions
            new BadgeType
            {
                Key = "1000Words",
                Name = "MehrAls1000Worte",
                Description = "{badgePoints} Fragen mit Bildern erstellt",
                Group =  BadgeTypeGroups.GetByKey(BadgeTypeGroupKeys.Questions),
                Levels = new List<BadgeLevel>
                {
                    BadgeLevel.GetBronze(1),
                    BadgeLevel.GetSilver(50),
                    BadgeLevel.GetGold(500)
                },
                BadgeCheckOn = new []{ BadgeCheckOn.QuestionUpdateOrCreate },
                AwardCheck = BadgeAwardCheck.GetLevel(fp => fp.Questions_WithImages())
            },

            new BadgeType
            {
                Key = "ABCD",
                Name = "ABCD",
                Description = "{badgePoints} Multiple-Choice-Fragen erstellt",
                Group =  BadgeTypeGroups.GetByKey(BadgeTypeGroupKeys.Questions),
                Levels = new List<BadgeLevel>
                {
                    BadgeLevel.GetBronze(1),
                    BadgeLevel.GetSilver(30),
                    BadgeLevel.GetGold(500)
                },
                BadgeCheckOn = new []{ BadgeCheckOn.QuestionUpdateOrCreate },
                AwardCheck = BadgeAwardCheck.GetLevel(fp => fp.Questions_MultipleChoice())
            },

            new BadgeType
            {
                Key = "KnowItAll",
                Name = "Allgemeinwisser",
                Description = "eine eigene Frage wurde {badgePoints} Mal in fremdes Wuwi aufgenommen",
                Group =  BadgeTypeGroups.GetByKey(BadgeTypeGroupKeys.Questions),
                Levels = new List<BadgeLevel>
                {
                    BadgeLevel.GetBronze(1),
                    BadgeLevel.GetSilver(30),
                    BadgeLevel.GetGold(300)
                },
                BadgeCheckOn = new []{ BadgeCheckOn.WishKnowledgeAdd },
                AwardCheck = BadgeAwardCheck.GetLevel(fp => fp.Questions_InOtherPeopleWuwi())
            },
            
            //Sets
            new BadgeType
            {
                Key = "DonDoItSelf",
                Name = "IchMachsDochNichtSelbst",
                Description = "{badgePoints} Lernsets mit mehr als 10 Fragen, mit mind. 1 fremden Frage zusammengestellt",
                Group =  BadgeTypeGroups.GetByKey(BadgeTypeGroupKeys.Sets),
                Levels = new List<BadgeLevel>
                {
                    BadgeLevel.GetBronze(1),
                    BadgeLevel.GetSilver(30),
                    BadgeLevel.GetGold(200)
                },
                BadgeCheckOn = new []{ BadgeCheckOn.SetUpdateOrCreate },
                AwardCheck = BadgeAwardCheck.GetLevel(fp =>
                {
                    var sets = fp.SetsWithAtLeast10Questions();

                    return sets.Count(s => 
                        s.QuestionsInSet.Any(y => 
                            y.Question.Creator.Id != fp.CurrentUser.Id)
                    );
                })
            },

            //Categories
            new BadgeType
            {
                Key = "FamilyFriend",
                Name = "Familienfreund",
                Description = "{badgePoints} Eltern-/Kindthemen verknüpft",
                Group =  BadgeTypeGroups.GetByKey(BadgeTypeGroupKeys.Categories),
                Levels = new List<BadgeLevel>
                {
                    BadgeLevel.GetBronze(20),
                    BadgeLevel.GetSilver(100),
                    BadgeLevel.GetGold(400)
                },
                BadgeCheckOn = new []{ BadgeCheckOn.CategoryUpdateOrCreate },
                /* AwardCheck = WE DO NOT SAVE WHO CREATED CONNECTIONS */
            },

            new BadgeType
            {
                Key = "Expert",
                Name = "VomFach",
                Description = "{badgePoints} Fragen zu einem Thema erstellt",
                Group =  BadgeTypeGroups.GetByKey(BadgeTypeGroupKeys.Categories),
                Levels = new List<BadgeLevel>
                {
                    BadgeLevel.GetBronze(10),
                    BadgeLevel.GetSilver(100),
                    BadgeLevel.GetGold(300)
                },
                BadgeCheckOn = new []{ BadgeCheckOn.QuestionUpdateOrCreate },
                AwardCheck = BadgeAwardCheck.GetLevel(fp => fp.Questions_MaxAddedToCategory())
            },

            new BadgeType
            {
                Key = "Universalist",
                Name = "Universalist",
                Description = "{badgePoints} verschiedene Themen zu eigenen Fragen zugeordnet",
                Group =  BadgeTypeGroups.GetByKey(BadgeTypeGroupKeys.Categories),
                Levels = new List<BadgeLevel>
                {
                    BadgeLevel.GetBronze(5),
                    BadgeLevel.GetSilver(50),
                    BadgeLevel.GetGold(200)
                },
                BadgeCheckOn = new []{ BadgeCheckOn.QuestionUpdateOrCreate },
                AwardCheck = BadgeAwardCheck.GetLevel(fp => fp.CountDifferentCategoriesAddedToQuestion())
            },

            //WishKnowledge
            new BadgeType
            {
                Key = "WannaKnow",
                Name = "WillsWissen",
                Description = "{badgePoints} Fragen im eigenen Wunschwissen hinzugefügt",
                Group =  BadgeTypeGroups.GetByKey(BadgeTypeGroupKeys.WishKnowledge),
                Levels = new List<BadgeLevel>
                {
                    BadgeLevel.GetBronze(5),
                    BadgeLevel.GetSilver(200),
                    BadgeLevel.GetGold(1000)
                },
                BadgeCheckOn = new []{ BadgeCheckOn.WishKnowledgeAdd },
                AwardCheck = BadgeAwardCheck.GetLevel(fp => fp.WuWi_Count())
            },

            new BadgeType
            {
                Key = "Archaeologist",
                Name = "Archäologe",
                Description = "{badgePoints} Fragen zum Wuwi hinzugefügt, die älter als 2 Jahre sind",
                Group =  BadgeTypeGroups.GetByKey(BadgeTypeGroupKeys.WishKnowledge),
                Levels = new List<BadgeLevel>
                {
                    BadgeLevel.GetBronze(5),
                    BadgeLevel.GetSilver(50),
                    BadgeLevel.GetGold(500)
                },
                BadgeCheckOn = new []{ BadgeCheckOn.WishKnowledgeAdd },
                /* TODO: PENDING QUESTION */
            },

            new BadgeType
            {
                Key = "FasterThanShadow",
                Name = "SchnellerAlsMeinSchatten",
                Description = "{badgePoints} fremde Fragen zum Wunschwissen hinzugefügt, die neuer als 24h sind.",
                Group =  BadgeTypeGroups.GetByKey(BadgeTypeGroupKeys.WishKnowledge),
                Levels = new List<BadgeLevel>
                {
                    BadgeLevel.GetBronze(1),
                    BadgeLevel.GetSilver(20),
                    BadgeLevel.GetGold(200)
                },
                BadgeCheckOn = new []{ BadgeCheckOn.WishKnowledgeAdd },
                AwardCheck = BadgeAwardCheck.GetLevel(fp => fp.WuWi_AddedInLessThan24Hours())
            },

            new BadgeType
            {
                Key = "ThanksForHelp",
                Name = "DankeFürDieHilfe",
                Description = "{badgePoints} fremde Fragen im eigenen Wuwi",
                Group =  BadgeTypeGroups.GetByKey(BadgeTypeGroupKeys.WishKnowledge),
                Levels = new List<BadgeLevel>
                {
                    BadgeLevel.GetBronze(1),
                    BadgeLevel.GetSilver(100),
                    BadgeLevel.GetGold(500)
                },
                BadgeCheckOn = new []{ BadgeCheckOn.WishKnowledgeAdd },
                AwardCheck = BadgeAwardCheck.GetLevel(fp => fp.WuWi_OtherIsCreator())
            },

            //Training
            new BadgeType
            {
                Key = "Swot",
                Name = "Streber",
                Description = "{badgePoints} Tage am Stück gelernt [nicht: Frage beantwortet]",
                Group =  BadgeTypeGroups.GetByKey(BadgeTypeGroupKeys.Training),
                Levels = new List<BadgeLevel>
                {
                    BadgeLevel.GetBronze(3),
                    BadgeLevel.GetSilver(30),
                    BadgeLevel.GetGold(300)
                },
                BadgeCheckOn = new []{ BadgeCheckOn.OncePerDay },
                AwardCheck = BadgeAwardCheck.GetLevel(fp => 
                    Sl.R<GetStreaksDays>().Run(fp.CurrentUser, onlyLearningSessions : true).LongestLength
                )
            },

            new BadgeType
            {
                Key = "OldStager",
                Name = "Alter Hase",
                Description = "an insgesamt {badgePoints} Tagen gelernt [nicht: Frage beantwortet]",
                Group =  BadgeTypeGroups.GetByKey(BadgeTypeGroupKeys.Training),
                Levels = new List<BadgeLevel>
                {
                    BadgeLevel.GetBronze(3),
                    BadgeLevel.GetSilver(30),
                    BadgeLevel.GetGold(300)
                },
                BadgeCheckOn = new []{ BadgeCheckOn.OncePerDay },
                AwardCheck = BadgeAwardCheck.GetLevel(fp =>
                    Sl.R<GetStreaksDays>().Run(fp.CurrentUser, onlyLearningSessions : true).TotalLearningDays
                )
            },

            new BadgeType
            {
                Key = "EarlyRiser",
                Name = "Frühaufsteher",
                Description = "schon an {badgePoints} Tagen zwischen 5-9 Uhr Fragen beantwortet [inkl. gelernt]",
                Group =  BadgeTypeGroups.GetByKey(BadgeTypeGroupKeys.Training),
                Levels = new List<BadgeLevel>
                {
                    BadgeLevel.GetBronze(2),
                    BadgeLevel.GetSilver(50),
                    BadgeLevel.GetGold(200)
                },
                BadgeCheckOn = new []{ BadgeCheckOn.OncePerDay },
                AwardCheck = BadgeAwardCheck.GetLevel(fp =>
                    Sl.R<GetStreaksDays>().Run(fp.CurrentUser, startHour:5, endHour:9).TotalLearningDays
                )
            },

            new BadgeType
            {
                Key = "Napper",
                Name = "Mittagsschläfer",
                Description = "In den letzten {badgePoints} Tagen nie zwischen 12-13 Uhr Fragen beantwortet.",
                Group =  BadgeTypeGroups.GetByKey(BadgeTypeGroupKeys.Training),
                Levels = new List<BadgeLevel>
                {
                    BadgeLevel.GetBronze(50),
                    BadgeLevel.GetSilver(200),
                    BadgeLevel.GetGold(1000)
                },
                BadgeCheckOn = new []{ BadgeCheckOn.OncePerDay },
                /* TODO: */
            },

            new BadgeType
            {
                Key = "AskMe",
                Name = "FragMich",
                IsSecret = true,
                Description = "{badgePoints} Fragen beantwortet",
                Group =  BadgeTypeGroups.GetByKey(BadgeTypeGroupKeys.Training),
                Levels = new List<BadgeLevel>
                {
                    BadgeLevel.GetBronze(76),
                    BadgeLevel.GetSilver(543),
                    BadgeLevel.GetGold(2808)
                },
                BadgeCheckOn = new []{ BadgeCheckOn.OncePerDay },
                AwardCheck = BadgeAwardCheck.GetLevel(fp => fp.AnswerCount())
            },

            new BadgeType
            {
                Key = "Strike",
                Name = "Strike",
                Description = "{badgePoints} Fragen am Stück richtig beantwortet",
                Group =  BadgeTypeGroups.GetByKey(BadgeTypeGroupKeys.Training),
                Levels = new List<BadgeLevel>
                {
                    BadgeLevel.GetBronze(5),
                    BadgeLevel.GetSilver(50),
                    BadgeLevel.GetGold(200)
                },
                BadgeCheckOn = new []{ BadgeCheckOn.OncePerDay },
                AwardCheck = BadgeAwardCheck.GetLevel(fp =>{
                    var result = GetStreakCorrectness.Run(fp.CurrentUser.Id);
                    return result == null ? -1 : (int)result.StreakLength;
                })
            },

            new BadgeType
            {
                Key = "NextPlease",
                Name = "NächsteBitte",
                Description = "{badgePoints} Fragen beim Lernen übersprungen",
                Group =  BadgeTypeGroups.GetByKey(BadgeTypeGroupKeys.Training),
                Levels = new List<BadgeLevel>
                {
                    BadgeLevel.GetBronze(5),
                    BadgeLevel.GetSilver(20),
                    BadgeLevel.GetGold(200)
                },
                BadgeCheckOn = new []{ BadgeCheckOn.OncePerDay },
                /* TODO:  */
            },

            new BadgeType
            {
                Key = "JustTellMe",
                Name = "SagsMirEinfach",
                Description = "{badgePoints} Mal auf “Antwort anzeigen” geklickt",
                Group =  BadgeTypeGroups.GetByKey(BadgeTypeGroupKeys.Training),
                Levels = new List<BadgeLevel>
                {
                    BadgeLevel.GetBronze(5),
                    BadgeLevel.GetSilver(200),
                    BadgeLevel.GetGold(1000)
                },
                BadgeCheckOn = new []{ BadgeCheckOn.OncePerDay },
                /* TODO:  */
            },

            //Community/Comments
            new BadgeType
            {
                Key = "Networker",
                Name = "Vernetzer",
                Description = "{badgePoints} Freunde im Netzwerk",
                Group =  BadgeTypeGroups.GetByKey(BadgeTypeGroupKeys.Community),
                Levels = new List<BadgeLevel>
                {
                    BadgeLevel.GetBronze(5),
                    BadgeLevel.GetSilver(30),
                    BadgeLevel.GetGold(200)
                },
                BadgeCheckOn = new []{ BadgeCheckOn.OncePerDay },
                /* TODO:  */
            },

            new BadgeType
            {
                Key = "Famous",
                Name = "Berühmtheit",
                Description = "Von {badgePoints} Nutzern gefolgt werden",
                Group =  BadgeTypeGroups.GetByKey(BadgeTypeGroupKeys.Community),
                Levels = new List<BadgeLevel>
                {
                    BadgeLevel.GetBronze(5),
                    BadgeLevel.GetSilver(30),
                    BadgeLevel.GetGold(200)
                },
                BadgeCheckOn = new []{ BadgeCheckOn.OncePerDay },
                /* TODO:  */
            },

            new BadgeType
            {
                Key = "KnowItAll",
                Name = "Besserwisser",
                Description = "{badgePoints} Verbesserungsvorschläge gemacht",
                Group =  BadgeTypeGroups.GetByKey(BadgeTypeGroupKeys.Community),
                Levels = new List<BadgeLevel>
                {
                    BadgeLevel.GetBronze(2),
                    BadgeLevel.GetSilver(50),
                    BadgeLevel.GetGold(400)
                },
                BadgeCheckOn = new []{ BadgeCheckOn.OncePerDay },
                /* TODO:  */
            },

            new BadgeType
            {
                Key = "ControllerType",
                Name = "Kontroletti",
                Description = "{badgePoints} Löschaufträge gestellt",
                Group =  BadgeTypeGroups.GetByKey(BadgeTypeGroupKeys.Community),
                Levels = new List<BadgeLevel>
                {
                    BadgeLevel.GetBronze(2),
                    BadgeLevel.GetSilver(50),
                    BadgeLevel.GetGold(300)
                },
                BadgeCheckOn = new []{ BadgeCheckOn.OncePerDay },
                /* TODO:  */
            },

            new BadgeType
            {
                Key = "Windbag",
                Name = "Quatschkopf",
                Description = "{badgePoints} Kommentare hinzugefügt",
                Group =  BadgeTypeGroups.GetByKey(BadgeTypeGroupKeys.Community),
                Levels = new List<BadgeLevel>
                {
                    BadgeLevel.GetBronze(2),
                    BadgeLevel.GetSilver(100),
                    BadgeLevel.GetGold(500)
                },
                BadgeCheckOn = new []{ BadgeCheckOn.OncePerDay },
                /* TODO:  */
            },
        });
    }
}