class CategoryApi {

    public static Pin(setId, onPinChanged: () => void = null) {
        $.post("/Api/Category/Pin/", { categoryId: setId }, () => {
            if (onPinChanged != null)
                onPinChanged();
        });
        Utils.SendGaEvent("WuWi", "Pin", "Category");
    }

    public static Unpin(setId, onPinChanged: () => void = null) {
        $.post("/Api/Category/Unpin/", { categoryId: setId }, () => {
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