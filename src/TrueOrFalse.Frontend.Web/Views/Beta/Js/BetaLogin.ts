 $(() => {
     $("#btnEnter").click((e) => {
         e.preventDefault();

         $.post("/Beta/IsValidBetaUser", { betacode: $("#txtBetaCode").val()} , (data) => {
             if (data.IsValid) {
                 window.location.href = "/";
             } else {
                 $("#msgInvalidBetaCode").fadeIn(500);
             }
         });
     });
 });