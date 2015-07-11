GosolarApp = {};
GosolarApp.Home = {};

(function (g, $) {
    var self = this;    
    var currentPosition = new Object();
    g.Init = function () {       
        g.GetLocation();
        $(".get-address").bind("click", function (e) {
            g.InitialMapGetAddress();
        });
    };

    g.InitialMapGetAddress = function () {
        console.log(currentPosition.lat);
        var address = g.InitialMapPoint(currentPosition.lat, currentPosition.lng);
        var optionsAddress = {
            center: address,
            zoom: 14
            //mapTypeControl: false,
            //streetViewControl: false
        };
        var addressMap = g.CreateMap(optionsAddress, "address-map");
        var markOptions = {
            position: address,
            map: addressMap
        };
        g.CreateMarker(markOptions);
    };

    g.GetLocation = function () {   
        if (navigator.geolocation) {
            navigator.geolocation.getCurrentPosition(g.ShowPosition);
        } else {
            var alert = '<div data-alert class="alert-box alert">Geolocation is not supported by this browser.<a href="#" class="close">&times;</a></div>';
            $(".container").prepend(alert);
        }    
    };

    g.ShowPosition = function (position) {
        currentPosition.lat = position.coords.latitude;
        currentPosition.lng = position.coords.longitude;
    };

    g.InitialMapPoint = function (lat, lng) {
        return new google.maps.LatLng(lat, lng);
    };

    g.CreateMap = function (mapOptions, idElement) {
        return new google.maps.Map(document.getElementById(idElement), mapOptions);
    };

    g.CreateMarker = function (markOptions) {
        var marker = new google.maps.Marker({
            position: markOptions.position,
            map: markOptions.map,
        });

        marker.setMap(markOptions.map);
    };
})(window.GosolarApp.Home = window.GosolarApp.Home || {}, jQuery);
