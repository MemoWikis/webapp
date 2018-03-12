

class NumbersCountUp {

    elem: JQuery[] = new Array();
    isNumber: boolean; //todo: löschen
    isProgressBar: boolean; //todo: löschen
    hasBeenViewed: Array<boolean>[] = new Array(); //speichere hasbeenviewed-info als attribut direkt im element
   // this.elem[0].attr("hasBeenViewed", "false");


    constructor() {
        
        if ($('.CountUpProgress').length != 0) {
            this.elem.push($(".CountUpProgress"));
        }

        if ($('.CountUp').length != 0) {
            this.elem.push($('.CountUp'));
        }

        for (var i = 0; i < this.elem.length; i++) {
            this.hasBeenViewed[i] = new Array();
            for (var j = 0; j < this.elem[i].length; j++) {
                this.hasBeenViewed[i][j] = false;
            }
        }


        this.animateWhenVisible();
    }


    //liefert die Klasse aus dem Array Classes innerhalb des Objektes 
    // ein JqueryElement kann mehrere Klassen besitzen diese werden innerhalb des Jquery Objektes als Array gespeichert
    //, entsprechend muss man das Array durchsuchen
    // keine Prüfung notwendig ob Class vorhanden da im Constructor nur die beiden Klassen CountUp und CountUpProgress zum Array hinzugefügt werden 
    deliverHtmlClass(arrayHtmlClasses) { //todo: löschen
        let wichHtmlClassBool: boolean = true;

        wichHtmlClassBool = arrayHtmlClasses.contains("CountUp");

        if (wichHtmlClassBool == false) {

            return ".CountUpProgress";
        }

        return ".CountUp";
    }


    deliverAttribut(element, attribut: string) { //diese prüfung ob null direkt unten in der funktion machen; dann wird deliverAttribut obsolet und kann weg.

        return element.attributes.getNamedItem(attribut) != null ? 
            element.attributes.getNamedItem(attribut).value : null;

        //try {
        //    return element.attributes.getNamedItem(attribut).value;
        //} catch (e) {
        //    return null;
        //}
    }

    countUp(start: number, end: number, character, htmlClass, index) {

        var loops = 50;
        var intervalSlower;
        var loopCount = 0;
        var value = start;
        var timePointSlower = (end * 0.9);
        var increment = (end - start) / loops;
        var interval = setInterval(updateTimer, 10);
        function slower() {
            intervalSlower = setTimeout(updateTimer, 100);
        }

        function numberWith1000Separator(x) {
            return x.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ".");
        }



        function updateTimer() {

            value += increment;
            loopCount++;

            if (htmlClass == ".CountUpProgress") {
              
                $(htmlClass + `:eq(${index})`).width(value.toFixed(0) + character);
            } else {
                $(htmlClass + `:eq(${index})`).html(value.toFixed(0) + character);
            }

            if (value > timePointSlower && value <= end) {
                clearInterval(interval);
                slower();
            }

            if (loopCount >= loops) {
                clearTimeout(intervalSlower);
                clearInterval(interval);
            }
        }
    }

    eachCountUp(element, index) {

        var self = this;
        let finalnumber = 0;

        // da mit unterschiedlichen Propertys gearbeitet wird, muss die Funktion null zurückgeben und wird deshalb auf null geprüft und je nach Property unterschiedlich behandelt
        let character = self.deliverAttribut(element, "data-character") != null ? self.deliverAttribut(element, "data-character") : "";
        let htmlClass = self.deliverHtmlClass(element.classList);
        parseInt(self.deliverAttribut(element, "data-number")) != null ? finalnumber = parseInt(self.deliverAttribut(element, "data-number")) : console.log("Please add Attribut data-number in your Numbers Count Up ");
        let startNumber = self.deliverAttribut(element, "data-startNumber") != null ? parseInt(self.deliverAttribut(element, "data-startNumber")) : 0;

        switch (htmlClass) {
            case ".CountUp":
                self.countUp(startNumber, finalnumber, character, htmlClass, index);
                break;

            case ".CountUpProgress":
                self.countUp(startNumber, finalnumber, character, htmlClass, index);
                break;

            default:
                console.log("No matching class specified");
                break;
        }

    }


    checkIfVisible(self) {
        self.elem.forEach((element, index) => {
            //var classes = element.className.contains("");
           
            for (let i = 0; i < element.length; i++) {

                var htmlClass = self.deliverHtmlClass(element[i].classList);

                var hT = $(element[i]).offset().top,
                    hH = $(element[i]).outerHeight(),
                    wH = $(window).innerHeight(),
                    wS = $(window).scrollTop();

                if ((wS) > (hT + hH - wH) && self.hasBeenViewed[index][i] == false) {

                    self.eachCountUp(element[i], i);
                    self.hasBeenViewed[index][i] = true;
                }
            }
        });
        return;
    }



    animateWhenVisible() {
        var self = this;

        self.checkIfVisible(self);
        $(document).scroll(function () {

            self.checkIfVisible(self);

        });

    }

}

