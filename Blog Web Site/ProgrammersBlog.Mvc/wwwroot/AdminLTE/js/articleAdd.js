$(document).ready(function () {



    //Trumbowyg alani
    $('#text-editor').trumbowyg({
        btns: [
            ['viewHTML'],
            ['undo', 'redo'], // Only supported in Blink browsers
            ['formatting'],
            ['strong', 'em', 'del'],
            ['superscript', 'subscript'],
            ['link'],
            ['insertImage'],
            ['justifyLeft', 'justifyCenter', 'justifyRight', 'justifyFull'],
            ['unorderedList', 'orderedList'],
            ['horizontalRule'],
            ['removeformat'],
            ['fullscreen'],
            ['foreColor', 'backColor'],
            ['fontsize'],
            ['fontfamily'],
            ['emoji'],
            ['giphy'],
            ['specialChars']
        ],
        plugins: {
            giphy: {
                apiKey: 'yourVeryOwnApiKey'
            }
        }
    });
    //Trumbowyg sonu

    //Select2 Alani
    $('#categoryList').select2({
        theme: 'bootstrap4',
        placeholder: "Lütfen kategori seciniz",
        allowClear: true
    });
    //Select2 Sonu

    //DatePicker Alani
    $(function () {
        $("#datePicker").datepicker({
            closeText: "kapat",
            prevText: "&#x3C;geri",
            nextText: "ileri&#x3e",
            currentText: "bugün",
            monthNames: ["Ocak", "Şubat", "Mart", "Nisan", "Mayıs", "Haziran",
                "Temmuz", "Ağustos", "Eylül", "Ekim", "Kasım", "Aralık"],
            monthNamesShort: ["Oca", "Şub", "Mar", "Nis", "May", "Haz",
                "Tem", "Ağu", "Eyl", "Eki", "Kas", "Ara"],
            dayNames: ["Pazar", "Pazartesi", "Salı", "Çarşamba", "Perşembe", "Cuma", "Cumartesi"],
            dayNamesShort: ["Pz", "Pt", "Sa", "Ça", "Pe", "Cu", "Ct"],
            dayNamesMin: ["Pz", "Pt", "Sa", "Ça", "Pe", "Cu", "Ct"],
            weekHeader: "Hf",
            dateFormat: "dd.mm.yy",
            firstDay: 1,
            isRTL: false,
            showMonthAfterYear: false,
            yearSuffix: "",
            duration: 600,
            showAnim: "blind",
            showOptions: {direction:"up"},
            minDate: -1,
            maxDate: +1,
            changeMonth: true,
            changeYear: true,
            showOn: "button",
            buttonImage: "/img/calendar.png",
            buttonImageOnly: true,
            buttonText: "Select date"
        });
    });

    //DatePicker Sonu
});