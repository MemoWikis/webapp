import { AnswerState } from '~/components/page/learning/learningSessionStore'
import { SolutionType } from '~/components/question/solutionTypeEnum'

export class SessionAnswer {
    answerState: AnswerState
    answerAsHtml?: string

    constructor(answerState: AnswerState, answerAsHtml?: string) {
        this.answerState = answerState
        this.answerAsHtml = answerAsHtml
    }

    get isCorrect(): boolean {
        return this.answerState === AnswerState.Correct
    }

    get isUnanswered(): boolean {
        return this.answerState === AnswerState.Unanswered
    }

    get isSkipped(): boolean {
        return this.answerState === AnswerState.Skipped
    }

    get isFalse(): boolean {
        return this.answerState === AnswerState.False
    }
}

export class Question {
    correctAnswerHtml: string = ""
    title: string = ""
    sessionAnswers: SessionAnswer[] = []
    imgUrl: string = ""
    id: number = 0
    solutionType: SolutionType = SolutionType.Flashcard

    get isUnanswered(): boolean {
        return this.sessionAnswers.every(s => s.isUnanswered)
    }

    get hasWrongAnswer(): boolean {
        return this.sessionAnswers.some(s => s.isFalse) && this.sessionAnswers.every(s => !s.isCorrect)
    }
}
