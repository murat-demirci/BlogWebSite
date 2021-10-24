$(document).ready(function () {

    //Select2 Alani
    $('#categoryList').select2({
        theme: 'bootstrap4',
        placeholder: "Lütfen kategori seciniz",
        allowClear: true
    });

    $('#filterByList').select2({
        theme: 'bootstrap4',
        placeholder: "Lütfen bir filtre seciniz",
        allowClear: true
    });

    $('#orderByList').select2({
        theme: 'bootstrap4',
        placeholder: "Lütfen siralama türü seciniz",
        allowClear: true
    });

    $('#isAscendingList').select2({
        theme: 'bootstrap4',
        placeholder: "Lütfen sirlama tipi seciniz",
        allowClear: true
    });
    //Select2 Sonu

    //DatePicker Alani
    $(function () {
        $("#startAtDatePicker").datepicker({
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
            showOptions: { direction: "up" },
            //minDate: -1,
            maxDate: 0,
            onSelect: function (selectedDate) {
                $("#endAtDatePicker").datepicker('option', 'minDate', selectedDate || getTodaysDate());
            },
            changeMonth: true,
            changeYear: true,
            showOn: "button",
            buttonImage: "/img/calendar.png",
            buttonImageOnly: true,
            buttonText: "Select date"
        });
        $("#endAtDatePicker").datepicker({
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
            showOptions: { direction: "up" },
            //minDate: -1,
            maxDate: 0,
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