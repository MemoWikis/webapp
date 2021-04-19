Vue.component('category-image-component',
    {
        props: ['categoryId'],
        data() {
            return {
                imgSrc: ''
            }
        },
        created() {
            this.imgSrc = $("#CategoryHeaderImg").attr('src');

        },
        mounted() {
            eventBus.$on('request-save', () => this.saveImage());
            eventBus.$on('cancel-edit-mode', () => { $("#CategoryHeaderImg").attr('src', this.imgSrc); });
        },
        methods: {
            openImageUploadModal() {
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