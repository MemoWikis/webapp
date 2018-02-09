

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
        this.eachCountUp();
       // this.myTestFunction();
       // this.animateWhenVisible();

        

        //if ($('.CountUp').length != 0) {
  
        //    var countUp = new NumbersCountUp('.CountUp');
        //    countUp.scrollToTheCountUp(countUp);
        //}


    }

    deliverHtmlClass(arrayHtmlClasses) {
        let wichHtmlClassBool: boolean = true;
        let htmlClass = ".CountUp";
        //for (let a of arrayHtmlClasses) {
           
        //        if (a == "CountUp" || a == "CountUpProgress")
        //            return a;

        //    }

        wichHtmlClassBool = arrayHtmlClasses.contains("CountUp");
        if (wichHtmlClassBool == false) {
            return ".CountUpProgress";
        }

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
                var  test = "";
                test += htmlClass;
                test += ":eq(" + index + ")";
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

    eachCountUp() {
        //var c = new NumbersCountUp();
        var self = this;
        var e = 0;
        var character = "";

        for (var element of self.elem) {
       // self.elem.forEach(function (value, index, array) {
            for (var i = 0; i < element.length; i++) {
                console.log(e++);
                //console.log(arrayOuter);
                console.log(element);
                console.log(element[i].className);
                console.log(element[i].attributes.getNamedItem("data-number").value);
                try {
                    character = element[i].attributes.getNamedItem("data-character").value;
                    console.log(element[i].attributes.getNamedItem("data-character").value);
                } catch (e) {
                    console.log("is Null");
                    
                } 
                     
                   
                    
               // console.log(element[i].attributes.getNamedItem("data-character").value);
                let htmlClass = self.deliverHtmlClass(element[i].classList);
                let finalnumber = parseInt(element[i].attributes.getNamedItem("data-number").value);
                let startNumber = 0;

                switch (htmlClass) {
                case ".CountUp":
                    self.countUp(startNumber, finalnumber, character, htmlClass, i);
                    break;

                case ".CountUpProgress":
                    self.countUp(startNumber, finalnumber, character, htmlClass, i);
                    break;

                default:
                    console.log("No matching class specified");
                    break;
                }
            }
        }
        //for(let HtmlClassElements of this.elem) {
        //    for (let i = 0; i < HtmlClassElements.length; i++) {
                
        //        self.countUp(0, parseInt(self.addAttributes(HtmlClassElements[i].className)), self.addCharacter(HtmlClassElements[i].className), HtmlClassElements[i].className);
        //        console.log(parseInt(self.addAttributes(HtmlClassElements[i].className)));
        //        console.log(self.addCharacter(HtmlClassElements[i].className));
        //        console.log(HtmlClassElements[i].className);
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


    //animateWhenVisible() {
    //    //var elem: any;
    //    //if (this.isNumber)
    //    //var elem = $('.CountUpProgress');
    //    var self = this;

    //    var hT1 = $(CountUp._class).offset().top;
    //    var wH1 = $(window).height();
        


    //        $(window).scroll(function() {
    //        var hT = self.elem.offset().top,
    //            hH = $(CountUp._class).outerHeight(),
    //            wH = $(window).height(),
    //            wS = $(window).scrollTop();
    //            console.log(wS);
    //            if ((wS) > (hT + hH - wH) && CountUp.hasBeenViewed === true) {
    //                CountUp.eachCountUp();
    //                CountUp.hasBeenViewed = false;

    //            }
    //        });
    //     //Scrollfunktion funktioniert nur wenn die Seite nicht mit Pixel Null startet 
    //     //wenn das Feld bei start der Webseite im Sichbereich ist startet es nur wenn gescrollt wird
    //        if ((hT1 - wH1) < 0 && CountUp.hasBeenViewed === true) {
    //        CountUp.eachCountUp();
    //        CountUp.hasBeenViewed = false;

    //    }


    //}


//}

$(document).ready(function () {

   
    new NumbersCountUp();


});