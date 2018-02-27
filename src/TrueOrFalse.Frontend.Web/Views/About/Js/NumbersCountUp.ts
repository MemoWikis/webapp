

class NumbersCountUp {
    static countObjectProgress: number;
    static countUpObject: number;

    i: number = 0;
    hasBeenViewed: boolean = true;
    //_class: string = "";
    elem: JQuery[] = new Array();
    isNumber: boolean;
    isProgressBar: boolean;



    constructor() {

        if ($('.CountUpProgress').length != 0) {

            this.elem.push($(".CountUpProgress"));
        }

        if ($('.CountUp').length != 0) {
            this.elem.push($('.CountUp'));
        }
        this.animateWhenVisible();
        //this.eachCountUp();
        // this.myTestFunction();
        // this.animateWhenVisible();



        //if ($('.CountUp').length != 0) {

        //    var countUp = new NumbersCountUp('.CountUp');
        //    countUp.scrollToTheCountUp(countUp);
        //}


    }

    //liefert das Attribut aus dem Array  innerhalb des Objektes 
    // ein JqueryElement kann mehrere Attribute besitzen diese werden innerhalb des Jquery Objektes als Array gespeichert
    //, entsprechend muss man das Array durchsuchen.

    deliverCharacterAttribute(arrayHtmlAttribute) {
        let wichHtmlAttributeBool: boolean = true;
       // let htmlClass = ".CountUp";
        wichHtmlAttributeBool = arrayHtmlAttribute.contains("data-character");

        if (wichHtmlAttributeBool == false) {
            // console.log(".CountUpProgress")
            return  ;
        }
        //console.log(".CountUp")
        return ".CountUp";
    }

    //liefert die Klasse aus dem Array Classes innerhalb des Objektes 
    // ein JqueryElement kann mehrere Klassen besitzen diese werden innerhalb des Jquery Objektes als Array gespeichert
    //, entsprechend muss man das Array durchsuchen
    deliverHtmlClass(arrayHtmlClasses) {
        let wichHtmlClassBool: boolean = true;
        //let htmlClass = ".CountUp";
        wichHtmlClassBool = arrayHtmlClasses.contains("CountUp");

        if (wichHtmlClassBool == false) {
            // console.log(".CountUpProgress")
            return ".CountUpProgress";
        }
        //console.log(".CountUp")
        return ".CountUp";
    }


    deliverFinalNumber(arrayFinalNumber) {
        for (let a of arrayFinalNumber) {
            if (a == "data-number") {
                var test12 = a.value;
                return test12;
            }
        }
        return "";
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

        function numberWithCommas(x) {
            return x.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ".");
        }



        function updateTimer() {

            value += increment;
            loopCount++;

            if (htmlClass == ".CountUpProgress") {
                var test = "";
                test += htmlClass;
                test += ":eq(" + index + ")";
                $(htmlClass + `:eq(${index})`).width(value.toFixed(0) + character);   // richtig verstehen 
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

    eachCountUp(element, index, character="") {
        //var c = new NumbersCountUp();
        var self = this;
        // var e = 0;
        

        // for (var element of self.elem) {
        //// self.elem.forEach(function (value, index, array) {
        //     for (var i = 0; i < element.length; i++) {

        //console.log(e++);
        //console.log(arrayOuter);
        // console.log(element);
        //console.log(element.className);
        //console.log(element.attributes.getNamedItem("data-number").value);
        try {
            character = element.attributes.getNamedItem("data-character").value;
            // console.log(element.attributes.getNamedItem("data-character").value);
        } catch (e) {
            //console.log("is Null");

        }



        // console.log(element[i].attributes.getNamedItem("data-character").value);
        let htmlClass = self.deliverHtmlClass(element.classList);
        let finalnumber = parseInt(element.attributes.getNamedItem("data-number").value);
        let startNumber = 0;

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
        //    }
        //}
        //for(let HtmlClassElements of this.elem) {
        //    for (let i = 0; i < HtmlClassElements.length; i++) {

        //        self.countUp(0, parseInt(self.addAttributes(HtmlClassElements[i].className)), self.addCharacter(HtmlClassElements[i].className), HtmlClassElements[i].className);
        //        console.log(parseInt(self.addAttributes(HtmlClassElements[i].className)));
        //        console.log(self.addCharacter(HtmlClassElements[i].className));
        //     console.log(HtmlClassElements[i].className);
        //        console.log();
        //    }
        //self.elem.each(function () {
        //var temp = self.elem != ".CountUpProgress";
        //var temp1 = self.elem != ".CountUp";
        //      if (".CountUp" == c._class && temp)



        //  self.countUp(0, parseInt(self.addAttributes(c.)), self.addCharacter(self.elem[i].selector), self.elem[i].selector);
        //if ('.CountUpProgress' === c._class && temp1)
        //    c.countUp(0, parseInt(c.addAttributes(this)), this, c.addCharacter(this), c._class);
    }


    //for(let htmlElement in self.elem) {
    //if (self.elem.hasOwnProperty(htmlElement)) {
    ////self.elem.each(function () {
    ////var temp = self.elem != ".CountUpProgress";
    ////var temp1 = self.elem != ".CountUp";
    ////      if (".CountUp" == c._class && temp)
    //console.log(htmlElement);
    //self.countUp(0, parseInt(self.addAttributes(htmlElement)), self.addCharacter(htmlElement), htmlElement);
    ////if ('.CountUpProgress' === c._class && temp1)
    ////    c.countUp(0, parseInt(c.addAttributes(this)), this, c.addCharacter(this), c._class);
    //}
    //}


    animateWhenVisible() {
        var self = this;
        var j = 0;
        $(window).scroll(function () {

            for (var element of self.elem) {
               
                
                for (let i = 0; i < element.length; i++) {

                    var htmlClass = self.deliverHtmlClass(element[i].classList);
                    //var character = element[i].attributes;
                    //console.log(element[i]);
                   
                   


                    var hT = $(element[i]).offset().top,
                        hH = $(element[i]).outerHeight(),
                        wH = $(window).innerHeight(),
                        wS = $(window).scrollTop();

                    if ((wS) > (hT + hH - wH)) {
                        // console.log(element[i]);
                        self.eachCountUp(element[i], i);
                    }
                }
            }
        });


        //Scrollfunktion funktioniert nur wenn die Seite nicht mit Pixel Null startet 
        //wenn das Feld bei start der Webseite im Sichbereich ist startet es nur wenn gescrollt wird
        //    if ((hT1 - wH1) < 0 && CountUp.hasBeenViewed === true) {
        //    CountUp.eachCountUp();
        //    CountUp.hasBeenViewed = false;

        //}

    }




}

$(document).ready(function () {


    new NumbersCountUp();


});