<div class="session-progress-bar session-title" v-if="showProgressBar">
    <div class="step-count">
        {{currentStep}} / {{steps}}
    </div>
    <div class="progress-percentage">
        {{progress}}%
    </div>

    <div class="progress-bar" :style="progressBarWidth"></div>
</div>