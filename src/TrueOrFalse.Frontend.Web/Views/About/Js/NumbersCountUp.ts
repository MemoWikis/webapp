

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
            this.eachCountUp();
        }

        if ($('.CountUp').length != 0) {
            this.elem.push($('.CountUp'));
            this.eachCountUp();
        }
       // this.myTestFunction();
       // this.animateWhenVisible();

        

        //if ($('.CountUp').length != 0) {
        //    debugger;
        //    var countUp = new NumbersCountUp('.CountUp');
        //    countUp.scrollToTheCountUp(countUp);
        //}


    }

    myTestFunction() {
        console.log("bin bei testf");
        console.log(this.i);
        console.log(this.elem);

    }

    addAttributes(vclass: string): string {
        var countUp = $(vclass); // alle Attribute einlesen
        console.log(countUp.eq(this.i).attr('data-number'));
        return countUp.eq(this.i).attr('data-number');
    }

    addCharacter(vclass: string): string {
        var countUp = $(vclass); // alle Attribute einlesen


        var character = countUp.eq(this.i).attr('data-character');
        if (character != undefined) {
            return character;
        }
        return "";
    }




    countUp(start: number, end: number, character, _class) {
        debugger;
        var loops = Math.ceil(1000 / 20);
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

        //function giveTheRightHtmlOutput(_class: string, value: number, character: string = "") {

        //    if (".CountUp" == _class)
        //        $(id).html(numberWithCommas(value.toFixed(0)) + character);
        //    if ('.CountUpProgress' == _class)
        //        $(id).width(numberWithCommas(value.toFixed(0)) + "%");
        //}

        function updateTimer() {



            value += increment;
            loopCount++;

            //giveTheRightHtmlOutput(_class, value, character);

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
        debugger;
        console.log(self.elem.length);
        for (let i = 0; i< self.elem.length;i++) {
                //self.elem.each(function () {
                //var temp = self.elem != ".CountUpProgress";
                //var temp1 = self.elem != ".CountUp";
                //      if (".CountUp" == c._class && temp)
                self.countUp(0, parseInt(self.addAttributes(self.elem[i].selector)), self.addCharacter(self.elem[i].selector), self.elem[i].selector);
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
}


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