Vue.component('category-image-component',
    {
        props: ['categoryId','isLearningTab'],
        data() {
            return {
                imgSrc: '',
                disabled: false,
            }
        },
        created() {
            this.imgSrc = $("#CategoryHeaderImg").attr('src');

        },
        mounted() {
            if (this.isLearningTab == 'True') {
                this.controlTab('LearningTab');
            };
            eventBus.$on('request-save', () => this.saveImage());
            eventBus.$on('cancel-edit-mode', () => { $("#CategoryHeaderImg").attr('src', this.imgSrc); });
            eventBus.$on('tab-change',
                (tabName) => {
                    this.controlTab(tabName);
                });
        },
        methods: {
            controlTab(tabName) {
                if (tabName == 'TopicTab')
                    this.disabled = false;
                else
                    this.disabled = true;
            },
            openImageUploadModal() {
                if (this.disabled)
                    return;
                $("#modalImageUpload").modal('show');
            },
            saveImage() {
                var imageIsNew = $("#ImageIsNew").val() == 'true';
                if (!imageIsNew)
                    return;

                var json = {
                    categoryId: this.categoryId,
                    source: $("#ImageSource").val(),
                    wikiFileName: $("#ImageWikiFileName").val(),
                    guid: $("#ImageGuid").val(),
                    licenseOwner: $("#ImageLicenseOwner").val()
                }
                $.ajax({
                    type: 'Post',
                    contentType: "application/json",
                    url: '/EditCategory/SaveImage',
                    data: JSON.stringify(json),
                    success: function (result) {
                    },
                });
            }
        }
    })