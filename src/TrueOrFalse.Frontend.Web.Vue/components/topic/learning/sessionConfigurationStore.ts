import { defineStore } from 'pinia'
import { useUserStore } from '~~/components/user/userStore'
import { useTopicStore } from '../topicStore'

export class QuestionFilterOptions {
    inWuwi: {
        count: 0,
        label: 'Im Wunschwissen',
        icon: 'fas fa-heart',
        isSelected: true,
        questionIds: [],
    }
    notInWuwi: {
        count: 0,
        label: 'Nicht im Wunschwissen',
        icon: 'fa fa-heart-o',
        isSelected: true,
        questionIds: [],
    }
    createdByCurrentUser: {
        count: 0,
        label: 'Von mir erstellt',
        icon: 'fas fa-user-check',
        isSelected: true,
        questionIds: [],
    }
    notCreatedByCurrentUser: {
        count: 0,
        label: 'Nicht von mir erstellt',
        icon: 'fas fa-user-slash',
        isSelected: true,
        questionIds: [],
    }
    privateQuestions: {
        count: 0,
        label: 'Private Fragen',
        icon: 'fas fa-lock',
        isSelected: true,
        questionIds: [],
    }
    publicQuestions: {
        count: 0,
        label: 'Öffentliche Fragen',
        icon: 'fas fa-globe',
        isSelected: true,
        questionIds: [],
    }
}

export class KnowledgeSummary {
    notLearned: {
        count: 0,
        label: 'Noch nicht Gelernt',
        colorClass: 'not-learned',
        isSelected: true,
        questionIds: [],
    }
    needsLearning: {
        count: 0,
        label: 'Zu Lernen',
        colorClass: 'needs-learning',
        isSelected: true,
        questionIds: [],
    }
    needsConsolidation: {
        count: 0,
        label: 'Zu Festigen',
        colorClass: 'needs-consolidation',
        isSelected: true,
        questionIds: [],
    }
    solid: {
        count: 0,
        label: 'Sicher gewußt',
        colorClass: 'solid',
        isSelected: true,
        questionIds: [],
    }
}
export enum QuestionOrder {
    Easiest,
    Hardest,
    PersonalHardest,
    Random
}

export enum RepetitionType {
    Normal,
    Leitner,
    None
}

export const useSessionConfigurationStore = defineStore('sessionConfigurationStore', {
    state: () => {
      return {
        order: QuestionOrder.Random,
        repetition: RepetitionType.Normal,
        questionsCount: 0,
        allQuestionCount: 0,
        currentQuestionCount: 0,
        knowledgeSummary: new KnowledgeSummary,
        questionFilterOptions: new QuestionFilterOptions,
        isTestMode: true,
        isPracticeMode: false,
        categoryHasNoQuestions: true,
        showError: false,
      }
    },
    actions: {
        startNewSession(){
            
        }
    },
  })

  