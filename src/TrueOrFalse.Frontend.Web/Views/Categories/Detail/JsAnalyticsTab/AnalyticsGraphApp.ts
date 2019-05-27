function loadCategoryGraph() {
    $('#AnalyticsTabContent .tab-body').html('<div style="text-align: center"><i class="fa fa-spinner fa-spin"></i></div>');
    $.ajax({
        url: '/EditCategory/GetCategoryGraphDisplay?categoryId=' + $('#hhdCategoryId').val(),
        type: 'GET',
        success: data => {
            $('#AnalyticsTabContent .tab-body')
                .html(data);
        }
    });
}
