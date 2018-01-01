

class CountUp {

    i: number = 0;
    hasBeenViewed: boolean = true;
    _class: string = "";



    constructor(_class) {
        this._class = _class;


    }

    addAttributes(vclass: string): string {
        debugger;
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




    countUp(start: number, end: number, id: string, character, _class) {

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

        function giveTheRightHtmlOutput(_class: string, value: number, character: string = "", id: string) {

            if (".CountUp" == _class)
                $(id).html(numberWithCommas(value.toFixed(0)) + character);
            if ('.CountUpProgress' == _class)
                $(id).width(numberWithCommas(value.toFixed(0)) + "%");
        }

        function updateTimer() {



            value += increment;
            loopCount++;

            giveTheRightHtmlOutput(_class, value, character, id);

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
        var c = new CountUp(this._class);
        $(c._class).each(function () {
            var temp = c._class != ".CountUpProgress";
            var temp1 = c._class != ".CountUp";
            if (".CountUp" == c._class && temp)
                c.countUp(0, parseInt(c.addAttributes(this)), this, c.addCharacter(this), c._class);
            if ('.CountUpProgress' === c._class && temp1)
                c.countUp(0, parseInt(c.addAttributes(this)), this, c.addCharacter(this), c._class);

        });
    }


    scrollToTheCountUp(CountUp) {
        var hT1 = $(CountUp._class).offset().top,
            wH1 = $(window).height();
        


            $(window).scroll(function() {
            var hT = $(CountUp._class).offset().top,
                hH = $(CountUp._class).outerHeight(),
                wH = $(window).height(),
                wS = $(window).scrollTop();
                console.log(wS);
                if ((wS) > (hT + hH - wH) && CountUp.hasBeenViewed === true) {
                    CountUp.eachCountUp();
                    CountUp.hasBeenViewed = false;

                }
            });
         //Scrollfunktion funktioniert nur wenn die Seite nicht mit Pixel Null startet 
         //wenn das Feld bei start der Webseite im Sichbereich ist startet es nur wenn gescrollt wird
            if ((hT1 - wH1) < 0 && CountUp.hasBeenViewed === true) {
            CountUp.eachCountUp();
            CountUp.hasBeenViewed = false;

        }


    }


}

$(document).ready(function () {

    console.log($('.CountUp').length);
    if ($('.CountUpProgress').length != 0) {
        var oCountUp = new CountUp('.CountUpProgress');
        oCountUp.scrollToTheCountUp(oCountUp);
    }

    if ($('.CountUp').length != 0) {
        debugger;
        var oCountUp1 = new CountUp('.CountUp');
        oCountUp1.scrollToTheCountUp(oCountUp1);
    }






});