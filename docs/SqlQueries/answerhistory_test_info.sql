SELECT 
	AlgoId,
	a.AnswerredCorrectly,
	IsCorrect,
	Probability,
	CASE IsCorrect 
		WHEN True THEN 100 - Probability
	   WHEN False THEN ABS(0 - Probability)
	END as Distance,
	CASE IsCorrect 
		WHEN True THEN POW(100 - Probability,2)
	   WHEN False THEN POW(ABS(0 - Probability),2)
	END as AvgDistanceWeighted	
FROM answerhistory_test a_test
LEFT JOIN answerhistory a
ON a_test.AnswerHistory_id = a.Id 