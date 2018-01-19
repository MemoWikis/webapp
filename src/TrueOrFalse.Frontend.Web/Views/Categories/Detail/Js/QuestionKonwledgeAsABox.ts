
 // Plan : Wunschwissen debuggen den AjaxCall finden der die Fragen und deren Zustände zurückgibt,daraus ergibt sich die Größe der For Schleife (üerprüfen ob foreach besser ),
// Farben der Kästchen sollen über Klassen im css definiert werden (überprüfen ob schon vorhanden)
//Größe der Kästchen entweder verschiedene Größen je nach ANzahl oder man gibt Ihnen einen bestimmten Platz und lässt sie skalieren 
// beim hovern über das Kästchen soll die Frage eingeblendet werden (überprüfen ob FrageObjekt die komplette Frage mitgibt ansonsten per Id aus Datenbank holen )


//momentane Probleme Klasse muss ja während dem Durchlauf durch die Schleife angepasst werden ,da die Fragen unterschiedliche Zustände haben (vielleicht kann man in der Schleife nach Zuständen filtern  if(muss noch lernen)?klasseGelb:klasse grün)

    
    $(document).ready(function () {

        for (var i = 0; i < 1800; i++) {
           
            $('#knowledgeAsABox').append("<span style ='float:left; height: 20px; width: 20px; background-color: green; border: black 1px solid '></span>");
        }
        
        
    });

