Gosolar = {};
Gosolar.Home = {};

(function (g, $) {
    var self = this;    

    g.Init = function () {
        g.InitMaps();
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


})(window.Gosolar.Home = window.Gosolar.Home || {}, jQuery);
