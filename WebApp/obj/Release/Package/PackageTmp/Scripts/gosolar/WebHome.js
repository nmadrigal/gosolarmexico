Gosolar = {};
Gosolar.Home = {};

(function (g, $) {
    var self = this;    

    g.Init = function () {
        g.InitMaps();
        g.populateStatesList();
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
        console.log(states);
        var comboStates = "";
        $.each(states, function (i, val) {
            comboStates += '<option value="' + val.text + '">' + val.text + '</option>';
        });
        $(".state").append(comboStates);
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

})(window.Gosolar.Home = window.Gosolar.Home || {}, jQuery);
