Gosolar = {};
Gosolar.Home = {};

(function (g, $) {
    var self = this;    

    g.Init = function () {
        g.InitMaps();
        g.populateStatesList();
        //$(".contact .btn-submit").bind("click", function(e) {
            //e.preventDefault();
            g.ValidateRules($(this));
        //});

    };

    g.InitMaps = function () {
        /////// office location 1//////
        var office1 = g.InitialMapPoint(25.666037, -100.317083);
        var optionsOffice1 = {
            center: office1,
            zoom: 14,
            mapTypeControl: false,
            streetViewControl: false
        };
        var officeMap1 = g.CreateMap(optionsOffice1, "mapCanvas1");
        var markOptions1 = {
            position: office1,
            map: officeMap1
        };
        g.CreateMarker(markOptions1);
        ///////////////////////////////
        ////// office location 2///////
        var office2 = g.InitialMapPoint(20.707877, -103.415077);
        var optionsOffice2 = {
            center: office2,
            zoom: 14,
            mapTypeControl: false,
            streetViewControl: false
        };
        var officeMap2 = g.CreateMap(optionsOffice2, "mapCanvas2");
        var markOptions2 = {
            position: office2,
            map: officeMap2
        };
        g.CreateMarker(markOptions2);
        ///////////////////////////////
        ////// office location 3 //////
        var office3 = g.InitialMapPoint(20.571138, -100.361708);
        var optionsOffice3 = {
            center: office3,
            zoom: 14,
            mapTypeControl: false,
            streetViewControl: false
        };
        var officeMap3 = g.CreateMap(optionsOffice3, "mapCanvas3");
        var markOptions3 = {
            position: office3,
            map: officeMap3
        };
        g.CreateMarker(markOptions3);
        //////////////////////////////
        ////// office location 4 //////
        var office4 = g.InitialMapPoint(21.194001, -86.818827);
        var optionsOffice4 = {
            center: office4,
            zoom: 14,
            mapTypeControl: false,
            streetViewControl: false
        };
        var officeMap4 = g.CreateMap(optionsOffice4, "mapCanvas4");
        var markOptions4 = {
            position: office4,
            map: officeMap4
        };
        g.CreateMarker(markOptions4);
    };

    g.InitialMapPoint = function (lat, lng) {
        return new google.maps.LatLng(lat, lng);
    };

    g.CreateMap = function (mapOptions, idElement) {
        return new google.maps.Map(document.getElementById(idElement), mapOptions);
    };

    g.CreateMarker = function (markOptions){
        var marker = new google.maps.Marker({
            position: markOptions.position,
            map: markOptions.map,
        });

        marker.setMap(markOptions.map);
    };

    g.populateStatesList = function () {
        var states = g.GetStates();
        var comboStates = "";
        $.each(states, function (i, val) {
            comboStates += '<option value="' + val.text + '">' + val.text + '</option>';
        });
        $(".state").append(comboStates);
    };

    g.ValidateRules = function (el) {
        
        $("#contactForm").validate({
            rules: {
                Name: "required",
                Email: {
                    required: true,
                    email: true
                },
                Phone: "required",
                State: "required",
                Message: "required"
            },
            messages: {
                Name: "Por favor escriba su nombre",
                Email: {
                    required: "Es necesaria su direccion de correo",
                    email: "Por favor escriba un correo valido: [alguien@dominio.com]"
                },
                Phone: "Escriba un numero de telefono",
                State: "Seleccione un estado",
                Message: "Por favor escriba sus comentarios"
            },
            submitHandler: function(el) {
                //form.submit();
                g.SubmitContact(el);
        }
        });
    };

    g.GetStates = function () {
        var states = [
            { id: 1, text: 'Aguascalientes'},
            { id: 2, text: 'Baja California'},
            { id: 3, text: 'Baja California Sur'},
            { id: 4, text: 'Campeche'},
            { id: 5, text: 'Chiapas'},
            { id: 6, text: 'Chihuahua'},
            { id: 7, text: 'Coahuila'},
            { id: 8, text: 'Colima'},
            { id: 9, text: 'Distrito Federal'},
            { id: 10, text: 'Durango'},
            { id: 11, text: 'Estado de México'},
            { id: 12, text: 'Guanajuato'},
            { id: 13, text: 'Guerrero'},
            { id: 14, text: 'Hidalgo'},
            { id: 15, text: 'Jalisco'},
            { id: 16, text: 'Michoacán'},
            { id: 17, text: 'Morelos'},
            { id: 18, text: 'Nayarit'},
            { id: 19, text: 'Nuevo León'},
            { id: 20, text: 'Oaxaca'},
            { id: 21, text: 'Puebla'},
            { id: 22, text: 'Querétaro'},
            { id: 23, text: 'Quintana Roo'},
            { id: 24, text: 'San Luis Potosí'},
            { id: 25, text: 'Sinaloa'},
            { id: 26, text: 'Sonora'},
            { id: 27, text: 'Tabasco'},
            { id: 28, text: 'Tamaulipas'},
            { id: 29, text: 'Tlaxcala'},
            { id: 30, text: 'Veracruz'},
            { id: 31, text: 'Yucatán'},
            { id: 32, text: 'Zacatecas'}];

        return states;
    };

    g.SubmitContact = function (el) {
        //g.ValidateRules();
        g.ShowLoading();
        var form = $("#contactForm");//el;//$(el).parents('form');
        console.log(form.serialize());
        $.ajax({
            type: "POST",
            url: "/Home/ContactForm",//form.attr('action'),
            data: form.serialize(),
            success: function (response) {
                var el = $(".contact");
                if (response == "OK")
                    g.ShowMessage("ok","Su mensaje ha sido enviado, Gracias por sus comentarios!", el);
                else
                    g.ShowMessage("error", "Su mensaje NO ha sido enviado, por favor intentelo de nuevo mas tarde!", el);
                g.ResetElements($("#contactForm"));
                g.HideLoading();
            },
            error: function (xhr, status, error) {
                g.ShowMessage("error", "Su mensaje NO ha sido enviado, por favor intentelo de nuevo mas tarde!", el);
            }
        });
    };

    //parameters: 
    //  type: ok, error
    //  msg: text message to display
    //  el: target element to display
    g.ShowMessage = function (type, msg, el) {

        var markup = $("<div class='message-container'>").prependTo(el);
        markup.append("<div class='backdrop'>");
        if (type == "ok")
            markup.append('<div class="message-content"><h1>' + msg + '</h1></div>');
        if (type == "error")
            markup.append('<div class="message-content error"><h1>' + msg + '</h1></div>');
        el.append(markup);
        setTimeout(function () {
            el.find(".message-container").remove();
        }, 3000);
    };

    g.ShowLoading = function (el) {
        $(".loading-container").show();
    };

    g.HideLoading= function () {
        $(".loading-container").hide();
    };

    //parameters:
    //view/container of elements to reset
    g.ResetElements = function (el) {
        var inputs = el.find("input");
        var selects = el.find("select");
        var textarea = el.find("textarea");
        $.each(inputs, function (i, val) {
            val.value = "";
        });
        $.each(selects, function (s, val) {
            el.find("option:first-child").attr("selected", "selected")
        });
        $.each(textarea, function (t, val) {
            val.value = "";
        });

    };

})(window.Gosolar.Home = window.Gosolar.Home || {}, jQuery);
