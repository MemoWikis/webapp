class CategoryApi {

    public static Pin(categoryId, onPinChanged: () => void = null) {
        $.post("/Api/Category/Pin/", { categoryId: categoryId }, () => {
            if (onPinChanged != null)
                onPinChanged();
        });
        Utils.SendGaEvent("WuWi", "Pin", "Category");
    }

    public static Unpin(categoryId, onPinChanged: () => void = null) {
        $.post("/Api/Category/Unpin/", { categoryId: categoryId }, () => {
            if (onPinChanged != null)
                onPinChanged();
        });
        Utils.SendGaEvent("WuWi", "Unpin", "Category");
    }

    static NavigatoTo(categoryName) {
        $.get("/Api/Category/GetUrl?categoryName=" + categoryName,
            function (data) { window.location.href = data; }
        );
    }
    
} 